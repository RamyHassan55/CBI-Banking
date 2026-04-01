namespace Creatio.Copilot
{
   
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text.RegularExpressions;
    using Terrasoft.Common;
    using Terrasoft.Core;
    using Terrasoft.Core.Entities;
    using Terrasoft.Core.Factories;
    using Terrasoft.Core.ServiceModelContract;
    using Terrasoft.Nui.ServiceModel.DataContract;
    using DataValueType = Terrasoft.Nui.ServiceModel.DataContract.DataValueType;
    using EntitySchema = Terrasoft.Core.Entities.EntitySchema;
    using EntitySchemaColumn = Terrasoft.Core.Entities.EntitySchemaColumn;
    using FilterType = Terrasoft.Nui.ServiceModel.DataContract.FilterType;

    #region Serializable types

    internal class SerializableFilters : SerializableFilter
    {

        #region Properties: Public

        public string RootSchemaName { get; set; }

        #endregion

    }

    internal class SerializableFilter
    {

        #region Properties: Public

        public FilterType FilterType { get; set; }

        public FilterComparisonType? ComparisonType { get; set; }

        public LogicalOperationStrict? LogicalOperation { get; set; }

        public bool? IsNull { get; set; }

        public bool IsEnabled { get; set; } = true;

        public bool? IsNot { get; set; }

        public SerializableFilters SubFilters { get; set; }

        public Dictionary<string, SerializableFilter> Items { get; set; }

        public SerializableExpression LeftExpression { get; set; }

        public SerializableExpression RightExpression { get; set; }

        public SerializableExpression[] RightExpressions { get; set; }

        public SerializableExpression RightLessExpression { get; set; }

        public SerializableExpression RightGreaterExpression { get; set; }

        public bool? TrimDateTimeParameterToDate { get; set; }

        public string Key { get; set; }

        public bool? IsAggregative { get; set; }

        public string LeftExpressionCaption { get; set; }

        public string ReferenceSchemaName { get; set; }

        #endregion

    }

    #endregion

    internal interface ILlmEsqFiltersConverter
    {
        SerializableFilter Convert(LLMUnknownFilterResponseContract inputFilter, string rootSchemaName, int parentIndex = 0);
        SerializableFilter ConvertBackwardReferenceFitler(LLMUnknownFilterResponseContract inputFilter, string rootSchemaName, int parentIndex = 0);
    }

    [DefaultBinding(typeof(ILlmEsqFiltersConverter))]
    internal class LlmEsqFiltersConverter : ILlmEsqFiltersConverter
    {
        #region: Fields: Private

        private readonly UserConnection _userConnection = ClassFactory.Get<UserConnection>();
        
        private readonly Regex _guidRegex =
            new Regex(@"^([0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12})$",
                RegexOptions.IgnoreCase);

        private readonly Regex _macroBasedValueRegex =
            new Regex(@"^(PREVIOUS|CURRENT|NEXT)_(HOUR|DAY|WEEK|MONTH|QUARTER|HALFYEAR|YEAR)(\(\d*\))?$",
                RegexOptions.IgnoreCase);

        private readonly Regex _macroWithParameterValueRegex =
            new Regex(
                @"^(WITHIN_PREV_HOURS|WITHIN_NEXT_HOURS|WITHIN_PREV_DAYS|WITHIN_NEXT_DAYS|ANNIVERSARY_TODAY|ANNIVERSARY_WITHIN_NEXTDAYS|ANNIVERSARY_WITHIN_PREVDAYS|ANNIVERSARY_EXACTLY_IN_DAYS)(\(\d*\))?$",
                RegexOptions.IgnoreCase);
        
        #endregion

        #region Methods: Private
        
        private SerializableFilter CastToNullFilter(LLMUnknownFilterResponseContract filterResponseContract, bool isNull) {
            string columnPath = FixPath(filterResponseContract.columnPath);
            return new SerializableFilter {
                FilterType = FilterType.IsNullFilter,
                LeftExpression = new SerializableExpression {
                    ColumnPath = columnPath,
                    ExpressionType = EntitySchemaQueryExpressionType.SchemaColumn
                },
                ComparisonType = isNull ? FilterComparisonType.IsNull : FilterComparisonType.IsNotNull,
                IsNull = isNull,
                IsEnabled = true
            };
        }

        private EntitySchemaQueryMacrosType ParseMacroType(string value) {
            switch (value.ToUpper().Replace("()", "")) {
                case "ANNIVERSARY_TODAY":
                    return EntitySchemaQueryMacrosType.DayOfYearToday;

                // PREVIOUS - like
                case "PREVIOUS_HOUR":
                    return EntitySchemaQueryMacrosType.PreviousHour;
                case "PREVIOUS_DAY":
                    return EntitySchemaQueryMacrosType.Yesterday;
                case "PREVIOUS_WEEK":
                    return EntitySchemaQueryMacrosType.PreviousWeek;
                case "PREVIOUS_MONTH":
                    return EntitySchemaQueryMacrosType.PreviousMonth;
                case "PREVIOUS_QUARTER":
                    return EntitySchemaQueryMacrosType.PreviousQuarter;
                case "PREVIOUS_HALFYEAR":
                    return EntitySchemaQueryMacrosType.PreviousHalfYear;
                case "PREVIOUS_YEAR":
                    return EntitySchemaQueryMacrosType.PreviousYear;

                // CURRENT - like
                case "CURRENT_HOUR":
                    return EntitySchemaQueryMacrosType.CurrentHour;
                case "CURRENT_DAY":
                    return EntitySchemaQueryMacrosType.Today;
                case "CURRENT_WEEK":
                    return EntitySchemaQueryMacrosType.CurrentWeek;
                case "CURRENT_MONTH":
                    return EntitySchemaQueryMacrosType.CurrentMonth;
                case "CURRENT_QUARTER":
                    return EntitySchemaQueryMacrosType.CurrentQuarter;
                case "CURRENT_HALFYEAR":
                    return EntitySchemaQueryMacrosType.CurrentHalfYear;
                case "CURRENT_YEAR":
                    return EntitySchemaQueryMacrosType.CurrentYear;

                // NEXT - like
                case "NEXT_HOUR":
                    return EntitySchemaQueryMacrosType.NextHour;
                case "NEXT_DAY":
                    return EntitySchemaQueryMacrosType.Tomorrow;
                case "NEXT_WEEK":
                    return EntitySchemaQueryMacrosType.NextWeek;
                case "NEXT_MONTH":
                    return EntitySchemaQueryMacrosType.NextMonth;
                case "NEXT_QUARTER":
                    return EntitySchemaQueryMacrosType.NextQuarter;
                case "NEXT_HALFYEAR":
                    return EntitySchemaQueryMacrosType.NextHalfYear;
                case "NEXT_YEAR":
                    return EntitySchemaQueryMacrosType.NextYear;
                default:
                    throw new ArgumentException($"Unsupported macro type: {value}");
            }
        }

        /// <summary>
        /// Cover marco without additional parameters
        /// </summary>
        /// <param name="macrosType"></param>
        /// <param name="inputFilterResponseContract"></param>
        /// <returns name="Filter"></returns>
        private SerializableFilter GenerateMacroConfig(EntitySchemaQueryMacrosType macrosType,
            LLMUnknownFilterResponseContract inputFilterResponseContract) {
            return new SerializableFilter {
                FilterType = FilterType.CompareFilter,
                ComparisonType = GetComparisonType(inputFilterResponseContract.comparisonType),
                IsEnabled = true,
                LeftExpression = new SerializableExpression {
                    ExpressionType = EntitySchemaQueryExpressionType.SchemaColumn,
                    ColumnPath = inputFilterResponseContract.columnPath
                },
                RightExpression = new SerializableExpression {
                    ExpressionType = EntitySchemaQueryExpressionType.Function,
                    FunctionType = FunctionType.Macros,
                    MacrosType = macrosType
                },
            };
        }

        /// <summary>
        /// Cover relative date filter with parameter configuration
        /// </summary>
        /// <param name="inputFilterResponseContract"></param>
        /// <returns name="Filter"></returns>
        private SerializableFilter GenerateRelativeDateParameterConfig(LLMUnknownFilterResponseContract inputFilterResponseContract) {
            DatePart datePart;
            string value = inputFilterResponseContract.value as string;
            if (value.Contains("DAY_OF_WEEK")) {
                datePart = DatePart.Weekday;
            } else if (value.Contains("DAY_OF_MONTH")) {
                datePart = DatePart.Day;
            } else if (value.Contains("MONTH")) {
                datePart = DatePart.Month;
            } else if (value.Contains("EXACT_YEAR")) {
                datePart = DatePart.Year;
            } else {
                 throw new Exception("Unsupported date part in value: " + value);
            }
            return new SerializableFilter {
                FilterType = FilterType.CompareFilter,
                ComparisonType = FilterComparisonType.Equal,
                IsEnabled = true,
                LeftExpression = new SerializableExpression {
                    ExpressionType = EntitySchemaQueryExpressionType.Function,
                    FunctionType = FunctionType.DatePart,
                    FunctionArgument = new SerializableExpression {
                        ExpressionType = EntitySchemaQueryExpressionType.SchemaColumn,
                        ColumnPath = inputFilterResponseContract.columnPath
                    },
                    DatePartType = datePart
                },
                RightExpression = new SerializableExpression {
                    ExpressionType = EntitySchemaQueryExpressionType.Parameter,
                    Parameter = new SerializableParameter {
                        DataValueType = DataValueType.Integer,
                        Value = ExtractNumberFromFunction(value)
                    }
                },
            };
        }

        /// <summary>
        /// Cover marco with additional parameters
        /// </summary>
        /// <param name="inputFilterResponseContract"></param>
        /// <returns name="Filter"></returns>
        private SerializableFilter GenerateMacroWithParameterConfig(LLMUnknownFilterResponseContract inputFilterResponseContract) {
            // important! index of record matters! in this method we used it later
            string[] patterns = {
                @"^WITHIN_PREV_HOURS(\(\d*\))?$",
                @"^WITHIN_NEXT_HOURS(\(\d*\))?$",
                @"^WITHIN_PREV_DAYS(\(\d*\))?$",
                @"^WITHIN_NEXT_DAYS(\(\d*\))?$",
                @"^ANNIVERSARY_WITHIN_NEXTDAYS(\(\d*\))?$",
                @"^ANNIVERSARY_WITHIN_PREVDAYS(\(\d*\))?$",
                @"^ANNIVERSARY_EXACTLY_IN_DAYS(\(\d*\))?$"
            };
            string inputValue = inputFilterResponseContract.value as string;
            if (string.IsNullOrEmpty(inputValue)) {
                throw new ArgumentException("Input filter value must be a non-empty string");
            }
            int matchedPatternIndex = -1;
            Match successfulMatch = null;
            for (int i = 0; i < patterns.Length; i++) {
                var regex = new Regex(patterns[i], RegexOptions.IgnoreCase);
                var match = regex.Match(inputValue);
                if (match.Success) {
                    matchedPatternIndex = i;
                    successfulMatch = match;
                    break;
                }
            }
            if (successfulMatch == null) {
                throw new ArgumentException($"Input value '{inputValue}' does not match any expected pattern");
            }
            EntitySchemaQueryMacrosType macrosType;
            switch (matchedPatternIndex) {
                case 0:
                    macrosType = EntitySchemaQueryMacrosType.PreviousNHours;
                    break;
                case 1:
                    macrosType = EntitySchemaQueryMacrosType.NextNHours;
                    break;
                case 2:
                    macrosType = EntitySchemaQueryMacrosType.PreviousNDays;
                    break;
                case 3:
                    macrosType = EntitySchemaQueryMacrosType.NextNDays;
                    break;
                case 4:
                    macrosType = EntitySchemaQueryMacrosType.NextNDaysOfYear;
                    break;
                case 5:
                    macrosType = EntitySchemaQueryMacrosType.PreviousNDaysOfYear;
                    break;
                case 6:
                    macrosType = EntitySchemaQueryMacrosType.DayOfYearTodayPlusDaysOffset;
                    break;
                default:
                    throw new ArgumentException("Invalid macro index");
            }
            return new SerializableFilter {
                FilterType = FilterType.CompareFilter,
                ComparisonType = GetComparisonType(inputFilterResponseContract.comparisonType),
                IsEnabled = true,
                LeftExpression = new SerializableExpression {
                    ExpressionType = EntitySchemaQueryExpressionType.SchemaColumn,
                    ColumnPath = inputFilterResponseContract.columnPath
                },
                RightExpression = new SerializableExpression {
                    ExpressionType = EntitySchemaQueryExpressionType.Function,
                    FunctionType = FunctionType.Macros,
                    MacrosType = macrosType,
                    FunctionArgument = new SerializableExpression {
                        ExpressionType = EntitySchemaQueryExpressionType.Parameter,
                        Parameter = new SerializableParameter {
                            DataValueType = DataValueType.Integer,
                            Value = ExtractNumberFromFunction(inputValue)
                        }
                    }
                },
            };
        }

        /// <summary>
        /// extracts number from function string like "PREVIOUS_HOUR(3)" -> 3
        /// </summary>
        /// <param name="value"></param>
        /// <returns name="int"></returns>
        private int ExtractNumberFromFunction(string value) {
            if (string.IsNullOrEmpty(value) || !value.Contains("(") || !value.EndsWith(")")) {
                throw new ArgumentException($"Input value '{value}' does not contain a valid parameter");
            }
            var numLength = value.IndexOf(")") - value.IndexOf("(") - 1;
            string numberString = value.Substring(value.IndexOf("(") + 1, numLength);
            if (numberString.Length < 1){
                throw new ArgumentException($"Input value '{value}'does not contain a valid parameter of passed function");
            }
            return int.Parse(numberString);
        }
        
        private bool IsRelativeDateFilter(string value) {
            return _macroBasedValueRegex.IsMatch(value) || _macroWithParameterValueRegex.IsMatch(value) ||
                   value.Contains("EXACT_TIME") || value.Contains("EXACT_YEAR") || value.Contains("DAY_OF_WEEK") || value.Contains("DAY_OF_MONTH") || value.Contains("MONTH");
        }

        private SerializableFilter GenerateRelativeDateFilter(LLMUnknownFilterResponseContract filterResponseContract) {
            var value = filterResponseContract.value as string;
            if (value.Contains("EXACT_TIME")) {
                int start = value.IndexOf("(") + 1;
                int end = value.IndexOf(")");
                int[] values = value.Substring(start, end - start).Split(',').Select(v => int.Parse(v.Trim()))
                    .ToArray();
                var userTime = new DateTime(
                    DateTime.Today.Year,
                    DateTime.Today.Month,
                    DateTime.Today.Day,
                    values[0],
                    values[1],
                    values[2]
                );
                var userTimeZone = _userConnection.CurrentUser.TimeZone;
                var utcTime = TimeZoneInfo.ConvertTimeToUtc(userTime, userTimeZone);
                return new SerializableFilter {
                    FilterType = FilterType.CompareFilter,
                    ComparisonType = FilterComparisonType.Equal,
                    IsEnabled = true,
                    LeftExpression = new SerializableExpression {
                        ExpressionType = EntitySchemaQueryExpressionType.Function,
                        FunctionType = FunctionType.DatePart,
                        FunctionArgument = new SerializableExpression {
                            ExpressionType = EntitySchemaQueryExpressionType.SchemaColumn,
                            ColumnPath = filterResponseContract.columnPath
                        },
                        DatePartType = DatePart.HourMinute
                    },
                    RightExpression = new SerializableExpression {
                        ExpressionType = EntitySchemaQueryExpressionType.Parameter,
                        Parameter = new SerializableDateParameter {
                            DataValueType = DataValueType.Time,
                            Value = "\"" + userTime.ToString("yyyy-MM-ddTHH:mm:ss.fff") + "\"",
                            DateValue = utcTime.ToString("yyyy-MM-ddTHH:mm:ss.fff") + "Z",
                        }
                    },
                };
            }
            var match = _macroBasedValueRegex.Match(value);
            if (match.Success || value == "ANNIVERSARY_TODAY()") {
                EntitySchemaQueryMacrosType macroType = ParseMacroType(value);
                return GenerateMacroConfig(macroType, filterResponseContract);
            }
            var anniversaryMatch = _macroWithParameterValueRegex.Match(value);
            if (anniversaryMatch.Success) {
                return GenerateMacroWithParameterConfig(filterResponseContract);
            }
            return GenerateRelativeDateParameterConfig(filterResponseContract);
        }

        private DataValueType GetDataValueType(object value) {
            if (value == null) {
                throw new ArgumentNullException(nameof(value), "Value cannot be null");
            }
            if (value is bool) {
                return DataValueType.Boolean;
            }
            if (value is string str) {
                if (DateTime.TryParse(str, CultureInfo.InvariantCulture,
                        DateTimeStyles.AssumeUniversal, out _)) {
                    return DataValueType.DateTime;
                }
                return DataValueType.Text;
            }
            if (value is int || value is long) {
                return DataValueType.Integer;
            }
            if (value is float || value is double) {
                return DataValueType.Float;
            }
            throw new InvalidOperationException("Unsupported value type");
        }

        private SerializableExpression
            GenerateRightExpression(LLMUnknownFilterResponseContract filterResponseContract) {
            var dataValueType = GetDataValueType(filterResponseContract.value);
            var parameter = dataValueType == DataValueType.DateTime
                ? new SerializableDateParameter {
                    DataValueType = DataValueType.DateTime,
                    Value = "\"" + filterResponseContract.value + "\"",
                    DateValue = filterResponseContract.value as string  // TODO: ### #### #####, ## ### ### # ######### Parameter!!!
                }
                : new SerializableParameter {
                    DataValueType = dataValueType,
                    Value = filterResponseContract.value
                };
            return new SerializableExpression {
                ExpressionType = EntitySchemaQueryExpressionType.Parameter,
                Parameter = parameter
            };
        }

        private FilterComparisonType GetComparisonType(string llmComparisonType) {
            switch (llmComparisonType.ToUpper()) {
                case "IS_NULL":
                    return FilterComparisonType.IsNull;
                case "IS_NOT_NULL":
                    return FilterComparisonType.IsNotNull;
                case "EQUAL":
                    return FilterComparisonType.Equal;
                case "NOT_EQUAL":
                    return FilterComparisonType.NotEqual;
                case "LESS":
                    return FilterComparisonType.Less;
                case "LESS_OR_EQUAL":
                    return FilterComparisonType.LessOrEqual;
                case "GREATER":
                    return FilterComparisonType.Greater;
                case "GREATER_OR_EQUAL":
                    return FilterComparisonType.GreaterOrEqual;
                case "START_WITH":
                    return FilterComparisonType.StartWith;
                case "NOT_START_WITH":
                    return FilterComparisonType.NotStartWith;
                case "CONTAIN":
                    return FilterComparisonType.Contain;
                case "NOT_CONTAIN":
                    return FilterComparisonType.NotContain;
                case "END_WITH":
                    return FilterComparisonType.EndWith;
                case "NOT_END_WITH":
                    return FilterComparisonType.NotEndWith;
                case "EXISTS":
                    return FilterComparisonType.Exists;
                case "NOT_EXISTS":
                    return FilterComparisonType.NotExists;
                default:
                    throw new ArgumentException($"Unsupported comparison type: {llmComparisonType}");
            }
        }

        private SerializableFilter CastToCompareFilter(LLMUnknownFilterResponseContract filterResponseContract) {
            var outputFilter = new SerializableFilter {
                ComparisonType = GetComparisonType(filterResponseContract.comparisonType),
                FilterType = FilterType.CompareFilter,
                IsEnabled = true,
                LeftExpression = new SerializableExpression {
                    ColumnPath = FixPath(filterResponseContract.columnPath),
                    ExpressionType = EntitySchemaQueryExpressionType.SchemaColumn
                },
                RightExpression = GenerateRightExpression(filterResponseContract),
            };
            if (GetDataValueType(filterResponseContract.value) == DataValueType.DateTime) {
                outputFilter.TrimDateTimeParameterToDate = true;
            }
            return outputFilter;
        }

        private string GetDisplayValue(EntitySchema entitySchema, string id) {
            var primaryColumnName = entitySchema.GetPrimaryColumnName();
            var primaryDisplayColumnName = entitySchema.GetPrimaryDisplayColumnName();
            var esq = new EntitySchemaQuery(_userConnection.EntitySchemaManager, entitySchema.Name);
            esq.AddColumn(primaryDisplayColumnName);
            esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, primaryColumnName, id));
            var entities = esq.GetEntityCollection(_userConnection);
            if (entities.Count > 0) {
                return entities[0].GetTypedColumnValue<string>(primaryDisplayColumnName);
            }
            return null;
        }

        private string FixPath(string path) {
            var x = path.Split('.').Select(part =>
                part.Length > 2 && part.EndsWith("Id") ? part.Substring(0, part.Length - 2) : part);
            return string.Join(".", x);
        }

        private SerializableFilter CastToLookupFilter(LLMUnknownFilterResponseContract filterResponseContract,
            string entitySchemaName) {
            EntitySchema entitySchema = _userConnection.EntitySchemaManager.GetInstanceByName(entitySchemaName);
            string path = FixPath(filterResponseContract.columnPath);
            EntitySchemaColumn schemaColumn = entitySchema.GetSchemaColumnByPath(path);
            EntitySchema referenceSchema = schemaColumn.ReferenceSchema;
            var rightExpressions = new List<SerializableExpression>();
            string[] ids;
            if (filterResponseContract.value is string value) {
                ids = new[] { value };
            } else if (filterResponseContract.value is string[] strings) {
                ids = strings;
            } else if (filterResponseContract.value is object[] objects && objects.All(x => x is string)) {
                ids = objects.Cast<string>().ToArray();
            } else {
                throw new ArgumentException("Unsupported value type for lookup filter");
            }
            foreach (string id in ids) {
                rightExpressions.Add(new SerializableExpression {
                    ExpressionType = EntitySchemaQueryExpressionType.Parameter,
                    Parameter = new SerializableParameter {
                        DataValueType = DataValueType.Lookup,
                        Value = new LookupValue {
                            Value = Guid.Parse(id),
                            DisplayValue = GetDisplayValue(referenceSchema, id)
                        }
                    }
                });
            }
            return new SerializableFilter {
                ComparisonType = GetComparisonType(filterResponseContract.comparisonType),
                FilterType = FilterType.InFilter,
                IsEnabled = true,
                LeftExpression = new SerializableExpression {
                    ColumnPath = path,
                    ExpressionType = EntitySchemaQueryExpressionType.SchemaColumn
                },
                ReferenceSchemaName = referenceSchema.Name,
                RightExpressions = rightExpressions.ToArray(),
            };
        }

        private SerializableFilter CastToRelativeDateFilter(LLMUnknownFilterResponseContract filterResponseContract) {
            return GenerateRelativeDateFilter(filterResponseContract);
        }

        private SerializableFilters CastToFilterGroup(LLMUnknownFilterResponseContract inputFilter, string rootSchemaName, int parentIndex) {
            var items = new Dictionary<string, SerializableFilter>();
            for (var i = 0; i < inputFilter.filters.Count; i++) {
                var filter = inputFilter.filters[i];
                string key = string.Concat("Filter_", parentIndex, "_", i);
                items.Add(key, Convert(filter, rootSchemaName));
            }
            return new SerializableFilters {
                FilterType = FilterType.FilterGroup,
                IsEnabled = true,
                LogicalOperation = inputFilter.logicalOperation == "AND"
                    ? LogicalOperationStrict.And
                    : LogicalOperationStrict.Or,
                Items = items,
				Key = new Guid().ToString(),
                RootSchemaName = rootSchemaName
            };
        }
        
        private bool IsLookupFilter(LLMUnknownFilterResponseContract inputFilter, string entitySchemaName) {
            EntitySchema entitySchema = _userConnection.EntitySchemaManager.GetInstanceByName(entitySchemaName);
            string path = FixPath(inputFilter.columnPath);
            EntitySchemaColumn schemaColumn = entitySchema.GetSchemaColumnByPath(path);
            return schemaColumn.DataValueType.IsLookup;
        }

        /// <summary>
        /// sometimes LLM builds incorrect path (it may happen when we have backward reference filter + subfilter with lookup)
        /// Here we fix such behavior. example [Order:ContactId].Id -> [Order:Contact].Id
        /// </summary>
        /// <param name="input"></param>
        private string ValidateAndFixSchemaPath(string input)
        {
           return Regex.Replace(input, @"(:[A-Za-z0-9]+)Id(\])", "$1$2");
        }

        #endregion

        public SerializableFilter Convert(LLMUnknownFilterResponseContract inputFilter, string rootSchemaName,
            int parentIndex = 0) {
            if (inputFilter.logicalOperation != null) {
                return CastToFilterGroup(inputFilter, rootSchemaName, parentIndex);
            } else {
                if (inputFilter.comparisonType == "IS_NULL") {
                    return CastToNullFilter(inputFilter, true);
                }
                if (inputFilter.comparisonType == "IS_NOT_NULL") {
                    return CastToNullFilter(inputFilter, false);
                }
                if (inputFilter.value is string strVal && IsRelativeDateFilter(strVal)) {
                    return CastToRelativeDateFilter(inputFilter);
                }
                if (IsLookupFilter(inputFilter, rootSchemaName)) {
                    return CastToLookupFilter(inputFilter, rootSchemaName);
                }
                return CastToCompareFilter(inputFilter);
            }
        }

        public SerializableFilter ConvertBackwardReferenceFitler(LLMUnknownFilterResponseContract inputFilter, string rootSchemaName,
            int parentIndex = 0){

            var outputFilter = new SerializableFilter()
            {
                IsEnabled = true,
                IsAggregative = true
            };

            inputFilter.columnPath = ValidateAndFixSchemaPath(inputFilter.columnPath);

            var backwardSchemaName = inputFilter.columnPath.Substring(
                inputFilter.columnPath.IndexOf('[') + 1,
                inputFilter.columnPath.IndexOf(':') - inputFilter.columnPath.IndexOf('[') - 1
                );

            SerializableFilters subFilters = CastToFilterGroup(inputFilter.subFilters, backwardSchemaName, parentIndex * 100);
            outputFilter.SubFilters = subFilters;
            outputFilter.LeftExpression = new SerializableExpression
            {
                ColumnPath = FixPath(inputFilter.columnPath),
                SubFilters = subFilters
            };

            if (inputFilter.aggregationType == "EXISTS" || inputFilter.aggregationType == "NOT_EXISTS") {
                outputFilter.FilterType = FilterType.Exists;
                outputFilter.ComparisonType = inputFilter.aggregationType == "EXISTS" ? FilterComparisonType.Exists : FilterComparisonType.NotExists;

                outputFilter.LeftExpression.ExpressionType = EntitySchemaQueryExpressionType.SchemaColumn;
            } else {
                outputFilter.FilterType = FilterType.CompareFilter;
                outputFilter.ComparisonType = this.GetComparisonType(inputFilter.comparisonType);
                // value can be int or  ISO date or relative date
                if (inputFilter.aggregationValue is string str && double.TryParse(str, out var number))
                {
                    inputFilter.value = number;
                }else if(inputFilter.aggregationValue is null)
                {
                    inputFilter.value = "";
                }else
                {
                    inputFilter.value = inputFilter.aggregationValue;
                }
                    outputFilter.RightExpression = this.Convert(inputFilter, backwardSchemaName).RightExpression;

                AggregationType aggregationType;
                switch (inputFilter.aggregationType)
                {
                    case "COUNT":
                        aggregationType = AggregationType.Count;
                        break;
                    case "SUM":
                        aggregationType = AggregationType.Sum;
                        break;
                    case "AVG":
                        aggregationType = AggregationType.Avg;
                        break;
                    case "MIN":
                        aggregationType = AggregationType.Min;
                        break;
                    case "MAX":
                        aggregationType = AggregationType.Max;
                        break;
                    default:
                        throw new ArgumentException($"Unsupported aggregation function: {inputFilter.aggregationType}");
                }

                outputFilter.LeftExpression.AggregationType = aggregationType;
                outputFilter.LeftExpression.FunctionType = FunctionType.Aggregation;
                outputFilter.LeftExpression.ExpressionType = EntitySchemaQueryExpressionType.SubQuery;
            }


;


            return outputFilter;
        }
    }

}


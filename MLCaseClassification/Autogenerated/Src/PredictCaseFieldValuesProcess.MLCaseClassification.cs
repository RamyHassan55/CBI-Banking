namespace Terrasoft.Core.Process
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Drawing;
	using System.Globalization;
	using System.Linq;
	using System.Text;
	using Terrasoft.Common;
	using Terrasoft.Configuration.ML;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;
	using Terrasoft.Core.DB;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Factories;
	using Terrasoft.Core.Process;
	using Terrasoft.Core.Process.Configuration;
	using Terrasoft.ML.Interfaces;

	#region Class: PredictCaseFieldValuesProcessMethodsWrapper

	/// <exclude/>
	public class PredictCaseFieldValuesProcessMethodsWrapper : ProcessModel
	{

		public PredictCaseFieldValuesProcessMethodsWrapper(Process process)
			: base(process) {
			AddScriptTaskMethod("ExecutePredictionScriptTaskExecute", ExecutePredictionScriptTaskExecute);
		}

		#region Methods: Private

		private bool ExecutePredictionScriptTaskExecute(ProcessExecutingContext context) {
			var userConnection = Get<UserConnection>("UserConnection");
			Guid entityId = Get<Guid>("StartCaseCreatedSignal.RecordId");
			
			Guid groupModelId, priorityModelId, serviceModelId;
			(groupModelId, priorityModelId, serviceModelId) = GetNewestCaseModels(userConnection);
			
			var connectionArg = new ConstructorArgument("userConnection", userConnection);
			
			var predictor = ClassFactory.Get<MLEntityPredictor>(connectionArg);
			var predictedValues = predictor.ClassifyEntityValues(new List<Guid> {
				groupModelId,
				priorityModelId,
				serviceModelId
			}, entityId);
			var saver = ClassFactory.Get<MLPredictionSaver>(connectionArg);
			var filter = ClassFactory.Get<MLBasePredictedValueFilter>(connectionArg);
			saver.SaveEntityPredictedValues(predictor.ModelRootSchemaUId, entityId, predictedValues, filter.OnSetupPredictedValue);
			return true;
		}

			private bool OnSetPredictedValue(Entity entity, string columnValueName, ClassificationResult result) {
				if (columnValueName == "ServiceItemId") {
					return OnSetPredictedServiceItem(entity, result);
				}
				return true;
			}
			
			private bool OnSetPredictedServiceItem(Entity entity, ClassificationResult result) {
				////TODO #CRM-34721 Devide into 2 packages: for Customer Center and Service Enterprise
				if (entity.FindEntityColumnValue("ServicePactId") == null) {
					return true;
				}
				UserConnection userConnection = Get<UserConnection>("UserConnection");
				Guid.TryParse(result.Value, out var serviceItemId);
				Guid servicePactId = entity.GetTypedColumnValue<Guid>("ServicePactId");
				var select = (Select)new Select(userConnection)
						.Column(Column.Const(1))
					.From("ServiceInServicePact").As("sp")
					.InnerJoin("ServiceStatus").As("ss").On("sp", "StatusId").IsEqual("ss", "Id")
					.Where("sp", "ServiceItemId").IsEqual(Column.Parameter(serviceItemId, "Guid"))
						.And("sp", "ServicePactId").IsEqual(Column.Parameter(servicePactId, "Guid"))
						.And("ss", "Active").IsEqual(Column.Parameter(true, "Boolean"));
				try {
					return select.ExecuteScalar<int>() > 0;
				} catch (Exception e) {
					return false;
				}
			}
			
			private static T GetValue<T>(ICompositeObject compositeObject, string key) {
				if (compositeObject.TryGetValue(key, out T value)) {
					return value;
				}
				return default(T);
			}
			
			private (Guid GroupModelId, Guid PriorityModelId, Guid ServiceModelId) GetNewestCaseModels(UserConnection userConnection) {
				var caseModels = Get<ICompositeObjectList<ICompositeObject>>("ReadDataUserTask.ResultCompositeObjectList");
				
				IDictionary<Guid, DateTime> modelsCreationDictionary = caseModels.ToDictionary(
					caseModelInfo => GetValue<Guid>(caseModelInfo, "Id"),
					caseModelInfo => GetValue<DateTime>(caseModelInfo, "CreatedOn")
				);
				IMLModelLoader modelLoader = ClassFactory.Get<IMLModelLoader>();
				var ids = modelsCreationDictionary.Keys.ToList();
				if (ids.Count == 0) {
					return (Guid.Empty, Guid.Empty, Guid.Empty);
				}
				var caseModelConfigs = modelLoader.LoadEnabledModels(userConnection, modelsCreationDictionary.Keys.ToList());
			
				var groupModelIds = caseModelConfigs.Where(item => item.PredictedResultColumnName == "GroupId")
					.Select(item => item.Id);
				Guid groupModelId =
					groupModelIds.OrderByDescending(id => modelsCreationDictionary[id]).FirstOrDefault();
			
				var priorityModelIds = caseModelConfigs.Where(item => item.PredictedResultColumnName == "PriorityId")
					.Select(item => item.Id);
				Guid priorityModelId =
					priorityModelIds.OrderByDescending(id => modelsCreationDictionary[id]).FirstOrDefault();
			
				var serviceModelIds = caseModelConfigs.Where(item => item.PredictedResultColumnName == "ServiceItemId")
					.Select(item => item.Id);
				Guid serviceModelId =
					serviceModelIds.OrderByDescending(id => modelsCreationDictionary[id]).FirstOrDefault();
			
				return (groupModelId, priorityModelId, serviceModelId);
			}

		#endregion

	}

	#endregion

}


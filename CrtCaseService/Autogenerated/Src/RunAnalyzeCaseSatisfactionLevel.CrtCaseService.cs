namespace Terrasoft.Core.Process
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Drawing;
	using System.Globalization;
	using System.Text;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;
	using Terrasoft.Core.DB;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Process;
	using Terrasoft.Core.Process.Configuration;

	#region Class: RunAnalyzeCaseSatisfactionLevelSchema

	/// <exclude/>
	public class RunAnalyzeCaseSatisfactionLevelSchema : Terrasoft.Core.Process.ProcessSchema
	{

		#region Constructors: Public

		public RunAnalyzeCaseSatisfactionLevelSchema(ProcessSchemaManager processSchemaManager)
			: base(processSchemaManager) {
		}

		public RunAnalyzeCaseSatisfactionLevelSchema(RunAnalyzeCaseSatisfactionLevelSchema source)
			: base(source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			Name = "RunAnalyzeCaseSatisfactionLevel";
			UId = new Guid("4898f7ea-06ca-401a-9cd3-bbb193f916c1");
			CreatedInPackageId = new Guid("30de0e3b-0a22-4b21-aec2-eb5dee3a342f");
			CreatedInSchemaUId = Guid.Empty;
			CreatedInVersion = @"7.8.0.0";
			CultureName = @"en-US";
			EntitySchemaUId = Guid.Empty;
			IsActiveVersion = false;
			IsCreatedInSvg = true;
			ModifiedInSchemaUId = Guid.Empty;
			ParametersEditPageSchemaUId = Guid.Empty;
			ParentSchemaUId = new Guid("bb4d6607-026b-4b27-b640-8f5c77c1e89d");
			SequenceFlowStrokeDefColor = Color.FromArgb(-4473925);
			SerializeToDB = true;
			SerializeToMemory = true;
			Tag = @"Business Process";
			TaskFillDefColor = Color.FromArgb(-1);
			UsageType = ProcessSchemaUsageType.Advanced;
			UseForceCompile = true;
			ZipMethodsBody = new byte[] {  };
			ZipCompiledMethodsBody = new byte[] {  };
			RealUId = new Guid("4898f7ea-06ca-401a-9cd3-bbb193f916c1");
			Version = 0;
			PackageUId = new Guid("335c46ac-da94-33b1-a9b5-03a3f3644fe8");
			UseSystemSecurityContext = false;
		}

		protected virtual ProcessSchemaParameter CreateCaseRecordIdParameter() {
			return new ProcessSchemaParameter(this) {
			UId = new Guid("c38a0af2-cfe7-436f-8d64-87341f47d144"),
				ContainerUId = Guid.Empty,
				CreatedInOwnerSchemaUId = Guid.Empty,
				CreatedInPackageId = Guid.Empty,
				CreatedInSchemaUId = new Guid("4898f7ea-06ca-401a-9cd3-bbb193f916c1"),
				Direction = ProcessSchemaParameterDirection.Variable,
				IsCopyValue = false,
				IsLazy = false,
				IsPerformer = false,
				IsRequired = false,
				IsResult = false,
				IsValueSerializable = true,
				ModifiedInSchemaUId = new Guid("4898f7ea-06ca-401a-9cd3-bbb193f916c1"),
				Name = @"CaseRecordId",
				SourceParameterUId = Guid.Empty,
				Tag = @"",
				UseBackgroundMode = false,
				DataValueType = DataValueTypeManager.GetInstanceByName("Guid"),
				SourceValue = new ProcessSchemaParameterValue() {
					Source = ProcessSchemaParameterValueSource.Script,
					Value = @"[#[IsOwnerSchema:false].[IsSchema:false].[Element:{73da230b-c3b0-4648-b931-60aef0bc046c}].[Parameter:{5984f6fb-289e-4980-aea6-b57aa1779872}]#]",
					MetaPath = null,
					ModifiedInSchemaUId = new Guid("4898f7ea-06ca-401a-9cd3-bbb193f916c1"),
				},
			};
		}

		protected override void InitializeParameters() {
			base.InitializeParameters();
			Parameters.Add(CreateCaseRecordIdParameter());
		}

		protected virtual void InitializeStartSignal1Parameters(IParametrizedProcessSchemaElement parametrizedElement) {
			var recordIdParameter = new ProcessSchemaParameter(this) {
				UId = new Guid("5984f6fb-289e-4980-aea6-b57aa1779872"),
				ContainerUId = new Guid("73da230b-c3b0-4648-b931-60aef0bc046c"),
				CreatedInOwnerSchemaUId = Guid.Empty,
				CreatedInPackageId = Guid.Empty,
				CreatedInSchemaUId = new Guid("4898f7ea-06ca-401a-9cd3-bbb193f916c1"),
				Direction = ProcessSchemaParameterDirection.Variable,
				IsCopyValue = false,
				IsLazy = false,
				IsPerformer = false,
				IsRequired = false,
				IsResult = false,
				IsValueSerializable = true,
				ModifiedInSchemaUId = new Guid("4898f7ea-06ca-401a-9cd3-bbb193f916c1"),
				Name = @"RecordId",
				SourceParameterUId = Guid.Empty,
				Tag = @"",
				UseBackgroundMode = false,
				DataValueType = DataValueTypeManager.GetInstanceByName("Guid")
			};
			recordIdParameter.SourceValue = new ProcessSchemaParameterValue(recordIdParameter) {
				Source = ProcessSchemaParameterValueSource.ConstValue,
				Value = null,
				MetaPath = null,
				ModifiedInSchemaUId = Guid.Empty
			};
			parametrizedElement.Parameters.Add(recordIdParameter);
			var entitySchemaUIdParameter = new ProcessSchemaParameter(this) {
				UId = new Guid("e9f654a3-168a-4e03-958d-43d8e77b1431"),
				ContainerUId = new Guid("73da230b-c3b0-4648-b931-60aef0bc046c"),
				CreatedInOwnerSchemaUId = Guid.Empty,
				CreatedInPackageId = Guid.Empty,
				CreatedInSchemaUId = new Guid("73da230b-c3b0-4648-b931-60aef0bc046c"),
				Direction = ProcessSchemaParameterDirection.Variable,
				IsCopyValue = false,
				IsLazy = false,
				IsPerformer = false,
				IsRequired = false,
				IsResult = false,
				IsValueSerializable = true,
				ModifiedInSchemaUId = new Guid("4898f7ea-06ca-401a-9cd3-bbb193f916c1"),
				Name = @"EntitySchemaUId",
				SourceParameterUId = Guid.Empty,
				Tag = @"",
				UseBackgroundMode = false,
				DataValueType = DataValueTypeManager.GetInstanceByName("Guid")
			};
			entitySchemaUIdParameter.SourceValue = new ProcessSchemaParameterValue(entitySchemaUIdParameter) {
				Source = ProcessSchemaParameterValueSource.ConstValue,
				Value = @"117d32f9-8275-4534-8411-1c66115ce9cd",
				MetaPath = null,
				ModifiedInSchemaUId = Guid.Empty
			};
			parametrizedElement.Parameters.Add(entitySchemaUIdParameter);
		}

		protected override void InitializeBaseElements() {
			base.InitializeBaseElements();
			ProcessSchemaLaneSet schemaLaneSet1 = CreateLaneSet1LaneSet();
			LaneSets.Add(schemaLaneSet1);
			ProcessSchemaLane schemaLane1 = CreateLane1Lane();
			schemaLaneSet1.Lanes.Add(schemaLane1);
			ProcessSchemaTerminateEvent terminateevent1 = CreateTerminateEvent1TerminateEvent();
			FlowElements.Add(terminateevent1);
			ProcessSchemaStartSignalEvent startsignal1 = CreateStartSignal1StartSignalEvent();
			FlowElements.Add(startsignal1);
			ProcessSchemaScriptTask runprocess = CreateRunProcessScriptTask();
			FlowElements.Add(runprocess);
			FlowElements.Add(CreateSequenceFlow1SequenceFlow());
			FlowElements.Add(CreateSequenceFlow2SequenceFlow());
		}

		protected virtual ProcessSchemaSequenceFlow CreateSequenceFlow1SequenceFlow() {
			ProcessSchemaSequenceFlow schemaFlow = new ProcessSchemaSequenceFlow(this, ProcessSchemaEditSequenceFlowType.Sequence) {
				Name = "SequenceFlow1",
				UId = new Guid("58c48669-2996-455f-8128-3c393ebe0a43"),
				BackgroundModePriority = BackgroundModePriority.Inherited,
				ContainerItemIndex = 0,
				ContainerUId = Guid.Empty,
				CreatedInOwnerSchemaUId = new Guid("bb4d6607-026b-4b27-b640-8f5c77c1e89d"),
				CreatedInPackageId = new Guid("10cb6199-40c6-4b6d-9d0e-500310d45f57"),
				CreatedInSchemaUId = new Guid("4898f7ea-06ca-401a-9cd3-bbb193f916c1"),
				CurveCenterPosition = new Point(0, 0),
				FlowType = ProcessSchemaEditSequenceFlowType.Sequence,
				ManagerItemUId = new Guid("0d8351f6-c2f4-4737-bdd9-6fbfe0837fec"),
				ModifiedInSchemaUId = new Guid("4898f7ea-06ca-401a-9cd3-bbb193f916c1"),
				SequenceFlowEndPointPosition = new Point(0, 0),
				SequenceFlowStartPointPosition = new Point(0, 0),
				Size = new Size(0, 0),
				SourceRefUId = new Guid("73da230b-c3b0-4648-b931-60aef0bc046c"),
				SourceSequenceFlowPointLocalPosition = new Point(1, 0),
				StrokeColor = Color.FromArgb(-7105128),
				TargetRefUId = new Guid("0b998ab2-94c4-48c4-9555-71041042693b"),
				TargetSequenceFlowPointLocalPosition = new Point(-1, 0),
				UseBackgroundMode = false,
				VisualType = ProcessSchemaSequenceFlowVisualType.AutoPolyline
			};
			return schemaFlow;
		}

		protected virtual ProcessSchemaSequenceFlow CreateSequenceFlow2SequenceFlow() {
			ProcessSchemaSequenceFlow schemaFlow = new ProcessSchemaSequenceFlow(this, ProcessSchemaEditSequenceFlowType.Sequence) {
				Name = "SequenceFlow2",
				UId = new Guid("072884ec-d907-4c9c-9716-1d042fe62b58"),
				BackgroundModePriority = BackgroundModePriority.Inherited,
				ContainerItemIndex = 0,
				ContainerUId = Guid.Empty,
				CreatedInOwnerSchemaUId = new Guid("bb4d6607-026b-4b27-b640-8f5c77c1e89d"),
				CreatedInPackageId = new Guid("10cb6199-40c6-4b6d-9d0e-500310d45f57"),
				CreatedInSchemaUId = new Guid("4898f7ea-06ca-401a-9cd3-bbb193f916c1"),
				CurveCenterPosition = new Point(0, 0),
				FlowType = ProcessSchemaEditSequenceFlowType.Sequence,
				ManagerItemUId = new Guid("0d8351f6-c2f4-4737-bdd9-6fbfe0837fec"),
				ModifiedInSchemaUId = new Guid("4898f7ea-06ca-401a-9cd3-bbb193f916c1"),
				SequenceFlowEndPointPosition = new Point(0, 0),
				SequenceFlowStartPointPosition = new Point(0, 0),
				Size = new Size(0, 0),
				SourceRefUId = new Guid("0b998ab2-94c4-48c4-9555-71041042693b"),
				SourceSequenceFlowPointLocalPosition = new Point(1, 0),
				StrokeColor = Color.FromArgb(-7105128),
				TargetRefUId = new Guid("d85c2283-ddfa-4265-a558-c22d2429128a"),
				TargetSequenceFlowPointLocalPosition = new Point(-1, 0),
				UseBackgroundMode = false,
				VisualType = ProcessSchemaSequenceFlowVisualType.AutoPolyline
			};
			return schemaFlow;
		}

		protected virtual ProcessSchemaLaneSet CreateLaneSet1LaneSet() {
			ProcessSchemaLaneSet schemaLaneSet1 = new ProcessSchemaLaneSet(this) {
				UId = new Guid("5a8d38cc-2217-4e06-aae6-58dbe1e2d7cb"),
				BackgroundModePriority = BackgroundModePriority.Inherited,
				ContainerItemIndex = 0,
				ContainerUId = Guid.Empty,
				CreatedInOwnerSchemaUId = new Guid("bb4d6607-026b-4b27-b640-8f5c77c1e89d"),
				CreatedInPackageId = new Guid("10cb6199-40c6-4b6d-9d0e-500310d45f57"),
				CreatedInSchemaUId = new Guid("4898f7ea-06ca-401a-9cd3-bbb193f916c1"),
				Direction = ProcessSchemaPoolDirectionType.Vertical,
				ManagerItemUId = new Guid("11a47caf-a0d5-41fa-a274-a0b11f77447a"),
				ModifiedInSchemaUId = new Guid("4898f7ea-06ca-401a-9cd3-bbb193f916c1"),
				Name = @"LaneSet1",
				Position = new Point(0, 0),
				Size = new Size(0, 0),
				UseBackgroundMode = false
			};
			return schemaLaneSet1;
		}

		protected virtual ProcessSchemaLane CreateLane1Lane() {
			ProcessSchemaLane schemaLane1 = new ProcessSchemaLane(this) {
				UId = new Guid("cde03cbb-c778-4c8e-a9a0-f1dd4b69874c"),
				BackgroundModePriority = BackgroundModePriority.Inherited,
				ContainerItemIndex = 0,
				ContainerUId = new Guid("5a8d38cc-2217-4e06-aae6-58dbe1e2d7cb"),
				CreatedInOwnerSchemaUId = new Guid("4898f7ea-06ca-401a-9cd3-bbb193f916c1"),
				CreatedInPackageId = new Guid("10cb6199-40c6-4b6d-9d0e-500310d45f57"),
				CreatedInSchemaUId = new Guid("4898f7ea-06ca-401a-9cd3-bbb193f916c1"),
				ManagerItemUId = new Guid("abcd74b9-5912-414b-82ac-f1aa4dcd554e"),
				ModifiedInSchemaUId = new Guid("4898f7ea-06ca-401a-9cd3-bbb193f916c1"),
				Name = @"Lane1",
				Position = new Point(0, 0),
				Size = new Size(0, 0),
				UseBackgroundMode = false
			};
			return schemaLane1;
		}

		protected virtual ProcessSchemaTerminateEvent CreateTerminateEvent1TerminateEvent() {
			ProcessSchemaTerminateEvent schemaTerminateEvent = new ProcessSchemaTerminateEvent(this) {
				UId = new Guid("d85c2283-ddfa-4265-a558-c22d2429128a"),
				BackgroundModePriority = BackgroundModePriority.Inherited,
				ContainerItemIndex = 0,
				ContainerUId = new Guid("cde03cbb-c778-4c8e-a9a0-f1dd4b69874c"),
				CreatedInOwnerSchemaUId = new Guid("bb4d6607-026b-4b27-b640-8f5c77c1e89d"),
				CreatedInPackageId = new Guid("10cb6199-40c6-4b6d-9d0e-500310d45f57"),
				CreatedInSchemaUId = new Guid("4898f7ea-06ca-401a-9cd3-bbb193f916c1"),
				EntitySchemaUId = Guid.Empty,
				IsLogging = true,
				ManagerItemUId = new Guid("1bd93619-0574-454e-bb4e-cf53b9eb9470"),
				ModifiedInSchemaUId = new Guid("4898f7ea-06ca-401a-9cd3-bbb193f916c1"),
				Name = @"TerminateEvent1",
				Position = new Point(600, 184),
				SerializeToDB = false,
				Size = new Size(27, 27),
				ThrowToBase = false,
				UseBackgroundMode = false
			};
			
			return schemaTerminateEvent;
		}

		protected virtual ProcessSchemaStartSignalEvent CreateStartSignal1StartSignalEvent() {
			ProcessSchemaStartSignalEvent schemaStartSignalEvent = new ProcessSchemaStartSignalEvent(this) {	UId = new Guid("73da230b-c3b0-4648-b931-60aef0bc046c"),
				BackgroundModePriority = BackgroundModePriority.Inherited,
				ContainerItemIndex = 0,
				ContainerUId = new Guid("cde03cbb-c778-4c8e-a9a0-f1dd4b69874c"),
				CreatedInOwnerSchemaUId = new Guid("bb4d6607-026b-4b27-b640-8f5c77c1e89d"),
				CreatedInPackageId = new Guid("10cb6199-40c6-4b6d-9d0e-500310d45f57"),
				CreatedInSchemaUId = new Guid("4898f7ea-06ca-401a-9cd3-bbb193f916c1"),
				EntitySchemaUId = new Guid("117d32f9-8275-4534-8411-1c66115ce9cd"),
				EntitySignal = EntityChangeType.Updated,
				HasEntityColumnChange = true,
				HasEntityFilters = true,
				ImageList = null,
				ImageName = null,
				IsInterrupting = false,
				IsLogging = true,
				ManagerItemUId = new Guid("1129e72f-0e8c-445a-b2ea-463517e86395"),
				ModifiedInSchemaUId = new Guid("4898f7ea-06ca-401a-9cd3-bbb193f916c1"),
				Name = @"StartSignal1",
				NewEntityChangedColumns = false,
				Position = new Point(53, 184),
				SerializeToDB = false,
				Size = new Size(27, 27),
				UseBackgroundMode = true,
				WaitingEntitySignal = true,
				WaitingRandomSignal = false
			};
			schemaStartSignalEvent.EntityChangedColumns.Add("ccfc5fbf-4dc9-47e4-91f3-54ea9251ab18");
			InitializeStartSignal1Parameters(schemaStartSignalEvent);
			return schemaStartSignalEvent;
		}

		protected virtual ProcessSchemaScriptTask CreateRunProcessScriptTask() {
			ProcessSchemaScriptTask ScriptTask = new ProcessSchemaScriptTask(this) {
				UId = new Guid("0b998ab2-94c4-48c4-9555-71041042693b"),
				BackgroundModePriority = BackgroundModePriority.Inherited,
				ContainerItemIndex = 0,
				ContainerUId = new Guid("cde03cbb-c778-4c8e-a9a0-f1dd4b69874c"),
				CreatedInOwnerSchemaUId = new Guid("bb4d6607-026b-4b27-b640-8f5c77c1e89d"),
				CreatedInPackageId = new Guid("10cb6199-40c6-4b6d-9d0e-500310d45f57"),
				CreatedInSchemaUId = new Guid("4898f7ea-06ca-401a-9cd3-bbb193f916c1"),
				EntitySchemaUId = Guid.Empty,
				FillColor = Color.FromArgb(-1),
				ImageList = null,
				ImageName = null,
				IsForCompensation = false,
				IsLogging = true,
				ManagerItemUId = new Guid("0e490dda-e140-4441-b600-6f5c64d024df"),
				ModifiedInSchemaUId = new Guid("4898f7ea-06ca-401a-9cd3-bbb193f916c1"),
				Name = @"RunProcess",
				Position = new Point(322, 170),
				SerializeToDB = false,
				Size = new Size(69, 55),
				UseBackgroundMode = false,
				UseFlowEngineScriptVersion = false,
				ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,165,84,219,142,218,48,16,125,134,175,200,230,97,21,36,148,31,232,69,218,13,97,21,169,93,181,64,183,207,110,50,1,75,142,3,190,164,69,93,254,189,51,118,2,33,187,148,94,30,237,153,57,115,230,204,177,27,166,2,208,187,224,93,32,225,123,144,74,195,205,126,153,111,160,98,159,45,168,125,244,69,131,74,106,41,33,55,188,150,113,63,225,35,147,108,13,106,26,132,9,211,16,78,222,140,27,4,211,134,25,171,147,90,216,74,62,178,10,16,153,26,196,119,69,225,47,163,112,201,12,215,37,115,136,31,160,1,17,47,93,81,156,21,225,36,166,162,62,84,166,231,92,50,225,139,17,237,143,193,124,221,57,175,76,47,64,215,162,129,226,34,94,87,221,37,14,6,195,81,175,49,122,209,159,18,80,200,187,162,226,114,193,215,27,163,177,170,100,66,183,131,230,8,234,149,109,209,30,192,248,243,64,255,105,64,253,23,144,215,170,200,10,68,230,101,16,245,170,111,112,141,86,136,73,240,115,60,34,96,238,248,58,30,136,124,74,164,6,171,253,182,83,225,137,9,11,111,191,213,181,120,31,93,24,211,173,5,27,182,176,157,56,127,135,58,84,255,8,74,99,244,185,62,63,7,55,167,38,110,156,145,2,99,149,12,140,178,168,218,232,224,153,160,105,169,202,75,126,157,204,131,229,197,113,196,163,69,187,177,160,103,110,196,186,110,125,234,145,73,132,147,57,220,239,9,42,58,190,133,81,187,18,5,218,10,67,183,180,219,30,70,156,40,96,6,94,221,51,213,159,10,47,122,199,233,214,203,155,131,201,55,115,85,87,179,251,232,204,40,193,237,237,64,42,116,10,137,17,167,213,214,236,59,129,143,64,75,48,61,217,58,75,227,243,156,158,195,16,207,214,17,255,96,178,215,13,214,154,193,133,60,177,223,49,75,68,173,173,130,25,42,137,228,6,43,75,172,82,40,57,221,18,153,246,72,185,43,142,187,154,184,118,142,127,1,2,12,180,191,224,204,29,134,43,161,212,152,196,141,66,76,96,123,40,30,107,195,75,158,51,10,135,62,254,117,3,170,53,129,251,204,50,157,238,44,19,81,59,227,39,166,112,74,3,234,124,61,190,246,78,22,81,152,86,140,139,21,84,91,129,44,61,132,140,40,60,122,1,177,2,165,152,174,75,19,35,203,146,175,173,98,126,106,90,15,168,134,231,128,1,109,116,236,30,146,165,96,159,242,106,43,176,247,52,248,95,244,84,27,94,185,224,2,118,22,180,241,192,4,235,5,246,226,198,233,15,200,45,234,234,46,15,3,195,177,6,34,231,234,137,123,220,135,113,255,185,255,2,75,59,24,44,165,6,0,0 }
			};
			
			return ScriptTask;
		}

		protected override void InitializeMethods() {
			base.InitializeMethods();
			SetMethodsDefInheritance();
		}

		#endregion

		#region Methods: Public

		public override Process CreateProcess(UserConnection userConnection) {
			return new RunAnalyzeCaseSatisfactionLevel(userConnection);
		}

		public override object Clone() {
			return new RunAnalyzeCaseSatisfactionLevelSchema(this);
		}

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("4898f7ea-06ca-401a-9cd3-bbb193f916c1"));
		}

		#endregion

	}

	#endregion

	#region Class: RunAnalyzeCaseSatisfactionLevel

	/// <exclude/>
	public class RunAnalyzeCaseSatisfactionLevel : Terrasoft.Core.Process.Process
	{

		#region Class: ProcessLane1

		/// <exclude/>
		public class ProcessLane1 : ProcessLane
		{

			public ProcessLane1(UserConnection userConnection, RunAnalyzeCaseSatisfactionLevel process)
				: base(userConnection) {
				Owner = process;
				IsUsedParentUserContexts = false;
			}

		}

		#endregion

		public RunAnalyzeCaseSatisfactionLevel(UserConnection userConnection)
			: base(userConnection) {
			InitializeMetaPathParameterValues();
			UId = Guid.NewGuid();
			Name = "RunAnalyzeCaseSatisfactionLevel";
			SchemaUId = new Guid("4898f7ea-06ca-401a-9cd3-bbb193f916c1");
			Caption = Schema.Caption;
			SchemaManagerName = "ProcessSchemaManager";
			SerializeToDB = true;
			SerializeToMemory = true;
			IsLogging = true;
			UseSystemSecurityContext = false;
			_notificationCaption = () => { return new LocalizableString((Caption)); };
			_caseRecordId = () => { return (Guid)((StartSignal1.RecordId)); };
			InitializeFlowElements();
		}

		#region Properties: Private

		private Guid InternalSchemaUId {
			get {
				return new Guid("4898f7ea-06ca-401a-9cd3-bbb193f916c1");
			}
		}

		private string ConditionalExpressionLogMessage {
			get {
				return "Process: RunAnalyzeCaseSatisfactionLevel, Source element: {0}, Conditional flow: {1}, Expression: {2}, Result: {3}";
			}
		}

		private string DeadEndGatewayLogMessage {
			get {
				return "Process: RunAnalyzeCaseSatisfactionLevel, Source element: {0}, None of the exclusive gateway conditions are met!";
			}
		}

		#endregion

		#region Properties: Public

		private Func<string> _notificationCaption;
		public virtual string NotificationCaption {
			get {
				return (_notificationCaption ?? (_notificationCaption = () => null)).Invoke();
			}
			set {
				_notificationCaption = () => { return value; };
			}
		}

		private Func<Guid> _caseRecordId;
		public virtual Guid CaseRecordId {
			get {
				return (_caseRecordId ?? (_caseRecordId = () => Guid.Empty)).Invoke();
			}
			set {
				_caseRecordId = () => { return value; };
			}
		}

		private ProcessLane1 _lane1;
		public ProcessLane1 Lane1 {
			get {
				return _lane1 ?? ((_lane1) = new ProcessLane1(UserConnection, this));
			}
		}

		private ProcessTerminateEvent _terminateEvent1;
		public ProcessTerminateEvent TerminateEvent1 {
			get {
				return _terminateEvent1 ?? (_terminateEvent1 = new ProcessTerminateEvent() {
					UId = Guid.NewGuid(),
					Owner = this,
					Type = "ProcessSchemaTerminateEvent",
					Name = "TerminateEvent1",
					SchemaElementUId = new Guid("d85c2283-ddfa-4265-a558-c22d2429128a"),
					CreatedInSchemaUId = InternalSchemaUId,
					ExecutedEventHandler = OnExecuted,
					IsLogging = true,
				});
			}
		}

		private ProcessStartSignalEvent _startSignal1;
		public ProcessStartSignalEvent StartSignal1 {
			get {
				return _startSignal1 ?? (_startSignal1 = new ProcessStartSignalEvent(UserConnection) {
					UId = Guid.NewGuid(),
					Owner = this,
					Type = "ProcessSchemaStartSignalEvent",
					Name = "StartSignal1",
					SerializeToDB = false,
					IsLogging = true,
					SchemaElementUId = new Guid("73da230b-c3b0-4648-b931-60aef0bc046c"),
					CreatedInSchemaUId = InternalSchemaUId,
					ExecutedEventHandler = OnExecuted,
				});
			}
		}

		private ProcessScriptTask _runProcess;
		public ProcessScriptTask RunProcess {
			get {
				return _runProcess ?? (_runProcess = new ProcessScriptTask() {
					UId = Guid.NewGuid(),
					Owner = this,
					Type = "ProcessSchemaScriptTask",
					Name = "RunProcess",
					SchemaElementUId = new Guid("0b998ab2-94c4-48c4-9555-71041042693b"),
					CreatedInSchemaUId = InternalSchemaUId,
					ExecutedEventHandler = OnExecuted,
					IsLogging = true,
					Script = RunProcessExecute,
				});
			}
		}

		#endregion

		#region Methods: Private

		private void InitializeFlowElements() {
			FlowElements[TerminateEvent1.SchemaElementUId] = new Collection<ProcessFlowElement> { TerminateEvent1 };
			FlowElements[StartSignal1.SchemaElementUId] = new Collection<ProcessFlowElement> { StartSignal1 };
			FlowElements[RunProcess.SchemaElementUId] = new Collection<ProcessFlowElement> { RunProcess };
		}

		private void OnExecuted(object sender, ProcessActivityAfterEventArgs e) {
			switch (e.Context.SenderName) {
					case "TerminateEvent1":
						CompleteProcess();
						break;
					case "StartSignal1":
						e.Context.QueueTasksV2.Enqueue(new ProcessQueueElement("RunProcess", e.Context.SenderName));
						break;
					case "RunProcess":
						e.Context.QueueTasksV2.Enqueue(new ProcessQueueElement("TerminateEvent1", e.Context.SenderName));
						break;
			}
		}

		private void WritePropertyValues(DataWriter writer, bool useAllValueSources) {
			if (!HasMapping("CaseRecordId")) {
				writer.WriteValue("CaseRecordId", CaseRecordId, Guid.Empty);
			}
		}

		#endregion

		#region Methods: Protected

		protected override void PrepareStart(ProcessExecutingContext context) {
			base.PrepareStart(context);
			context.Process = this;
			if (IsProcessExecutedBySignal) {
				return;
			}
		}

		protected override void CompleteApplyingFlowElementsPropertiesData() {
			base.CompleteApplyingFlowElementsPropertiesData();
			foreach (var item in FlowElements) {
				foreach (var itemValue in item.Value) {
					if (Guid.Equals(itemValue.CreatedInSchemaUId, InternalSchemaUId)) {
						itemValue.ExecutedEventHandler = OnExecuted;
					}
				}
			}
		}

		protected override void InitializeMetaPathParameterValues() {
			base.InitializeMetaPathParameterValues();
			MetaPathParameterValues.Add("c38a0af2-cfe7-436f-8d64-87341f47d144", () => CaseRecordId);
			MetaPathParameterValues.Add("5984f6fb-289e-4980-aea6-b57aa1779872", () => StartSignal1.RecordId);
			MetaPathParameterValues.Add("e9f654a3-168a-4e03-958d-43d8e77b1431", () => StartSignal1.EntitySchemaUId);
		}

		protected override void ApplyPropertiesDataValues(DataReader reader) {
			base.ApplyPropertiesDataValues(reader);
			bool hasValueToRead = reader.HasValue();
			switch (reader.CurrentName) {
				case "CaseRecordId":
					if (!hasValueToRead) break;
					CaseRecordId = reader.GetValue<System.Guid>();
				break;
			}
		}

		protected override void WritePropertyValues(DataWriter writer) {
			base.WritePropertyValues(writer);
			WritePropertyValues(writer, true);
		}

		#endregion

		#region Methods: Public

		public virtual bool RunProcessExecute(ProcessExecutingContext context) {
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "Case");
			var statusColumnName =  esq.AddColumn("SatisfactionLevel.Status.Id").Name;
			var statusIsFinalColumn = esq.AddColumn("SatisfactionLevel.Status.IsFinal");
			var statusIsResolvedColumn = esq.AddColumn("Status.IsResolved");
			var statusCaseIsFinalColumn = esq.AddColumn("Status.IsFinal");
			esq.UseAdminRights = false;
			var caseEntity = esq.GetEntity(UserConnection, CaseRecordId);
			if (caseEntity != null) {
				var isCaseFinal = caseEntity.GetTypedColumnValue<bool>(statusCaseIsFinalColumn.Name);
				var isResolved = caseEntity.GetTypedColumnValue<bool>(statusIsResolvedColumn.Name);
				if (isCaseFinal || !isResolved) {
					return true;
				}
				var newCaseStatus = caseEntity.GetTypedColumnValue<Guid>(statusColumnName);
				var entitySchema = UserConnection.EntitySchemaManager.GetInstanceByName("Case");
				Entity resultCase = entitySchema.CreateEntity(UserConnection);
				resultCase.UseAdminRights = false;
				if (resultCase.FetchFromDB(CaseRecordId) && newCaseStatus != Guid.Empty) {
					resultCase.SetColumnValue("StatusId", newCaseStatus);
					var isFinal = caseEntity.GetTypedColumnValue<bool>(statusIsFinalColumn.Name);
					if (isFinal) {
						resultCase.SetColumnValue("ClosureDate", UserConnection.CurrentUser.GetCurrentDateTime());
						var delete = new Delete(UserConnection)
						.From("DelayedNotification")
						.Where("CaseId").IsEqual(Column.Parameter(CaseRecordId))
						.And("EmailTemplateId").In(
							Column.Parameter(Terrasoft.Configuration.CaseServiceConsts.ResolutionNotificationTplId), 
							Column.Parameter(Terrasoft.Configuration.CaseServiceConsts.EstimationRequestTplId)
						);
						delete.Execute();
					}
					resultCase.Save(false);
				}
			}
			return true;
		}

		public override void ThrowEvent(ProcessExecutingContext context, string message) {
			base.ThrowEvent(context, message);
		}

		public override void WritePropertiesData(DataWriter writer, bool writeFlowElements = true) {
			if (Status == Core.Process.ProcessStatus.Inactive) {
				return;
			}
			writer.WriteStartObject(Name);
			base.WritePropertiesData(writer, writeFlowElements);
			WritePropertyValues(writer, false);
			writer.WriteFinishObject();
		}

		public override object CloneShallow() {
			var cloneItem = (RunAnalyzeCaseSatisfactionLevel)base.CloneShallow();
			cloneItem.ExecutedEventHandler = ExecutedEventHandler;
			return cloneItem;
		}

		#endregion

	}

	#endregion

}


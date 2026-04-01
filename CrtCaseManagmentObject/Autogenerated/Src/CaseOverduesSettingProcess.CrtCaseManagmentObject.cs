namespace Terrasoft.Core.Process
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Data;
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

	#region Class: CaseOverduesSettingProcessSchema

	/// <exclude/>
	public class CaseOverduesSettingProcessSchema : Terrasoft.Core.Process.ProcessSchema
	{

		#region Constructors: Public

		public CaseOverduesSettingProcessSchema(ProcessSchemaManager processSchemaManager)
			: base(processSchemaManager) {
		}

		public CaseOverduesSettingProcessSchema(CaseOverduesSettingProcessSchema source)
			: base(source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			Name = "CaseOverduesSettingProcess";
			UId = new Guid("b43b77dd-0397-4708-9b90-280ac06c4f02");
			CreatedInPackageId = new Guid("a2fc2e06-4c85-4afa-ac7e-48c4edb4a538");
			CreatedInSchemaUId = Guid.Empty;
			CreatedInVersion = @"0.0.0.0";
			CultureName = @"en-US";
			EntitySchemaUId = Guid.Empty;
			IsCreatedInSvg = true;
			IsLogging = false;
			ModifiedInSchemaUId = Guid.Empty;
			ParametersEditPageSchemaUId = Guid.Empty;
			ParentSchemaUId = new Guid("bb4d6607-026b-4b27-b640-8f5c77c1e89d");
			SequenceFlowStrokeDefColor = Color.FromArgb(-4473925);
			SerializeToDB = false;
			SerializeToMemory = true;
			Tag = @"Business process";
			TaskFillDefColor = Color.FromArgb(-1);
			UsageType = ProcessSchemaUsageType.Advanced;
			UseForceCompile = true;
			ZipMethodsBody = new byte[] {  };
			ZipCompiledMethodsBody = new byte[] {  };
			RealUId = new Guid("b43b77dd-0397-4708-9b90-280ac06c4f02");
			Version = 0;
			PackageUId = new Guid("83b1062a-d944-eba7-a72b-5400034c6223");
			UseSystemSecurityContext = false;
		}

		protected override void InitializeParameters() {
			base.InitializeParameters();
		}

		protected override void InitializeBaseElements() {
			base.InitializeBaseElements();
			ProcessSchemaLaneSet schemaLaneSet1 = CreateLaneSet1LaneSet();
			LaneSets.Add(schemaLaneSet1);
			ProcessSchemaLane schemaLane1 = CreateLane1Lane();
			schemaLaneSet1.Lanes.Add(schemaLane1);
			ProcessSchemaStartEvent start1 = CreateStart1StartEvent();
			FlowElements.Add(start1);
			ProcessSchemaTerminateEvent terminate1 = CreateTerminate1TerminateEvent();
			FlowElements.Add(terminate1);
			ProcessSchemaScriptTask setservicerequestoverdues = CreateSetServiceRequestOverduesScriptTask();
			FlowElements.Add(setservicerequestoverdues);
			FlowElements.Add(CreateSequenceFlow1SequenceFlow());
			FlowElements.Add(CreateSequenceFlow2SequenceFlow());
		}

		protected virtual ProcessSchemaSequenceFlow CreateSequenceFlow1SequenceFlow() {
			ProcessSchemaSequenceFlow schemaFlow = new ProcessSchemaSequenceFlow(this, ProcessSchemaEditSequenceFlowType.Sequence) {
				Name = "SequenceFlow1",
				UId = new Guid("51bf9617-b20a-4c28-bf50-51fa7c271412"),
				BackgroundModePriority = BackgroundModePriority.Inherited,
				ContainerItemIndex = 0,
				ContainerUId = Guid.Empty,
				CreatedInOwnerSchemaUId = new Guid("bb4d6607-026b-4b27-b640-8f5c77c1e89d"),
				CreatedInPackageId = new Guid("a2fc2e06-4c85-4afa-ac7e-48c4edb4a538"),
				CreatedInSchemaUId = new Guid("b43b77dd-0397-4708-9b90-280ac06c4f02"),
				CurveCenterPosition = new Point(373, 203),
				DragGroupName = @"SequenceFlow",
				FlowType = ProcessSchemaEditSequenceFlowType.Sequence,
				ManagerItemUId = new Guid("0d8351f6-c2f4-4737-bdd9-6fbfe0837fec"),
				ModifiedInSchemaUId = new Guid("b43b77dd-0397-4708-9b90-280ac06c4f02"),
				SequenceFlowEndPointPosition = new Point(0, 0),
				SequenceFlowStartPointPosition = new Point(0, 0),
				Size = new Size(0, 0),
				SourceRefUId = new Guid("f95b0cc9-1f6b-4cbe-804b-72a3f9068aaf"),
				SourceSequenceFlowPointLocalPosition = new Point(0, 0),
				StrokeColor = Color.FromArgb(-7105128),
				TargetRefUId = new Guid("61b6c3f5-777d-4ec0-b83d-8a45acb801e0"),
				TargetSequenceFlowPointLocalPosition = new Point(0, 0),
				UseBackgroundMode = false,
				VisualType = ProcessSchemaSequenceFlowVisualType.AutoPolyline
			};
			return schemaFlow;
		}

		protected virtual ProcessSchemaSequenceFlow CreateSequenceFlow2SequenceFlow() {
			ProcessSchemaSequenceFlow schemaFlow = new ProcessSchemaSequenceFlow(this, ProcessSchemaEditSequenceFlowType.Sequence) {
				Name = "SequenceFlow2",
				UId = new Guid("7a719379-d662-4cab-a06d-8d2b5c0a62b5"),
				BackgroundModePriority = BackgroundModePriority.Inherited,
				ContainerItemIndex = 0,
				ContainerUId = Guid.Empty,
				CreatedInOwnerSchemaUId = new Guid("bb4d6607-026b-4b27-b640-8f5c77c1e89d"),
				CreatedInPackageId = new Guid("a2fc2e06-4c85-4afa-ac7e-48c4edb4a538"),
				CreatedInSchemaUId = new Guid("b43b77dd-0397-4708-9b90-280ac06c4f02"),
				CurveCenterPosition = new Point(373, 203),
				DragGroupName = @"SequenceFlow",
				FlowType = ProcessSchemaEditSequenceFlowType.Sequence,
				ManagerItemUId = new Guid("0d8351f6-c2f4-4737-bdd9-6fbfe0837fec"),
				ModifiedInSchemaUId = new Guid("b43b77dd-0397-4708-9b90-280ac06c4f02"),
				SequenceFlowEndPointPosition = new Point(0, 0),
				SequenceFlowStartPointPosition = new Point(0, 0),
				Size = new Size(0, 0),
				SourceRefUId = new Guid("61b6c3f5-777d-4ec0-b83d-8a45acb801e0"),
				SourceSequenceFlowPointLocalPosition = new Point(0, 0),
				StrokeColor = Color.FromArgb(-7105128),
				TargetRefUId = new Guid("cd2459a2-adeb-4627-90c3-87821a6fdef9"),
				TargetSequenceFlowPointLocalPosition = new Point(0, 0),
				UseBackgroundMode = false,
				VisualType = ProcessSchemaSequenceFlowVisualType.AutoPolyline
			};
			return schemaFlow;
		}

		protected virtual ProcessSchemaLaneSet CreateLaneSet1LaneSet() {
			ProcessSchemaLaneSet schemaLaneSet1 = new ProcessSchemaLaneSet(this) {
				UId = new Guid("21415b97-3b49-42b5-acd3-82548e536530"),
				BackgroundModePriority = BackgroundModePriority.Inherited,
				ContainerItemIndex = 1,
				ContainerUId = Guid.Empty,
				CreatedInOwnerSchemaUId = new Guid("bb4d6607-026b-4b27-b640-8f5c77c1e89d"),
				CreatedInPackageId = new Guid("a2fc2e06-4c85-4afa-ac7e-48c4edb4a538"),
				CreatedInSchemaUId = new Guid("b43b77dd-0397-4708-9b90-280ac06c4f02"),
				Direction = ProcessSchemaPoolDirectionType.Vertical,
				DragGroupName = @"LaneSet",
				ManagerItemUId = new Guid("11a47caf-a0d5-41fa-a274-a0b11f77447a"),
				ModifiedInSchemaUId = new Guid("b43b77dd-0397-4708-9b90-280ac06c4f02"),
				Name = @"LaneSet1",
				Position = new Point(5, 5),
				Size = new Size(0, 0),
				UseBackgroundMode = false
			};
			return schemaLaneSet1;
		}

		protected virtual ProcessSchemaLane CreateLane1Lane() {
			ProcessSchemaLane schemaLane1 = new ProcessSchemaLane(this) {
				UId = new Guid("b092122d-adfc-46b7-8e4a-cbf1b7e9267e"),
				BackgroundModePriority = BackgroundModePriority.Inherited,
				ContainerItemIndex = 1,
				ContainerUId = new Guid("21415b97-3b49-42b5-acd3-82548e536530"),
				CreatedInOwnerSchemaUId = Guid.Empty,
				CreatedInPackageId = new Guid("a2fc2e06-4c85-4afa-ac7e-48c4edb4a538"),
				CreatedInSchemaUId = new Guid("b43b77dd-0397-4708-9b90-280ac06c4f02"),
				DragGroupName = @"Lane",
				ManagerItemUId = new Guid("abcd74b9-5912-414b-82ac-f1aa4dcd554e"),
				ModifiedInSchemaUId = new Guid("b43b77dd-0397-4708-9b90-280ac06c4f02"),
				Name = @"Lane1",
				Position = new Point(29, 0),
				Size = new Size(0, 0),
				UseBackgroundMode = false
			};
			return schemaLane1;
		}

		protected virtual ProcessSchemaStartEvent CreateStart1StartEvent() {
			ProcessSchemaStartEvent schemaStartEvent = new ProcessSchemaStartEvent(this) {
				UId = new Guid("f95b0cc9-1f6b-4cbe-804b-72a3f9068aaf"),
				BackgroundModePriority = BackgroundModePriority.Inherited,
				ContainerItemIndex = 0,
				ContainerUId = new Guid("b092122d-adfc-46b7-8e4a-cbf1b7e9267e"),
				CreatedInOwnerSchemaUId = new Guid("bb4d6607-026b-4b27-b640-8f5c77c1e89d"),
				CreatedInPackageId = new Guid("a2fc2e06-4c85-4afa-ac7e-48c4edb4a538"),
				CreatedInSchemaUId = new Guid("b43b77dd-0397-4708-9b90-280ac06c4f02"),
				DragGroupName = @"Event",
				EntitySchemaUId = Guid.Empty,
				IsInterrupting = false,
				IsLogging = true,
				ManagerItemUId = new Guid("53818048-7868-48f6-ada0-0ebaa65af628"),
				ModifiedInSchemaUId = new Guid("b43b77dd-0397-4708-9b90-280ac06c4f02"),
				Name = @"Start1",
				Position = new Point(50, 184),
				SerializeToDB = false,
				Size = new Size(27, 27),
				UseBackgroundMode = false
			};
			
			return schemaStartEvent;
		}

		protected virtual ProcessSchemaTerminateEvent CreateTerminate1TerminateEvent() {
			ProcessSchemaTerminateEvent schemaTerminateEvent = new ProcessSchemaTerminateEvent(this) {
				UId = new Guid("cd2459a2-adeb-4627-90c3-87821a6fdef9"),
				BackgroundModePriority = BackgroundModePriority.Inherited,
				ContainerItemIndex = 0,
				ContainerUId = new Guid("b092122d-adfc-46b7-8e4a-cbf1b7e9267e"),
				CreatedInOwnerSchemaUId = new Guid("bb4d6607-026b-4b27-b640-8f5c77c1e89d"),
				CreatedInPackageId = new Guid("a2fc2e06-4c85-4afa-ac7e-48c4edb4a538"),
				CreatedInSchemaUId = new Guid("b43b77dd-0397-4708-9b90-280ac06c4f02"),
				DragGroupName = @"Event",
				EntitySchemaUId = Guid.Empty,
				IsLogging = true,
				ManagerItemUId = new Guid("1bd93619-0574-454e-bb4e-cf53b9eb9470"),
				ModifiedInSchemaUId = new Guid("b43b77dd-0397-4708-9b90-280ac06c4f02"),
				Name = @"Terminate1",
				Position = new Point(600, 184),
				SerializeToDB = false,
				Size = new Size(27, 27),
				ThrowToBase = false,
				UseBackgroundMode = false
			};
			
			return schemaTerminateEvent;
		}

		protected virtual ProcessSchemaScriptTask CreateSetServiceRequestOverduesScriptTask() {
			ProcessSchemaScriptTask ScriptTask = new ProcessSchemaScriptTask(this) {
				UId = new Guid("61b6c3f5-777d-4ec0-b83d-8a45acb801e0"),
				BackgroundModePriority = BackgroundModePriority.Inherited,
				ContainerItemIndex = 0,
				ContainerUId = new Guid("b092122d-adfc-46b7-8e4a-cbf1b7e9267e"),
				CreatedInOwnerSchemaUId = new Guid("bb4d6607-026b-4b27-b640-8f5c77c1e89d"),
				CreatedInPackageId = new Guid("a2fc2e06-4c85-4afa-ac7e-48c4edb4a538"),
				CreatedInSchemaUId = new Guid("b43b77dd-0397-4708-9b90-280ac06c4f02"),
				DragGroupName = @"Task",
				EntitySchemaUId = Guid.Empty,
				FillColor = Color.FromArgb(-1),
				ImageList = null,
				ImageName = null,
				IsForCompensation = false,
				IsLogging = true,
				ManagerItemUId = new Guid("0e490dda-e140-4441-b600-6f5c64d024df"),
				ModifiedInSchemaUId = new Guid("b43b77dd-0397-4708-9b90-280ac06c4f02"),
				Name = @"SetServiceRequestOverdues",
				Position = new Point(309, 170),
				SerializeToDB = false,
				Size = new Size(69, 55),
				UseBackgroundMode = false,
				UseFlowEngineScriptVersion = false,
				ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,189,82,203,106,195,48,16,60,55,144,127,16,58,201,16,12,61,135,30,90,55,208,64,176,67,220,144,179,176,22,98,42,75,238,74,178,251,249,149,95,37,110,156,54,208,199,205,187,158,157,153,213,78,197,145,184,82,112,11,228,142,40,168,201,190,45,216,222,0,70,90,41,200,108,174,213,130,208,136,27,160,65,120,200,237,241,41,87,214,176,6,188,211,245,70,103,47,77,131,5,193,124,118,19,166,96,25,221,129,41,181,50,144,84,128,194,1,93,144,72,75,87,168,208,51,26,255,255,150,118,224,195,17,16,24,77,45,183,206,172,133,167,143,181,39,10,215,38,118,82,178,22,115,175,68,247,145,148,160,30,164,87,107,203,211,186,215,19,32,18,69,71,211,221,248,135,159,71,191,89,11,216,128,49,9,174,94,29,151,172,183,182,229,200,11,176,128,172,65,61,231,5,132,123,155,197,186,14,58,162,72,106,3,35,125,252,206,200,217,50,215,216,25,115,76,73,159,213,35,210,225,205,91,97,109,71,59,158,60,63,225,166,63,245,114,62,235,2,16,174,222,32,115,254,246,129,111,13,205,95,76,69,234,77,52,99,255,149,138,65,111,139,186,202,47,135,99,128,253,121,56,38,253,92,202,200,151,174,38,169,174,143,202,231,67,252,48,42,8,214,161,34,22,29,44,223,1,31,0,208,147,79,4,0,0 }
			};
			
			return ScriptTask;
		}

		protected override void InitializeMethods() {
			base.InitializeMethods();
			SetMethodsDefInheritance();
		}

		protected override void InitializeUsings() {
			Usings.Add(new SchemaUsing() {
				UId = new Guid("50c42b5a-2b9b-4207-94b7-67de24bace66"),
				Name = "System.Data",
				Alias = "",
				CreatedInPackageId = new Guid("a2fc2e06-4c85-4afa-ac7e-48c4edb4a538")
			});
		}

		#endregion

		#region Methods: Public

		public override Process CreateProcess(UserConnection userConnection) {
			return new CaseOverduesSettingProcess(userConnection);
		}

		public override object Clone() {
			return new CaseOverduesSettingProcessSchema(this);
		}

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("b43b77dd-0397-4708-9b90-280ac06c4f02"));
		}

		#endregion

	}

	#endregion

	#region Class: CaseOverduesSettingProcess

	/// <exclude/>
	public class CaseOverduesSettingProcess : Terrasoft.Core.Process.Process
	{

		#region Class: ProcessLane1

		/// <exclude/>
		public class ProcessLane1 : ProcessLane
		{

			public ProcessLane1(UserConnection userConnection, CaseOverduesSettingProcess process)
				: base(userConnection) {
				Owner = process;
				IsUsedParentUserContexts = false;
			}

		}

		#endregion

		public CaseOverduesSettingProcess(UserConnection userConnection)
			: base(userConnection) {
			InitializeMetaPathParameterValues();
			UId = Guid.NewGuid();
			Name = "CaseOverduesSettingProcess";
			SchemaUId = new Guid("b43b77dd-0397-4708-9b90-280ac06c4f02");
			Caption = Schema.Caption;
			SchemaManagerName = "ProcessSchemaManager";
			SerializeToDB = false;
			SerializeToMemory = true;
			IsLogging = false;
			UseSystemSecurityContext = false;
			_notificationCaption = () => { return new LocalizableString((Caption)); };
			InitializeFlowElements();
		}

		#region Properties: Private

		private Guid InternalSchemaUId {
			get {
				return new Guid("b43b77dd-0397-4708-9b90-280ac06c4f02");
			}
		}

		private string ConditionalExpressionLogMessage {
			get {
				return "Process: CaseOverduesSettingProcess, Source element: {0}, Conditional flow: {1}, Expression: {2}, Result: {3}";
			}
		}

		private string DeadEndGatewayLogMessage {
			get {
				return "Process: CaseOverduesSettingProcess, Source element: {0}, None of the exclusive gateway conditions are met!";
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

		private ProcessLane1 _lane1;
		public ProcessLane1 Lane1 {
			get {
				return _lane1 ?? ((_lane1) = new ProcessLane1(UserConnection, this));
			}
		}

		private ProcessFlowElement _start1;
		public ProcessFlowElement Start1 {
			get {
				return _start1 ?? (_start1 = new ProcessFlowElement() {
					UId = Guid.NewGuid(),
					Owner = this,
					Type = "ProcessSchemaStartEvent",
					Name = "Start1",
					SchemaElementUId = new Guid("f95b0cc9-1f6b-4cbe-804b-72a3f9068aaf"),
					CreatedInSchemaUId = InternalSchemaUId,
					ExecutedEventHandler = OnExecuted,
					IsLogging = true,
				});
			}
		}

		private ProcessTerminateEvent _terminate1;
		public ProcessTerminateEvent Terminate1 {
			get {
				return _terminate1 ?? (_terminate1 = new ProcessTerminateEvent() {
					UId = Guid.NewGuid(),
					Owner = this,
					Type = "ProcessSchemaTerminateEvent",
					Name = "Terminate1",
					SchemaElementUId = new Guid("cd2459a2-adeb-4627-90c3-87821a6fdef9"),
					CreatedInSchemaUId = InternalSchemaUId,
					ExecutedEventHandler = OnExecuted,
					IsLogging = true,
				});
			}
		}

		private ProcessScriptTask _setServiceRequestOverdues;
		public ProcessScriptTask SetServiceRequestOverdues {
			get {
				return _setServiceRequestOverdues ?? (_setServiceRequestOverdues = new ProcessScriptTask() {
					UId = Guid.NewGuid(),
					Owner = this,
					Type = "ProcessSchemaScriptTask",
					Name = "SetServiceRequestOverdues",
					SchemaElementUId = new Guid("61b6c3f5-777d-4ec0-b83d-8a45acb801e0"),
					CreatedInSchemaUId = InternalSchemaUId,
					ExecutedEventHandler = OnExecuted,
					IsLogging = true,
					Script = SetServiceRequestOverduesExecute,
				});
			}
		}

		#endregion

		#region Methods: Private

		private void InitializeFlowElements() {
			FlowElements[Start1.SchemaElementUId] = new Collection<ProcessFlowElement> { Start1 };
			FlowElements[Terminate1.SchemaElementUId] = new Collection<ProcessFlowElement> { Terminate1 };
			FlowElements[SetServiceRequestOverdues.SchemaElementUId] = new Collection<ProcessFlowElement> { SetServiceRequestOverdues };
		}

		private void OnExecuted(object sender, ProcessActivityAfterEventArgs e) {
			switch (e.Context.SenderName) {
					case "Start1":
						e.Context.QueueTasksV2.Enqueue(new ProcessQueueElement("SetServiceRequestOverdues", e.Context.SenderName));
						break;
					case "Terminate1":
						CompleteProcess();
						break;
					case "SetServiceRequestOverdues":
						e.Context.QueueTasksV2.Enqueue(new ProcessQueueElement("Terminate1", e.Context.SenderName));
						break;
			}
		}

		private void WritePropertyValues(DataWriter writer, bool useAllValueSources) {
		}

		#endregion

		#region Methods: Protected

		protected override void PrepareStart(ProcessExecutingContext context) {
			base.PrepareStart(context);
			context.Process = this;
			if (IsProcessExecutedBySignal) {
				return;
			}
			context.QueueTasksV2.Enqueue(new ProcessQueueElement("Start1", string.Empty));
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
		}

		protected override void WritePropertyValues(DataWriter writer) {
			base.WritePropertyValues(writer);
			WritePropertyValues(writer, true);
		}

		#endregion

		#region Methods: Public

		public virtual bool SetServiceRequestOverduesExecute(ProcessExecutingContext context) {
			var update = new Update(UserConnection, "Case").WithHints(new RowLockHint())
				.Set("ResponseOverdue", Column.Const("1"))
				.Where("StatusId").Not().IsNull()
				.And()
				.OpenBlock()
					.OpenBlock("RespondedOn").IsNull()
					.And("ResponseDate").IsLessOrEqual(Column.Parameter(DateTime.UtcNow))
					.CloseBlock()
					.Or()
					.OpenBlock("RespondedOn").Not().IsNull()
					.And("ResponseDate").IsLessOrEqual("RespondedOn")
					.CloseBlock()
				.CloseBlock()
				.And("ResponseOverdue").IsNotEqual(Column.Const("1")) as Update;
			update.Execute();
			
			update = new Update(UserConnection, "Case").WithHints(new RowLockHint())
				.Set("SolutionOverdue", Column.Const("1"))
				.Where("StatusId").Not().IsNull()
				.And()
				.OpenBlock()
					.OpenBlock("SolutionProvidedOn").IsNull()
					.And("SolutionDate").IsLessOrEqual(Column.Parameter(DateTime.UtcNow))
					.CloseBlock()
					.Or()
					.OpenBlock("SolutionProvidedOn").Not().IsNull()
					.And("SolutionDate").IsLessOrEqual("SolutionProvidedOn")
					.CloseBlock()
				.CloseBlock()
				.And("SolutionOverdue").IsNotEqual(Column.Const("1")) as Update;
			update.Execute();
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
			var cloneItem = (CaseOverduesSettingProcess)base.CloneShallow();
			cloneItem.ExecutedEventHandler = ExecutedEventHandler;
			return cloneItem;
		}

		#endregion

	}

	#endregion

}


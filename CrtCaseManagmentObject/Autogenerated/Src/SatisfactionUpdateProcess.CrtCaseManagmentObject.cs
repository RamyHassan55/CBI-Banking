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
	using Terrasoft.Configuration;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;
	using Terrasoft.Core.DB;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Process;
	using Terrasoft.Core.Process.Configuration;

	#region Class: SatisfactionUpdateProcessSchema

	/// <exclude/>
	public class SatisfactionUpdateProcessSchema : Terrasoft.Core.Process.ProcessSchema
	{

		#region Constructors: Public

		public SatisfactionUpdateProcessSchema(ProcessSchemaManager processSchemaManager)
			: base(processSchemaManager) {
		}

		public SatisfactionUpdateProcessSchema(SatisfactionUpdateProcessSchema source)
			: base(source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			Name = "SatisfactionUpdateProcess";
			UId = new Guid("92753ebc-e72e-41f5-8d5a-68c4b7974bca");
			CreatedInPackageId = new Guid("9141d462-7675-4656-8ea8-25b81010cd22");
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
			TaskFillDefColor = Color.FromArgb(-1);
			UsageType = ProcessSchemaUsageType.Advanced;
			ZipMethodsBody = new byte[] {  };
			ZipCompiledMethodsBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,109,144,221,106,131,64,16,133,175,21,124,135,173,87,43,136,47,144,54,16,210,144,139,164,144,96,219,251,173,142,205,192,186,202,254,88,164,244,221,187,63,45,209,68,216,155,101,230,156,239,204,73,226,222,124,112,172,200,128,82,27,198,201,51,86,26,59,193,228,248,136,66,231,100,111,176,94,147,61,232,235,160,100,26,85,195,252,247,8,3,112,154,145,239,36,142,6,38,201,174,60,147,39,34,224,139,236,132,70,61,150,213,5,90,118,54,32,71,250,166,64,110,59,33,192,43,139,233,194,11,19,236,19,100,78,210,59,243,52,91,37,177,181,45,54,117,189,225,60,236,111,59,110,90,161,168,155,57,172,186,85,41,155,194,137,108,240,192,177,10,30,192,55,57,254,45,228,244,196,191,27,22,203,240,208,166,147,192,170,11,93,132,19,20,11,137,174,37,245,157,117,179,136,187,29,23,247,117,236,161,14,247,189,51,110,192,145,215,52,61,57,141,239,34,194,134,62,204,210,22,246,26,205,80,168,3,140,212,155,103,1,22,205,215,108,131,97,156,47,160,79,18,91,187,52,33,123,216,79,18,219,103,125,140,20,243,142,86,126,146,196,191,205,204,68,9,67,2,0,0 };
			RealUId = new Guid("92753ebc-e72e-41f5-8d5a-68c4b7974bca");
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
			ProcessSchemaScriptTask setsatisfactionlevelscripttask = CreateSetSatisfactionLevelScriptTaskScriptTask();
			FlowElements.Add(setsatisfactionlevelscripttask);
			FlowElements.Add(CreateSequenceFlow1SequenceFlow());
			FlowElements.Add(CreateSequenceFlow2SequenceFlow());
		}

		protected virtual ProcessSchemaSequenceFlow CreateSequenceFlow1SequenceFlow() {
			ProcessSchemaSequenceFlow schemaFlow = new ProcessSchemaSequenceFlow(this, ProcessSchemaEditSequenceFlowType.Sequence) {
				Name = "SequenceFlow1",
				UId = new Guid("c35981e6-30bd-4881-a05d-ea9eb86708a0"),
				BackgroundModePriority = BackgroundModePriority.Inherited,
				ContainerItemIndex = 0,
				ContainerUId = Guid.Empty,
				CreatedInOwnerSchemaUId = new Guid("bb4d6607-026b-4b27-b640-8f5c77c1e89d"),
				CreatedInPackageId = new Guid("9141d462-7675-4656-8ea8-25b81010cd22"),
				CreatedInSchemaUId = new Guid("92753ebc-e72e-41f5-8d5a-68c4b7974bca"),
				CurveCenterPosition = new Point(373, 203),
				DragGroupName = @"SequenceFlow",
				FlowType = ProcessSchemaEditSequenceFlowType.Sequence,
				ManagerItemUId = new Guid("0d8351f6-c2f4-4737-bdd9-6fbfe0837fec"),
				ModifiedInSchemaUId = new Guid("92753ebc-e72e-41f5-8d5a-68c4b7974bca"),
				SequenceFlowEndPointPosition = new Point(0, 0),
				SequenceFlowStartPointPosition = new Point(0, 0),
				Size = new Size(0, 0),
				SourceRefUId = new Guid("03ed6b2a-7daa-4d2e-acc8-3c5231eb7a36"),
				SourceSequenceFlowPointLocalPosition = new Point(0, 0),
				StrokeColor = Color.FromArgb(-7105128),
				TargetRefUId = new Guid("d1cfdcbf-8b95-4b1f-96a9-755138c91dd2"),
				TargetSequenceFlowPointLocalPosition = new Point(0, 0),
				UseBackgroundMode = false,
				VisualType = ProcessSchemaSequenceFlowVisualType.AutoPolyline
			};
			return schemaFlow;
		}

		protected virtual ProcessSchemaSequenceFlow CreateSequenceFlow2SequenceFlow() {
			ProcessSchemaSequenceFlow schemaFlow = new ProcessSchemaSequenceFlow(this, ProcessSchemaEditSequenceFlowType.Sequence) {
				Name = "SequenceFlow2",
				UId = new Guid("01d452f2-ee08-430e-a761-667ecdf437b2"),
				BackgroundModePriority = BackgroundModePriority.Inherited,
				ContainerItemIndex = 0,
				ContainerUId = Guid.Empty,
				CreatedInOwnerSchemaUId = new Guid("bb4d6607-026b-4b27-b640-8f5c77c1e89d"),
				CreatedInPackageId = new Guid("9141d462-7675-4656-8ea8-25b81010cd22"),
				CreatedInSchemaUId = new Guid("92753ebc-e72e-41f5-8d5a-68c4b7974bca"),
				CurveCenterPosition = new Point(373, 203),
				DragGroupName = @"SequenceFlow",
				FlowType = ProcessSchemaEditSequenceFlowType.Sequence,
				ManagerItemUId = new Guid("0d8351f6-c2f4-4737-bdd9-6fbfe0837fec"),
				ModifiedInSchemaUId = new Guid("92753ebc-e72e-41f5-8d5a-68c4b7974bca"),
				SequenceFlowEndPointPosition = new Point(0, 0),
				SequenceFlowStartPointPosition = new Point(0, 0),
				Size = new Size(0, 0),
				SourceRefUId = new Guid("d1cfdcbf-8b95-4b1f-96a9-755138c91dd2"),
				SourceSequenceFlowPointLocalPosition = new Point(0, 0),
				StrokeColor = Color.FromArgb(-7105128),
				TargetRefUId = new Guid("bf5a4500-60e8-49a7-b936-cab4fc19a130"),
				TargetSequenceFlowPointLocalPosition = new Point(0, 0),
				UseBackgroundMode = false,
				VisualType = ProcessSchemaSequenceFlowVisualType.AutoPolyline
			};
			return schemaFlow;
		}

		protected virtual ProcessSchemaLaneSet CreateLaneSet1LaneSet() {
			ProcessSchemaLaneSet schemaLaneSet1 = new ProcessSchemaLaneSet(this) {
				UId = new Guid("8f1a79b9-e0b9-4065-bb4c-6773b9d9a52f"),
				BackgroundModePriority = BackgroundModePriority.Inherited,
				ContainerItemIndex = 1,
				ContainerUId = Guid.Empty,
				CreatedInOwnerSchemaUId = new Guid("bb4d6607-026b-4b27-b640-8f5c77c1e89d"),
				CreatedInPackageId = new Guid("9141d462-7675-4656-8ea8-25b81010cd22"),
				CreatedInSchemaUId = new Guid("92753ebc-e72e-41f5-8d5a-68c4b7974bca"),
				Direction = ProcessSchemaPoolDirectionType.Vertical,
				DragGroupName = @"LaneSet",
				ManagerItemUId = new Guid("11a47caf-a0d5-41fa-a274-a0b11f77447a"),
				ModifiedInSchemaUId = new Guid("92753ebc-e72e-41f5-8d5a-68c4b7974bca"),
				Name = @"LaneSet1",
				Position = new Point(5, 5),
				Size = new Size(0, 0),
				UseBackgroundMode = false
			};
			return schemaLaneSet1;
		}

		protected virtual ProcessSchemaLane CreateLane1Lane() {
			ProcessSchemaLane schemaLane1 = new ProcessSchemaLane(this) {
				UId = new Guid("28ed4bf2-6990-4640-a02d-25e42219854d"),
				BackgroundModePriority = BackgroundModePriority.Inherited,
				ContainerItemIndex = 1,
				ContainerUId = new Guid("8f1a79b9-e0b9-4065-bb4c-6773b9d9a52f"),
				CreatedInOwnerSchemaUId = Guid.Empty,
				CreatedInPackageId = new Guid("9141d462-7675-4656-8ea8-25b81010cd22"),
				CreatedInSchemaUId = new Guid("92753ebc-e72e-41f5-8d5a-68c4b7974bca"),
				DragGroupName = @"Lane",
				ManagerItemUId = new Guid("abcd74b9-5912-414b-82ac-f1aa4dcd554e"),
				ModifiedInSchemaUId = new Guid("92753ebc-e72e-41f5-8d5a-68c4b7974bca"),
				Name = @"Lane1",
				Position = new Point(29, 0),
				Size = new Size(0, 0),
				UseBackgroundMode = false
			};
			return schemaLane1;
		}

		protected virtual ProcessSchemaStartEvent CreateStart1StartEvent() {
			ProcessSchemaStartEvent schemaStartEvent = new ProcessSchemaStartEvent(this) {
				UId = new Guid("03ed6b2a-7daa-4d2e-acc8-3c5231eb7a36"),
				BackgroundModePriority = BackgroundModePriority.Inherited,
				ContainerItemIndex = 0,
				ContainerUId = new Guid("28ed4bf2-6990-4640-a02d-25e42219854d"),
				CreatedInOwnerSchemaUId = new Guid("bb4d6607-026b-4b27-b640-8f5c77c1e89d"),
				CreatedInPackageId = new Guid("9141d462-7675-4656-8ea8-25b81010cd22"),
				CreatedInSchemaUId = new Guid("92753ebc-e72e-41f5-8d5a-68c4b7974bca"),
				DragGroupName = @"Event",
				EntitySchemaUId = Guid.Empty,
				IsInterrupting = false,
				IsLogging = true,
				ManagerItemUId = new Guid("53818048-7868-48f6-ada0-0ebaa65af628"),
				ModifiedInSchemaUId = new Guid("92753ebc-e72e-41f5-8d5a-68c4b7974bca"),
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
				UId = new Guid("bf5a4500-60e8-49a7-b936-cab4fc19a130"),
				BackgroundModePriority = BackgroundModePriority.Inherited,
				ContainerItemIndex = 0,
				ContainerUId = new Guid("28ed4bf2-6990-4640-a02d-25e42219854d"),
				CreatedInOwnerSchemaUId = new Guid("bb4d6607-026b-4b27-b640-8f5c77c1e89d"),
				CreatedInPackageId = new Guid("9141d462-7675-4656-8ea8-25b81010cd22"),
				CreatedInSchemaUId = new Guid("92753ebc-e72e-41f5-8d5a-68c4b7974bca"),
				DragGroupName = @"Event",
				EntitySchemaUId = Guid.Empty,
				IsLogging = true,
				ManagerItemUId = new Guid("1bd93619-0574-454e-bb4e-cf53b9eb9470"),
				ModifiedInSchemaUId = new Guid("92753ebc-e72e-41f5-8d5a-68c4b7974bca"),
				Name = @"Terminate1",
				Position = new Point(309, 184),
				SerializeToDB = false,
				Size = new Size(27, 27),
				ThrowToBase = false,
				UseBackgroundMode = false
			};
			
			return schemaTerminateEvent;
		}

		protected virtual ProcessSchemaScriptTask CreateSetSatisfactionLevelScriptTaskScriptTask() {
			ProcessSchemaScriptTask ScriptTask = new ProcessSchemaScriptTask(this) {
				UId = new Guid("d1cfdcbf-8b95-4b1f-96a9-755138c91dd2"),
				BackgroundModePriority = BackgroundModePriority.Inherited,
				ContainerItemIndex = 0,
				ContainerUId = new Guid("28ed4bf2-6990-4640-a02d-25e42219854d"),
				CreatedInOwnerSchemaUId = new Guid("bb4d6607-026b-4b27-b640-8f5c77c1e89d"),
				CreatedInPackageId = new Guid("9141d462-7675-4656-8ea8-25b81010cd22"),
				CreatedInSchemaUId = new Guid("92753ebc-e72e-41f5-8d5a-68c4b7974bca"),
				DragGroupName = @"Task",
				EntitySchemaUId = Guid.Empty,
				FillColor = Color.FromArgb(-1),
				ImageList = null,
				ImageName = null,
				IsForCompensation = false,
				IsLogging = true,
				ManagerItemUId = new Guid("0e490dda-e140-4441-b600-6f5c64d024df"),
				ModifiedInSchemaUId = new Guid("92753ebc-e72e-41f5-8d5a-68c4b7974bca"),
				Name = @"SetSatisfactionLevelScriptTask",
				Position = new Point(155, 170),
				SerializeToDB = false,
				Size = new Size(69, 55),
				UseBackgroundMode = false,
				UseFlowEngineScriptVersion = false,
				ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,157,85,93,111,218,48,20,125,102,210,254,131,155,39,71,66,209,158,183,209,137,210,15,161,117,45,43,235,250,80,245,193,74,46,96,41,113,152,237,180,139,42,254,123,175,175,19,98,2,98,90,145,0,197,57,247,220,115,63,61,120,22,154,129,249,195,70,76,193,11,187,80,86,218,122,158,174,160,16,63,43,208,53,191,55,160,39,165,82,144,90,89,170,36,4,252,16,74,44,65,15,89,52,23,86,154,133,32,200,253,58,19,22,162,248,203,199,15,3,36,78,102,90,22,66,215,196,54,41,243,170,80,201,212,140,243,23,81,155,57,228,72,139,174,173,174,192,225,141,213,82,45,89,170,1,41,178,91,229,241,55,162,0,4,57,178,113,150,249,51,30,77,90,80,20,39,183,58,3,125,86,159,131,73,121,156,56,124,200,38,12,76,179,163,84,136,72,166,89,180,103,186,46,165,178,199,44,103,14,176,111,151,150,69,1,199,45,39,30,210,218,14,252,7,41,124,134,17,150,251,148,179,138,18,106,26,138,43,176,125,68,175,70,148,121,185,96,252,164,177,76,198,170,230,113,204,94,241,124,160,193,86,90,109,51,190,193,175,107,1,138,212,52,93,112,46,137,8,171,246,245,170,146,217,144,133,245,189,134,103,200,41,238,83,78,174,22,37,86,34,93,49,238,117,49,240,127,114,171,188,241,236,220,200,204,133,65,0,23,201,175,122,13,77,70,126,139,188,2,114,119,202,251,21,35,55,20,146,151,153,96,176,86,72,101,190,67,205,101,214,134,214,5,130,78,60,242,81,102,79,100,60,160,231,164,201,58,190,247,149,194,86,188,169,242,252,86,95,20,107,91,243,29,80,204,190,29,147,234,9,80,108,191,214,49,251,204,118,136,66,1,179,80,94,251,52,98,159,142,251,162,100,247,154,177,115,67,44,228,100,195,32,55,208,100,163,201,21,246,28,119,53,116,133,61,92,71,246,202,90,89,255,171,97,72,158,6,93,90,223,149,48,34,217,248,42,111,130,166,204,112,63,88,192,34,183,141,121,45,141,109,122,132,208,65,163,162,174,33,163,87,204,244,163,116,230,168,168,67,239,229,193,211,133,203,141,214,198,206,193,136,253,123,23,38,151,82,101,83,101,172,80,41,156,213,46,58,191,94,162,192,65,64,141,164,125,63,137,95,108,254,232,208,104,119,6,9,190,29,103,133,84,119,114,185,162,233,93,8,172,255,206,80,118,51,129,3,233,91,162,25,23,55,79,1,215,37,216,116,117,169,203,226,252,172,153,3,76,252,206,104,129,177,184,202,45,108,219,151,234,26,180,31,81,238,103,127,103,92,91,146,45,115,24,207,28,108,208,49,60,218,171,19,46,233,225,129,250,62,182,172,79,190,137,168,131,188,156,147,222,160,63,172,164,133,249,90,164,192,195,32,218,153,127,151,170,118,151,15,217,33,202,80,81,200,42,158,129,83,189,66,4,253,116,125,239,166,247,78,168,37,240,118,151,251,59,147,87,108,116,202,170,246,114,13,228,197,221,178,12,105,130,27,32,24,45,186,148,219,165,79,39,253,134,243,201,72,92,95,240,131,215,124,3,120,88,129,198,212,208,37,58,85,188,185,232,103,66,227,4,88,208,38,16,131,58,132,105,220,249,200,3,49,201,197,95,72,43,212,17,36,37,188,178,222,0,207,201,84,10,176,8,0,0 }
			};
			
			return ScriptTask;
		}

		protected override void InitializeMethods() {
			base.InitializeMethods();
			SetMethodsDefInheritance();
		}

		protected override void InitializeUsings() {
			Usings.Add(new SchemaUsing() {
				UId = new Guid("ca2c725f-edc8-445a-a394-c112481e1f94"),
				Name = "System.Linq",
				Alias = "",
				CreatedInPackageId = new Guid("9141d462-7675-4656-8ea8-25b81010cd22")
			});
			Usings.Add(new SchemaUsing() {
				UId = new Guid("2c267f4e-2dc4-4414-8725-4fc3c492c5aa"),
				Name = "Terrasoft.Configuration",
				Alias = "",
				CreatedInPackageId = new Guid("9141d462-7675-4656-8ea8-25b81010cd22")
			});
		}

		#endregion

		#region Methods: Public

		public override Process CreateProcess(UserConnection userConnection) {
			return new SatisfactionUpdateProcess(userConnection);
		}

		public override object Clone() {
			return new SatisfactionUpdateProcessSchema(this);
		}

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("92753ebc-e72e-41f5-8d5a-68c4b7974bca"));
		}

		#endregion

	}

	#endregion

	#region Class: SatisfactionUpdateProcess

	/// <exclude/>
	public class SatisfactionUpdateProcess : Terrasoft.Core.Process.Process
	{

		#region Class: ProcessLane1

		/// <exclude/>
		public class ProcessLane1 : ProcessLane
		{

			public ProcessLane1(UserConnection userConnection, SatisfactionUpdateProcess process)
				: base(userConnection) {
				Owner = process;
				IsUsedParentUserContexts = false;
			}

		}

		#endregion

		public SatisfactionUpdateProcess(UserConnection userConnection)
			: base(userConnection) {
			InitializeMetaPathParameterValues();
			UId = Guid.NewGuid();
			Name = "SatisfactionUpdateProcess";
			SchemaUId = new Guid("92753ebc-e72e-41f5-8d5a-68c4b7974bca");
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
				return new Guid("92753ebc-e72e-41f5-8d5a-68c4b7974bca");
			}
		}

		private string ConditionalExpressionLogMessage {
			get {
				return "Process: SatisfactionUpdateProcess, Source element: {0}, Conditional flow: {1}, Expression: {2}, Result: {3}";
			}
		}

		private string DeadEndGatewayLogMessage {
			get {
				return "Process: SatisfactionUpdateProcess, Source element: {0}, None of the exclusive gateway conditions are met!";
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
					SchemaElementUId = new Guid("03ed6b2a-7daa-4d2e-acc8-3c5231eb7a36"),
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
					SchemaElementUId = new Guid("bf5a4500-60e8-49a7-b936-cab4fc19a130"),
					CreatedInSchemaUId = InternalSchemaUId,
					ExecutedEventHandler = OnExecuted,
					IsLogging = true,
				});
			}
		}

		private ProcessScriptTask _setSatisfactionLevelScriptTask;
		public ProcessScriptTask SetSatisfactionLevelScriptTask {
			get {
				return _setSatisfactionLevelScriptTask ?? (_setSatisfactionLevelScriptTask = new ProcessScriptTask() {
					UId = Guid.NewGuid(),
					Owner = this,
					Type = "ProcessSchemaScriptTask",
					Name = "SetSatisfactionLevelScriptTask",
					SchemaElementUId = new Guid("d1cfdcbf-8b95-4b1f-96a9-755138c91dd2"),
					CreatedInSchemaUId = InternalSchemaUId,
					ExecutedEventHandler = OnExecuted,
					IsLogging = true,
					Script = SetSatisfactionLevelScriptTaskExecute,
				});
			}
		}

		#endregion

		#region Methods: Private

		private void InitializeFlowElements() {
			FlowElements[Start1.SchemaElementUId] = new Collection<ProcessFlowElement> { Start1 };
			FlowElements[Terminate1.SchemaElementUId] = new Collection<ProcessFlowElement> { Terminate1 };
			FlowElements[SetSatisfactionLevelScriptTask.SchemaElementUId] = new Collection<ProcessFlowElement> { SetSatisfactionLevelScriptTask };
		}

		private void OnExecuted(object sender, ProcessActivityAfterEventArgs e) {
			switch (e.Context.SenderName) {
					case "Start1":
						e.Context.QueueTasksV2.Enqueue(new ProcessQueueElement("SetSatisfactionLevelScriptTask", e.Context.SenderName));
						break;
					case "Terminate1":
						CompleteProcess();
						break;
					case "SetSatisfactionLevelScriptTask":
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

		public virtual bool SetSatisfactionLevelScriptTaskExecute(ProcessExecutingContext context) {
				var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "SatisfactionUpdate");
				esq.PrimaryQueryColumn.IsAlwaysSelect = true;
				string createdOnColumnName = esq.AddColumn("CreatedOn").OrderByDesc().Name;
				string caseIdColumnName = esq.AddColumn("Case.Id").Name;
				string pointColumnName = esq.AddColumn("Point").Name;
				string commentColumnName = esq.AddColumn("Comment").Name;							
				EntityCollection updates = esq.GetEntityCollection(UserConnection);
				if (!updates.Any()) {
					return true;
				}
				var points = new Dictionary<Guid, SatisfactionLevelPoint>();
				foreach (Entity entity in updates) {
					var id = entity.GetTypedColumnValue<Guid>(caseIdColumnName);
					if (points.ContainsKey(id)) {
						var point = points[id];
						point.Comment = string.IsNullOrEmpty(point.Comment) ? entity.GetTypedColumnValue<string>(commentColumnName) : point.Comment;
						point.Point = point.Point == 0 ? entity.GetTypedColumnValue<int>(pointColumnName) : point.Point;
					} else {
						points.Add(id, new SatisfactionLevelPoint { Point = entity.GetTypedColumnValue<int>(pointColumnName),
							Comment = entity.GetTypedColumnValue<string>(commentColumnName)
						});
					}
				}
				var deleteKeys = new List<Guid>();
				Dictionary<int, Guid> satisfactionLevels = GetDictionarySatisfactionLevel();
				EntitySchema caseEntitySchema = UserConnection.EntitySchemaManager.FindInstanceByName("Case");
				Entity caseEntity = caseEntitySchema.CreateEntity(UserConnection);
				caseEntity.UseAdminRights = false;
				foreach (var point in points) {
					if (caseEntity.FetchFromDB(point.Key)) {
						var estimate = point.Value.Point;
						if (satisfactionLevels.ContainsKey(estimate)) {
							caseEntity.SetColumnValue("SatisfactionLevelId", satisfactionLevels[estimate]);
						}
						if (!string.IsNullOrWhiteSpace(point.Value.Comment)) {
							caseEntity.SetColumnValue("SatisfactionLevelComment", point.Value.Comment);
						}
						caseEntity.Save(false);
						}
					}
					deleteKeys.AddRange(updates.Select(u => u.PrimaryColumnValue));
					if (deleteKeys.Any()) {
						var deleteQuery = new Delete(UserConnection)
							.From("SatisfactionUpdate")
							.Where("Id").In(Column.Parameters(deleteKeys)) as Delete;
						deleteQuery.Execute();
						}
				return true;
		}

			
			public virtual Dictionary<int, Guid> GetDictionarySatisfactionLevel() {
				var ESQ = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "SatisfactionLevel");
			ESQ.AddAllSchemaColumns();
			var satisfactionLevels = ESQ.GetEntityCollection(UserConnection);
			var retDictionary = new Dictionary<int, Guid>();
			foreach(var satisfactionLevel in satisfactionLevels) {
				var point = satisfactionLevel.GetTypedColumnValue<int>("Point");
				if(!retDictionary.ContainsKey(point)) {
					retDictionary.Add(point, satisfactionLevel.PrimaryColumnValue);
				}
			}
			return retDictionary;
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
			var cloneItem = (SatisfactionUpdateProcess)base.CloneShallow();
			cloneItem.ExecutedEventHandler = ExecutedEventHandler;
			return cloneItem;
		}

		#endregion

	}

	#endregion

}


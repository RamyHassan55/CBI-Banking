namespace Terrasoft.Core.Process
{

	using Creatio.FeatureToggling;
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Drawing;
	using System.Globalization;
	using System.Text;
	using Terrasoft.Common;
	using Terrasoft.Configuration;
	using Terrasoft.Configuration.Calendars;
	using Terrasoft.Configuration.TermCalculationService;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;
	using Terrasoft.Core.DB;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Factories;
	using Terrasoft.Core.Process;
	using Terrasoft.Core.Process.Configuration;

	#region Class: ActualizeCalculationTermsAfterStatusChangeSchema

	/// <exclude/>
	public class ActualizeCalculationTermsAfterStatusChangeSchema : Terrasoft.Core.Process.ProcessSchema
	{

		#region Constructors: Public

		public ActualizeCalculationTermsAfterStatusChangeSchema(ProcessSchemaManager processSchemaManager)
			: base(processSchemaManager) {
		}

		public ActualizeCalculationTermsAfterStatusChangeSchema(ActualizeCalculationTermsAfterStatusChangeSchema source)
			: base(source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			Name = "ActualizeCalculationTermsAfterStatusChange";
			UId = new Guid("12ed463d-51aa-4d38-a7ff-929a9bc8beee");
			CreatedInPackageId = new Guid("c4b72b51-b2ae-4127-a458-608f5464622c");
			CreatedInSchemaUId = Guid.Empty;
			CreatedInVersion = @"0.0.0.0";
			CultureName = null;
			EntitySchemaUId = Guid.Empty;
			IsCreatedInSvg = true;
			ModifiedInSchemaUId = Guid.Empty;
			ParametersEditPageSchemaUId = Guid.Empty;
			ParentSchemaUId = new Guid("bb4d6607-026b-4b27-b640-8f5c77c1e89d");
			SequenceFlowStrokeDefColor = Color.FromArgb(-4473925);
			SerializeToDB = false;
			SerializeToMemory = true;
			Tag = @"Business Process";
			TaskFillDefColor = Color.FromArgb(-1);
			UsageType = ProcessSchemaUsageType.Advanced;
			ZipMethodsBody = new byte[] {  };
			ZipCompiledMethodsBody = new byte[] {  };
			RealUId = new Guid("12ed463d-51aa-4d38-a7ff-929a9bc8beee");
			Version = 0;
			PackageUId = new Guid("ff2cadfe-f4c2-06cd-930f-1401ae848c21");
			UseSystemSecurityContext = false;
		}

		protected virtual ProcessSchemaParameter CreateCaseRecordIdParameter() {
			return new ProcessSchemaParameter(this) {
			UId = new Guid("5ba3b2b0-c2db-44e1-bf19-e058ea310f85"),
				ContainerUId = Guid.Empty,
				CreatedInOwnerSchemaUId = Guid.Empty,
				CreatedInPackageId = Guid.Empty,
				CreatedInSchemaUId = new Guid("12ed463d-51aa-4d38-a7ff-929a9bc8beee"),
				Direction = ProcessSchemaParameterDirection.Variable,
				IsCopyValue = false,
				IsLazy = false,
				IsPerformer = false,
				IsRequired = false,
				IsResult = false,
				IsValueSerializable = true,
				ModifiedInSchemaUId = new Guid("12ed463d-51aa-4d38-a7ff-929a9bc8beee"),
				Name = @"CaseRecordId",
				SourceParameterUId = Guid.Empty,
				Tag = @"",
				UseBackgroundMode = false,
				DataValueType = DataValueTypeManager.GetInstanceByName("Guid"),
			};
		}

		protected override void InitializeParameters() {
			base.InitializeParameters();
			Parameters.Add(CreateCaseRecordIdParameter());
		}

		protected virtual void InitializeStartSignal1Parameters(IParametrizedProcessSchemaElement parametrizedElement) {
			var recordIdParameter = new ProcessSchemaParameter(this) {
				UId = new Guid("36f4dbcc-6d57-4748-9968-0246e02c237a"),
				ContainerUId = new Guid("1c01f230-4666-435f-8da3-235234c5161f"),
				CreatedInOwnerSchemaUId = Guid.Empty,
				CreatedInPackageId = Guid.Empty,
				CreatedInSchemaUId = new Guid("12ed463d-51aa-4d38-a7ff-929a9bc8beee"),
				Direction = ProcessSchemaParameterDirection.Variable,
				IsCopyValue = false,
				IsLazy = false,
				IsPerformer = false,
				IsRequired = false,
				IsResult = false,
				IsValueSerializable = true,
				ModifiedInSchemaUId = new Guid("12ed463d-51aa-4d38-a7ff-929a9bc8beee"),
				Name = @"RecordId",
				SourceParameterUId = Guid.Empty,
				Tag = @"",
				UseBackgroundMode = false,
				DataValueType = DataValueTypeManager.GetInstanceByName("Guid")
			};
			recordIdParameter.SourceValue = new ProcessSchemaParameterValue(recordIdParameter) {
				Source = ProcessSchemaParameterValueSource.ConstValue,
				Value = @"",
				MetaPath = null,
				ModifiedInSchemaUId = Guid.Empty
			};
			parametrizedElement.Parameters.Add(recordIdParameter);
		}

		protected virtual void InitializeStartSignal2Parameters(IParametrizedProcessSchemaElement parametrizedElement) {
			var recordIdParameter = new ProcessSchemaParameter(this) {
				UId = new Guid("39910edb-c399-4a13-885f-46917fb764cf"),
				ContainerUId = new Guid("8b7c37ff-f3d2-4834-907b-952c0d815039"),
				CreatedInOwnerSchemaUId = Guid.Empty,
				CreatedInPackageId = Guid.Empty,
				CreatedInSchemaUId = new Guid("12ed463d-51aa-4d38-a7ff-929a9bc8beee"),
				Direction = ProcessSchemaParameterDirection.Variable,
				IsCopyValue = false,
				IsLazy = false,
				IsPerformer = false,
				IsRequired = false,
				IsResult = false,
				IsValueSerializable = true,
				ModifiedInSchemaUId = new Guid("12ed463d-51aa-4d38-a7ff-929a9bc8beee"),
				Name = @"RecordId",
				SourceParameterUId = Guid.Empty,
				Tag = @"",
				UseBackgroundMode = false,
				DataValueType = DataValueTypeManager.GetInstanceByName("Guid")
			};
			recordIdParameter.SourceValue = new ProcessSchemaParameterValue(recordIdParameter) {
				Source = ProcessSchemaParameterValueSource.ConstValue,
				Value = @"",
				MetaPath = null,
				ModifiedInSchemaUId = Guid.Empty
			};
			parametrizedElement.Parameters.Add(recordIdParameter);
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
			ProcessSchemaScriptTask scripttask1 = CreateScriptTask1ScriptTask();
			FlowElements.Add(scripttask1);
			ProcessSchemaStartSignalEvent startsignal2 = CreateStartSignal2StartSignalEvent();
			FlowElements.Add(startsignal2);
			FlowElements.Add(CreateSequenceFlow2SequenceFlow());
			FlowElements.Add(CreateSequenceFlow1SequenceFlow());
			FlowElements.Add(CreateSequenceFlow3SequenceFlow());
		}

		protected virtual ProcessSchemaSequenceFlow CreateSequenceFlow2SequenceFlow() {
			ProcessSchemaSequenceFlow schemaFlow = new ProcessSchemaSequenceFlow(this, ProcessSchemaEditSequenceFlowType.Sequence) {
				Name = "SequenceFlow2",
				UId = new Guid("257986d2-a8ba-4d76-ba8c-9f0e519ca699"),
				BackgroundModePriority = BackgroundModePriority.Inherited,
				ContainerItemIndex = 0,
				ContainerUId = Guid.Empty,
				CreatedInOwnerSchemaUId = new Guid("bb4d6607-026b-4b27-b640-8f5c77c1e89d"),
				CreatedInPackageId = new Guid("ff2cadfe-f4c2-06cd-930f-1401ae848c21"),
				CreatedInSchemaUId = new Guid("12ed463d-51aa-4d38-a7ff-929a9bc8beee"),
				CurveCenterPosition = new Point(0, 0),
				FlowType = ProcessSchemaEditSequenceFlowType.Sequence,
				ManagerItemUId = new Guid("0d8351f6-c2f4-4737-bdd9-6fbfe0837fec"),
				ModifiedInSchemaUId = new Guid("12ed463d-51aa-4d38-a7ff-929a9bc8beee"),
				SequenceFlowEndPointPosition = new Point(0, 0),
				SequenceFlowStartPointPosition = new Point(0, 0),
				Size = new Size(0, 0),
				SourceRefUId = new Guid("1c01f230-4666-435f-8da3-235234c5161f"),
				SourceSequenceFlowPointLocalPosition = new Point(1, 0),
				StrokeColor = Color.FromArgb(-7105128),
				TargetRefUId = new Guid("162d1904-7c40-4313-8c25-e5f44d73777a"),
				TargetSequenceFlowPointLocalPosition = new Point(-1, 0),
				UseBackgroundMode = false,
				VisualType = ProcessSchemaSequenceFlowVisualType.AutoPolyline
			};
			return schemaFlow;
		}

		protected virtual ProcessSchemaSequenceFlow CreateSequenceFlow1SequenceFlow() {
			ProcessSchemaSequenceFlow schemaFlow = new ProcessSchemaSequenceFlow(this, ProcessSchemaEditSequenceFlowType.Sequence) {
				Name = "SequenceFlow1",
				UId = new Guid("ad18dab0-8b0c-4346-87fd-efcbec3397be"),
				BackgroundModePriority = BackgroundModePriority.Inherited,
				ContainerItemIndex = 0,
				ContainerUId = Guid.Empty,
				CreatedInOwnerSchemaUId = new Guid("bb4d6607-026b-4b27-b640-8f5c77c1e89d"),
				CreatedInPackageId = new Guid("ff2cadfe-f4c2-06cd-930f-1401ae848c21"),
				CreatedInSchemaUId = new Guid("12ed463d-51aa-4d38-a7ff-929a9bc8beee"),
				CurveCenterPosition = new Point(0, 0),
				FlowType = ProcessSchemaEditSequenceFlowType.Sequence,
				ManagerItemUId = new Guid("0d8351f6-c2f4-4737-bdd9-6fbfe0837fec"),
				ModifiedInSchemaUId = new Guid("12ed463d-51aa-4d38-a7ff-929a9bc8beee"),
				SequenceFlowEndPointPosition = new Point(0, 0),
				SequenceFlowStartPointPosition = new Point(0, 0),
				Size = new Size(0, 0),
				SourceRefUId = new Guid("162d1904-7c40-4313-8c25-e5f44d73777a"),
				SourceSequenceFlowPointLocalPosition = new Point(1, 0),
				StrokeColor = Color.FromArgb(-7105128),
				TargetRefUId = new Guid("e58a168a-5c48-45e1-81bd-524ca5198111"),
				TargetSequenceFlowPointLocalPosition = new Point(0, 0),
				UseBackgroundMode = false,
				VisualType = ProcessSchemaSequenceFlowVisualType.AutoPolyline
			};
			return schemaFlow;
		}

		protected virtual ProcessSchemaSequenceFlow CreateSequenceFlow3SequenceFlow() {
			ProcessSchemaSequenceFlow schemaFlow = new ProcessSchemaSequenceFlow(this, ProcessSchemaEditSequenceFlowType.Sequence) {
				Name = "SequenceFlow3",
				UId = new Guid("d7441161-8a76-4f69-8caa-abfc6b5ce7a9"),
				BackgroundModePriority = BackgroundModePriority.Inherited,
				ContainerItemIndex = 0,
				ContainerUId = Guid.Empty,
				CreatedInOwnerSchemaUId = new Guid("bb4d6607-026b-4b27-b640-8f5c77c1e89d"),
				CreatedInPackageId = new Guid("ff2cadfe-f4c2-06cd-930f-1401ae848c21"),
				CreatedInSchemaUId = new Guid("12ed463d-51aa-4d38-a7ff-929a9bc8beee"),
				CurveCenterPosition = new Point(0, 0),
				FlowType = ProcessSchemaEditSequenceFlowType.Sequence,
				ManagerItemUId = new Guid("0d8351f6-c2f4-4737-bdd9-6fbfe0837fec"),
				ModifiedInSchemaUId = new Guid("12ed463d-51aa-4d38-a7ff-929a9bc8beee"),
				SequenceFlowEndPointPosition = new Point(0, 0),
				SequenceFlowStartPointPosition = new Point(0, 0),
				Size = new Size(0, 0),
				SourceRefUId = new Guid("8b7c37ff-f3d2-4834-907b-952c0d815039"),
				SourceSequenceFlowPointLocalPosition = new Point(1, 0),
				StrokeColor = Color.FromArgb(-7105128),
				TargetRefUId = new Guid("162d1904-7c40-4313-8c25-e5f44d73777a"),
				TargetSequenceFlowPointLocalPosition = new Point(0, -1),
				UseBackgroundMode = false,
				VisualType = ProcessSchemaSequenceFlowVisualType.AutoPolyline
			};
			schemaFlow.PolylinePointPositions.Add(new Point(363, 294));
			return schemaFlow;
		}

		protected virtual ProcessSchemaLaneSet CreateLaneSet1LaneSet() {
			ProcessSchemaLaneSet schemaLaneSet1 = new ProcessSchemaLaneSet(this) {
				UId = new Guid("8e74ef7a-e4d0-49c7-a6ab-71859ce761e3"),
				BackgroundModePriority = BackgroundModePriority.Inherited,
				ContainerItemIndex = 0,
				ContainerUId = Guid.Empty,
				CreatedInOwnerSchemaUId = new Guid("bb4d6607-026b-4b27-b640-8f5c77c1e89d"),
				CreatedInPackageId = new Guid("ff2cadfe-f4c2-06cd-930f-1401ae848c21"),
				CreatedInSchemaUId = new Guid("12ed463d-51aa-4d38-a7ff-929a9bc8beee"),
				Direction = ProcessSchemaPoolDirectionType.Vertical,
				ManagerItemUId = new Guid("11a47caf-a0d5-41fa-a274-a0b11f77447a"),
				ModifiedInSchemaUId = new Guid("12ed463d-51aa-4d38-a7ff-929a9bc8beee"),
				Name = @"LaneSet1",
				Position = new Point(0, 0),
				Size = new Size(0, 0),
				UseBackgroundMode = false
			};
			return schemaLaneSet1;
		}

		protected virtual ProcessSchemaLane CreateLane1Lane() {
			ProcessSchemaLane schemaLane1 = new ProcessSchemaLane(this) {
				UId = new Guid("9368e760-06e3-4a5f-a5fb-ccb80b19301b"),
				BackgroundModePriority = BackgroundModePriority.Inherited,
				ContainerItemIndex = 0,
				ContainerUId = new Guid("8e74ef7a-e4d0-49c7-a6ab-71859ce761e3"),
				CreatedInOwnerSchemaUId = new Guid("12ed463d-51aa-4d38-a7ff-929a9bc8beee"),
				CreatedInPackageId = new Guid("ff2cadfe-f4c2-06cd-930f-1401ae848c21"),
				CreatedInSchemaUId = new Guid("12ed463d-51aa-4d38-a7ff-929a9bc8beee"),
				ManagerItemUId = new Guid("abcd74b9-5912-414b-82ac-f1aa4dcd554e"),
				ModifiedInSchemaUId = new Guid("12ed463d-51aa-4d38-a7ff-929a9bc8beee"),
				Name = @"Lane1",
				Position = new Point(0, 0),
				Size = new Size(0, 0),
				UseBackgroundMode = false
			};
			return schemaLane1;
		}

		protected virtual ProcessSchemaTerminateEvent CreateTerminateEvent1TerminateEvent() {
			ProcessSchemaTerminateEvent schemaTerminateEvent = new ProcessSchemaTerminateEvent(this) {
				UId = new Guid("e58a168a-5c48-45e1-81bd-524ca5198111"),
				BackgroundModePriority = BackgroundModePriority.Inherited,
				ContainerItemIndex = 0,
				ContainerUId = new Guid("9368e760-06e3-4a5f-a5fb-ccb80b19301b"),
				CreatedInOwnerSchemaUId = new Guid("bb4d6607-026b-4b27-b640-8f5c77c1e89d"),
				CreatedInPackageId = new Guid("ff2cadfe-f4c2-06cd-930f-1401ae848c21"),
				CreatedInSchemaUId = new Guid("12ed463d-51aa-4d38-a7ff-929a9bc8beee"),
				EntitySchemaUId = Guid.Empty,
				IsLogging = true,
				ManagerItemUId = new Guid("1bd93619-0574-454e-bb4e-cf53b9eb9470"),
				ModifiedInSchemaUId = new Guid("12ed463d-51aa-4d38-a7ff-929a9bc8beee"),
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
			ProcessSchemaStartSignalEvent schemaStartSignalEvent = new ProcessSchemaStartSignalEvent(this) {	UId = new Guid("1c01f230-4666-435f-8da3-235234c5161f"),
				BackgroundModePriority = BackgroundModePriority.Inherited,
				ContainerItemIndex = 0,
				ContainerUId = new Guid("9368e760-06e3-4a5f-a5fb-ccb80b19301b"),
				CreatedInOwnerSchemaUId = new Guid("bb4d6607-026b-4b27-b640-8f5c77c1e89d"),
				CreatedInPackageId = new Guid("ff2cadfe-f4c2-06cd-930f-1401ae848c21"),
				CreatedInSchemaUId = new Guid("12ed463d-51aa-4d38-a7ff-929a9bc8beee"),
				EntitySchemaUId = new Guid("117d32f9-8275-4534-8411-1c66115ce9cd"),
				EntitySignal = EntityChangeType.Updated,
				HasEntityColumnChange = true,
				HasEntityFilters = true,
				ImageList = null,
				ImageName = null,
				IsInterrupting = false,
				IsLogging = true,
				ManagerItemUId = new Guid("1129e72f-0e8c-445a-b2ea-463517e86395"),
				ModifiedInSchemaUId = new Guid("12ed463d-51aa-4d38-a7ff-929a9bc8beee"),
				Name = @"StartSignal1",
				NewEntityChangedColumns = false,
				Position = new Point(73, 184),
				SerializeToDB = false,
				Size = new Size(27, 27),
				UseBackgroundMode = false,
				WaitingEntitySignal = true,
				WaitingRandomSignal = false
			};
			schemaStartSignalEvent.EntityChangedColumns.Add("a71adaea-3464-4dee-bb4f-c7a535450ad7");
			InitializeStartSignal1Parameters(schemaStartSignalEvent);
			return schemaStartSignalEvent;
		}

		protected virtual ProcessSchemaScriptTask CreateScriptTask1ScriptTask() {
			ProcessSchemaScriptTask ScriptTask = new ProcessSchemaScriptTask(this) {
				UId = new Guid("162d1904-7c40-4313-8c25-e5f44d73777a"),
				BackgroundModePriority = BackgroundModePriority.Inherited,
				ContainerItemIndex = 0,
				ContainerUId = new Guid("9368e760-06e3-4a5f-a5fb-ccb80b19301b"),
				CreatedInOwnerSchemaUId = new Guid("bb4d6607-026b-4b27-b640-8f5c77c1e89d"),
				CreatedInPackageId = new Guid("ff2cadfe-f4c2-06cd-930f-1401ae848c21"),
				CreatedInSchemaUId = new Guid("12ed463d-51aa-4d38-a7ff-929a9bc8beee"),
				EntitySchemaUId = Guid.Empty,
				FillColor = Color.FromArgb(-1),
				ImageList = null,
				ImageName = null,
				IsForCompensation = false,
				IsLogging = true,
				ManagerItemUId = new Guid("0e490dda-e140-4441-b600-6f5c64d024df"),
				ModifiedInSchemaUId = new Guid("12ed463d-51aa-4d38-a7ff-929a9bc8beee"),
				Name = @"ScriptTask1",
				Position = new Point(328, 170),
				SerializeToDB = false,
				Size = new Size(69, 55),
				UseBackgroundMode = false,
				UseFlowEngineScriptVersion = false,
				ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,157,145,177,110,131,48,16,134,231,240,20,14,67,100,47,150,154,21,101,73,10,40,67,91,169,180,15,112,53,151,4,9,142,202,28,169,80,213,119,175,29,104,74,72,166,172,246,253,223,125,254,157,182,69,46,54,208,224,43,154,218,230,219,92,172,68,142,59,104,75,150,169,187,83,81,80,236,132,204,24,44,103,197,158,160,124,208,231,209,249,74,248,25,29,87,159,220,41,241,29,204,38,164,155,177,40,248,153,50,151,247,49,151,23,204,35,88,129,196,5,119,153,57,96,5,79,64,176,71,235,34,239,13,218,77,77,132,134,139,154,116,124,61,20,93,165,93,236,6,76,167,200,91,106,24,200,224,186,123,134,10,101,232,253,66,215,83,143,29,82,254,112,130,208,27,139,192,216,143,201,75,167,161,230,255,172,78,144,205,33,177,117,245,184,150,227,6,148,88,44,196,60,113,164,214,98,115,242,105,98,130,143,18,115,175,82,154,182,116,75,222,208,86,47,228,115,253,186,80,169,192,85,73,248,117,250,108,127,253,55,235,182,15,143,155,58,233,51,110,36,230,76,103,35,205,12,142,40,149,175,223,162,51,34,193,182,197,232,23,116,120,149,233,86,2,0,0 }
			};
			
			return ScriptTask;
		}

		protected virtual ProcessSchemaStartSignalEvent CreateStartSignal2StartSignalEvent() {
			ProcessSchemaStartSignalEvent schemaStartSignalEvent = new ProcessSchemaStartSignalEvent(this) {	UId = new Guid("8b7c37ff-f3d2-4834-907b-952c0d815039"),
				BackgroundModePriority = BackgroundModePriority.Inherited,
				ContainerItemIndex = 0,
				ContainerUId = new Guid("9368e760-06e3-4a5f-a5fb-ccb80b19301b"),
				CreatedInOwnerSchemaUId = new Guid("bb4d6607-026b-4b27-b640-8f5c77c1e89d"),
				CreatedInPackageId = new Guid("ff2cadfe-f4c2-06cd-930f-1401ae848c21"),
				CreatedInSchemaUId = new Guid("12ed463d-51aa-4d38-a7ff-929a9bc8beee"),
				EntitySchemaUId = new Guid("117d32f9-8275-4534-8411-1c66115ce9cd"),
				EntitySignal = EntityChangeType.Inserted,
				HasEntityColumnChange = false,
				HasEntityFilters = true,
				ImageList = null,
				ImageName = null,
				IsInterrupting = false,
				IsLogging = true,
				ManagerItemUId = new Guid("1129e72f-0e8c-445a-b2ea-463517e86395"),
				ModifiedInSchemaUId = new Guid("12ed463d-51aa-4d38-a7ff-929a9bc8beee"),
				Name = @"StartSignal2",
				NewEntityChangedColumns = false,
				Position = new Point(73, 280),
				SerializeToDB = false,
				Size = new Size(27, 27),
				UseBackgroundMode = false,
				WaitingEntitySignal = true,
				WaitingRandomSignal = false
			};
			InitializeStartSignal2Parameters(schemaStartSignalEvent);
			return schemaStartSignalEvent;
		}

		protected override void InitializeMethods() {
			base.InitializeMethods();
			SetMethodsDefInheritance();
		}

		protected override void InitializeUsings() {
			Usings.Add(new SchemaUsing() {
				UId = new Guid("a5186d3c-ea57-4045-a2d2-e151dc44a44d"),
				Name = "Terrasoft.Configuration",
				Alias = "",
				CreatedInPackageId = new Guid("c4b72b51-b2ae-4127-a458-608f5464622c")
			});
			Usings.Add(new SchemaUsing() {
				UId = new Guid("bc8d9945-ef06-4931-a611-57ed410d5cec"),
				Name = "Terrasoft.Core.Factories",
				Alias = "",
				CreatedInPackageId = new Guid("c4b72b51-b2ae-4127-a458-608f5464622c")
			});
			Usings.Add(new SchemaUsing() {
				UId = new Guid("f939aa76-5111-42e9-9ef2-3cb58255e74a"),
				Name = "Terrasoft.Configuration.TermCalculationService",
				Alias = "",
				CreatedInPackageId = new Guid("c4b72b51-b2ae-4127-a458-608f5464622c")
			});
			Usings.Add(new SchemaUsing() {
				UId = new Guid("844f89bb-2c63-4486-a373-26af8150884b"),
				Name = "Terrasoft.Core",
				Alias = "",
				CreatedInPackageId = new Guid("c4b72b51-b2ae-4127-a458-608f5464622c")
			});
			Usings.Add(new SchemaUsing() {
				UId = new Guid("b014b92f-05c4-4e71-827a-e779ce1bcbc1"),
				Name = "System",
				Alias = "",
				CreatedInPackageId = new Guid("c4b72b51-b2ae-4127-a458-608f5464622c")
			});
			Usings.Add(new SchemaUsing() {
				UId = new Guid("cd749e26-028e-4427-9d4d-f4c5ee9c4de8"),
				Name = "System.Collections.Generic",
				Alias = "",
				CreatedInPackageId = new Guid("c4b72b51-b2ae-4127-a458-608f5464622c")
			});
			Usings.Add(new SchemaUsing() {
				UId = new Guid("57b0a18a-68e6-475c-b9d5-9ea901f77c8e"),
				Name = "Terrasoft.Configuration.Calendars",
				Alias = "",
				CreatedInPackageId = new Guid("c4b72b51-b2ae-4127-a458-608f5464622c")
			});
			Usings.Add(new SchemaUsing() {
				UId = new Guid("a2a90c1a-8dd0-408d-b519-29868612f07b"),
				Name = "Creatio.FeatureToggling",
				Alias = "",
				CreatedInPackageId = new Guid("c4b72b51-b2ae-4127-a458-608f5464622c")
			});
		}

		#endregion

		#region Methods: Public

		public override Process CreateProcess(UserConnection userConnection) {
			return new ActualizeCalculationTermsAfterStatusChange(userConnection);
		}

		public override object Clone() {
			return new ActualizeCalculationTermsAfterStatusChangeSchema(this);
		}

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("12ed463d-51aa-4d38-a7ff-929a9bc8beee"));
		}

		#endregion

	}

	#endregion

	#region Class: ActualizeCalculationTermsAfterStatusChange

	/// <exclude/>
	public class ActualizeCalculationTermsAfterStatusChange : Terrasoft.Core.Process.Process
	{

		#region Class: ProcessLane1

		/// <exclude/>
		public class ProcessLane1 : ProcessLane
		{

			public ProcessLane1(UserConnection userConnection, ActualizeCalculationTermsAfterStatusChange process)
				: base(userConnection) {
				Owner = process;
				IsUsedParentUserContexts = false;
			}

		}

		#endregion

		public ActualizeCalculationTermsAfterStatusChange(UserConnection userConnection)
			: base(userConnection) {
			InitializeMetaPathParameterValues();
			UId = Guid.NewGuid();
			Name = "ActualizeCalculationTermsAfterStatusChange";
			SchemaUId = new Guid("12ed463d-51aa-4d38-a7ff-929a9bc8beee");
			Caption = Schema.Caption;
			SchemaManagerName = "ProcessSchemaManager";
			SerializeToDB = false;
			SerializeToMemory = true;
			IsLogging = true;
			UseSystemSecurityContext = false;
			_notificationCaption = () => { return new LocalizableString((Caption)); };
			InitializeFlowElements();
		}

		#region Properties: Private

		private Guid InternalSchemaUId {
			get {
				return new Guid("12ed463d-51aa-4d38-a7ff-929a9bc8beee");
			}
		}

		private string ConditionalExpressionLogMessage {
			get {
				return "Process: ActualizeCalculationTermsAfterStatusChange, Source element: {0}, Conditional flow: {1}, Expression: {2}, Result: {3}";
			}
		}

		private string DeadEndGatewayLogMessage {
			get {
				return "Process: ActualizeCalculationTermsAfterStatusChange, Source element: {0}, None of the exclusive gateway conditions are met!";
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

		public virtual Guid CaseRecordId {
			get;
			set;
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
					SchemaElementUId = new Guid("e58a168a-5c48-45e1-81bd-524ca5198111"),
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
					SchemaElementUId = new Guid("1c01f230-4666-435f-8da3-235234c5161f"),
					CreatedInSchemaUId = InternalSchemaUId,
					ExecutedEventHandler = OnExecuted,
				});
			}
		}

		private ProcessScriptTask _scriptTask1;
		public ProcessScriptTask ScriptTask1 {
			get {
				return _scriptTask1 ?? (_scriptTask1 = new ProcessScriptTask() {
					UId = Guid.NewGuid(),
					Owner = this,
					Type = "ProcessSchemaScriptTask",
					Name = "ScriptTask1",
					SchemaElementUId = new Guid("162d1904-7c40-4313-8c25-e5f44d73777a"),
					CreatedInSchemaUId = InternalSchemaUId,
					ExecutedEventHandler = OnExecuted,
					IsLogging = true,
					Script = ScriptTask1Execute,
				});
			}
		}

		private ProcessStartSignalEvent _startSignal2;
		public ProcessStartSignalEvent StartSignal2 {
			get {
				return _startSignal2 ?? (_startSignal2 = new ProcessStartSignalEvent(UserConnection) {
					UId = Guid.NewGuid(),
					Owner = this,
					Type = "ProcessSchemaStartSignalEvent",
					Name = "StartSignal2",
					SerializeToDB = false,
					IsLogging = true,
					SchemaElementUId = new Guid("8b7c37ff-f3d2-4834-907b-952c0d815039"),
					CreatedInSchemaUId = InternalSchemaUId,
					ExecutedEventHandler = OnExecuted,
				});
			}
		}

		#endregion

		#region Methods: Private

		private void InitializeFlowElements() {
			FlowElements[TerminateEvent1.SchemaElementUId] = new Collection<ProcessFlowElement> { TerminateEvent1 };
			FlowElements[StartSignal1.SchemaElementUId] = new Collection<ProcessFlowElement> { StartSignal1 };
			FlowElements[ScriptTask1.SchemaElementUId] = new Collection<ProcessFlowElement> { ScriptTask1 };
			FlowElements[StartSignal2.SchemaElementUId] = new Collection<ProcessFlowElement> { StartSignal2 };
		}

		private void OnExecuted(object sender, ProcessActivityAfterEventArgs e) {
			switch (e.Context.SenderName) {
					case "TerminateEvent1":
						CompleteProcess();
						break;
					case "StartSignal1":
						e.Context.QueueTasksV2.Enqueue(new ProcessQueueElement("ScriptTask1", e.Context.SenderName));
						break;
					case "ScriptTask1":
						e.Context.QueueTasksV2.Enqueue(new ProcessQueueElement("TerminateEvent1", e.Context.SenderName));
						break;
					case "StartSignal2":
						e.Context.QueueTasksV2.Enqueue(new ProcessQueueElement("ScriptTask1", e.Context.SenderName));
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
			MetaPathParameterValues.Add("5ba3b2b0-c2db-44e1-bf19-e058ea310f85", () => CaseRecordId);
			MetaPathParameterValues.Add("36f4dbcc-6d57-4748-9968-0246e02c237a", () => StartSignal1.RecordId);
			MetaPathParameterValues.Add("39910edb-c399-4a13-885f-46917fb764cf", () => StartSignal2.RecordId);
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

		public virtual bool ScriptTask1Execute(ProcessExecutingContext context) {
			Guid CaseRecordId = default(Guid);
			if (StartSignal1.RecordId != Guid.Empty) {
				CaseRecordId = StartSignal1.RecordId;
			}
			if (StartSignal2.RecordId != Guid.Empty) {
				CaseRecordId = StartSignal2.RecordId;
			}
			var entitySchemaManager = UserConnection.EntitySchemaManager;
			var entitySchema = entitySchemaManager.GetInstanceByName("Case");
			Entity entityCase = entitySchema.CreateEntity(UserConnection);
			if (entityCase.FetchFromDB(CaseRecordId) && !Features.GetIsEnabled("CalculateTermOnCaseEntity"))
			{
				new CaseTermCalculationManager(UserConnection).Calculate(entityCase);
				entityCase.Save();
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
			var cloneItem = (ActualizeCalculationTermsAfterStatusChange)base.CloneShallow();
			cloneItem.ExecutedEventHandler = ExecutedEventHandler;
			return cloneItem;
		}

		#endregion

	}

	#endregion

}


namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: IGoogleAnalyticsAggregatedDataRepositorySchema

	/// <exclude/>
	public class IGoogleAnalyticsAggregatedDataRepositorySchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public IGoogleAnalyticsAggregatedDataRepositorySchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public IGoogleAnalyticsAggregatedDataRepositorySchema(IGoogleAnalyticsAggregatedDataRepositorySchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("26506298-1462-481a-bf71-9c6de30a617f");
			Name = "IGoogleAnalyticsAggregatedDataRepository";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("0fb3085b-d38c-4333-ad19-d589593a41f0");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,117,144,65,75,196,64,12,133,207,45,244,63,132,61,233,165,253,1,214,194,218,5,241,186,10,158,99,55,29,7,218,73,73,82,161,136,255,221,153,74,171,44,8,115,24,38,239,123,111,94,2,142,164,19,118,4,173,216,35,179,27,232,24,112,88,204,119,90,228,159,69,158,205,234,131,131,23,18,65,229,222,202,150,67,239,221,44,104,158,195,93,145,71,137,15,70,18,41,88,47,125,114,123,186,242,58,58,39,228,208,232,114,66,195,51,77,172,222,88,150,72,167,144,172,170,42,168,117,30,71,148,165,217,30,158,241,131,20,112,71,1,55,59,184,68,151,20,199,96,239,4,167,135,114,55,169,174,93,234,9,5,71,8,177,233,253,33,113,135,166,229,97,160,46,21,0,238,255,6,76,232,8,212,98,55,77,49,101,93,173,240,175,151,144,205,18,180,73,95,3,158,232,103,13,32,164,243,96,81,190,205,19,240,74,111,123,255,212,58,49,231,85,184,54,219,103,55,255,108,105,45,121,27,87,156,125,21,121,60,223,188,76,211,188,172,1,0,0 };
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("26506298-1462-481a-bf71-9c6de30a617f"));
		}

		#endregion

	}

	#endregion

}


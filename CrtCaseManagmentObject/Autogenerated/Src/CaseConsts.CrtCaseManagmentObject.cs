namespace Terrasoft.Configuration
{
	using System;
	public static class CaseConsts
	{
		//Category - Incident
		public static readonly Guid CaseCategoryIncidentId = new Guid("1B0BC159-150A-E111-A31B-00155D04C01D");
		
		//Category - Service Request
		public static readonly Guid CaseCategoryServiceCallId = new Guid("1C0BC159-150A-E111-A31B-00155D04C01D");
		
		//Category - Default case category
		public static readonly string DefaultCaseCategory = "DefaultCaseCategory";
		
		//Case origin default email id
		public static readonly Guid CaseOriginDefaultEmailId = new Guid("7F9E1F1E-F46B-1410-3492-0050BA5D6C38");
		
		//Incident status - Wait for response
		public static readonly Guid CaseStatusPendingId = new Guid("56022749-B807-48FF-876C-0B9133D90365");
		
		//Case status - New
		public static readonly Guid CaseStatusNewId = new Guid("AE5F2F10-F46B-1410-FD9A-0050BA5D6C38");

		//Case status - InProgress
		public static readonly Guid CaseStatusInProgressId = new Guid("7E9F1204-F46B-1410-FB9A-0050BA5D6C38");

		//Case status - Reopened
		public static readonly Guid CaseStatusReopenedId = new Guid("F063EBBE-FDC6-4982-8431-D8CFA52FEDCF");

		//Case status - Closed
		public static readonly Guid CaseStatusClosedId = new Guid("3E7F420C-F46B-1410-FC9A-0050BA5D6C38");

		//Case status - Cancel
		public static readonly Guid CaseStatusCancelId = new Guid("6E5F4218-F46B-1410-FE9A-0050BA5D6C38");

		//Case closure code - Canceled by user
		public static readonly Guid ClosureCodeCanceledByUserId = new Guid("068EA63F-F5DA-4D0E-B061-95A64AD120E7");
		
		//Case closure code - Closed by user
		public static readonly Guid ClosureCodeClosedByUserId = new Guid("7643E27F-51B2-4226-9001-E2A0AA415B3F");
		
		//Service pact type- SLA
		public static readonly Guid ServicePactTypeSLAId = new Guid("F6822F7F-0C38-4D98-87D2-9CFD25BDAA60");
		
		//Case schema UId
		public static readonly Guid CaseSchemaUId = new Guid("117D32F9-8275-4534-8411-1C66115CE9CD");
		
		//NotificationType - Reminding
		public static readonly Guid NotificationTypeRemindingId = new Guid("9EE66ABE-EC9D-4667-8995-29E8765DE2D5");
		
		//CustomerSatisfactionSurvey - Satisfaction email template
		public static readonly Guid SatisfactionSurveyTemplateId = new Guid("23579C9B-AD32-4520-8353-7BA3AACB53A9");

		//Confirmation of closing request - email template
		public static readonly Guid ConfirmationOfClosingRequestTemplateId = new Guid("291B7433-D6CA-43DA-B194-2590D86369B2");

		//EmailTemplate - Assignee email template
		public static readonly Guid AssigneeTemplateId = new Guid("B47E41C6-94D0-4D02-8531-4054C157C2AC");
		//EmailTemplate - Group email template
		public static readonly Guid GroupTemplateId = new Guid("0DC0759C-80B3-48B3-A832-7E32925D748B");
		
		public static readonly Guid DisableSendUsageType = new Guid("064224d7-130f-4e64-bff7-0eebb3f8e7e6");
		public static readonly Guid DelaySendUsageType = new Guid("d2598f06-2f81-44b6-970e-8ac1934cb60f");
		public static readonly Guid ImmediateSendUsageType = new Guid("e34695a7-7d65-45d8-b5a0-7b69ae52be6a");

		//NotificationType - Notification
		public static readonly Guid NotificationTypeNotificationId = new Guid("685E7149-C015-4A4D-B4A6-2E5625A6314C");
		
		public static readonly Guid ReceivedFromBlacklistedEmail = new Guid("5F0D0A0A-BFB2-4CA9-A6EE-438CE12AA05C");
		public static readonly Guid AutomaticEmailReply = new Guid("298B5A1A-18D7-4E8A-9F42-00C7C1AE1FBF");
		public static readonly Guid MarkedAsSpam = new Guid("2B74EA83-21F8-4F5D-8B92-FA2B8D823CBA");
		public static readonly string EmailAddressPattern = @"([\w\.\-]+)@([\w\-]+)((\.([-\w])+)+)";
		public static readonly string CaseCodeMaskCode = "CaseCodeMask";
		public static readonly string AllowedAutoSubmittedPropertyValue = "No";
		public static readonly string EmailBodyForCaseMaxLengthCode = "EmailBodyForCaseMaxLength";
	}
}

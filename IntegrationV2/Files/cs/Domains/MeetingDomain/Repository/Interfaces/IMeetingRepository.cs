namespace IntegrationV2.Files.cs.Domains.MeetingDomain.Repository.Interfaces
{
	using System;
	using System.Collections.Generic;
	using IntegrationV2.Files.cs.Domains.MeetingDomain.Model;

	#region Interface: IMeetingRepository

	/// <summary>
	/// Meeting repository interface.
	/// </summary>
	public interface IMeetingRepository
	{

		#region Methods: Public

		/// <summary>
		/// Gets list of <see cref="Meeting"/> models by <paramref name="meetingId"/>.
		/// </summary>
		/// <param name="activityId">Activity record identifier.</param>
		/// <returns><see cref="Meeting"/> instance collection.</returns>
		List<Meeting> GetMeetings(Guid meetingId);

		/// <summary>
		/// Gets internal calendar meetings for export.
		/// </summary>
		/// <param name="contactId">User contact identifier.</param>
		/// <param name="syncSinceDate">Since date.</param>
		/// <returns><see cref="Meeting"/> instance collection.</returns>
		List<Meeting> GetMeetings(Guid contactId, DateTime syncSinceDate);

		/// <summary>
		/// Gets internal calendar meetings for export.
		/// </summary>
		/// <param name="contactId">User contact identifier.</param>
		/// <param name="sinceDate">Since date.</param>
		/// <param name="dueDate">Due date.</param>
		/// <returns><see cref="Meeting"/> instance collection.</returns>
		List<Meeting> GetMeetings(Guid contactId, DateTime sinceDate, DateTime dueDate);

		/// <summary>
		/// Gets list of <see cref="Meeting"/> models by <paramref name="iCalUid"/>.
		/// </summary>
		/// <param name="iCalUid">External calendar record identifier.</param>
		/// <returns><see cref="Meeting"/> instance collection.</returns>
		List<Meeting> GetMeetings(string iCalUid);

		/// <summary>
		/// Gets list of deleted <see cref="Meeting"/> models by <paramref name="meetingId"/>.
		/// </summary>
		/// <param name="meetingId">Meeting unique identifier.</param>
		/// <returns>List of <see cref="Meeting"/> instances.</returns>
		List<Meeting> GetDeletedMeetings(Guid meetingId);

		/// <summary>
		/// Gets list of deleted <see cref="Meeting"/> models in <paramref name="contactId"/> calendar.
		/// <paramref name="sinceDate"/> to <paramref name="dueDate"/> period will be scanned for meetints that
		/// were deleted in external storage.
		/// </summary>
		/// <param name="contactId">Calendar owner identifier.</param>
		/// <param name="sinceDate">Period start date.</param>
		/// <param name="dueDate">Period end date.</param>
		/// <param name="existingRemoteMeetings">Existing metings identifiers.</param>
		/// <param name="createdBefore">Created before date filter value.</param>
		/// <returns>List of <see cref="Meeting"/> instances.</returns>
		List<Meeting> GetDeletedMeetings(Guid contactId, DateTime sinceDate, DateTime dueDate, IEnumerable<string> existingRemoteMeetings,
			DateTime createdBefore);

		/// <summary>
		/// Saves synchronization metadata.
		/// </summary>
		/// <param name="meeting"><see cref="Meeting"/> model instance.</param>
		void Save(Meeting meeting);

		/// <summary>
		/// Save activity metadata entity.
		/// </summary>
		/// <param name="meeting"><see cref="Meeting"/> instance.</param>
		void SaveMetadata(Meeting meeting);

		/// <summary>
		/// Sets invite response "Declined" for <paramref name="meeting"/> participants.
		/// </summary>
		/// <param name="meeting"><see cref="Meeting"/> instance.</param>
		void DeclineMeeting(Meeting meeting);

		/// <summary>
		/// Removes synchronization metadata.
		/// </summary>
		/// <param name="meeting"><see cref="Meeting"/> instance.</param>
		void Delete(Meeting meeting);

		/// <summary>
		/// Loads <paramref name="meeting"/> data from DB.
		/// </summary>
		/// <param name="meeting"><see cref="Meeting"/> instance.</param>
		void Load(Meeting meeting);

		#endregion

	}

	#endregion

}

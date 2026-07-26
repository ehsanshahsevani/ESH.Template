using Domain;
using FluentResults;

namespace DynamicFields.Abstraction;

public interface INotificationAnnouncementService
{
	Task<Result>
		SendNotificationForChangeStatusTo10Async(
			string categoryId, Domain.Profile profile, CancellationToken cancellationToken = default);

	Task<Result>
		SendNotificationForChangeStatusTo20Async(
			string categoryId,
			string needToEditReasonId,
			Domain.Profile profile, CancellationToken cancellationToken = default);

	Task<Result>
		SendNotificationForChangeStatusTo30Async(
			string categoryId, Domain.Profile profile, CancellationToken cancellationToken = default);
	Task<Result> SendNotificationForChangeStatusTo40Async(string categoryId, Profile profile, CancellationToken cancellationToken = default);
}
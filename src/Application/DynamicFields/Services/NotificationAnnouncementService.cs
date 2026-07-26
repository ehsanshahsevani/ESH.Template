using FluentResults;
using DynamicFields.Abstraction;
using ESH.BuildingBlocks.Localization.Abstraction;
using ESH.BuildingBlocks.NotificationCenter.Abstraction;
using ESH.BuildingBlocks.NotificationCenter.Enums;
using ESH.BuildingBlocks.SubSystem.Contract;
using ESH.SeedworkSystem.Domain.MultiLanguage;

namespace DynamicFields.Services;

public class NotificationAnnouncementService : INotificationAnnouncementService
{
	private ISubSystemManager SubSystemManager { get; }
	private ILanguageLocalizerManager LanguageLocalizerManager { get; }
	private INotificationHttpService NotificationService { get; }

	public NotificationAnnouncementService(
		INotificationHttpService notificationService,
		ISubSystemManager subSystemManager,
		ILanguageLocalizerManager languageLocalizerManager
		)
	{
		SubSystemManager = subSystemManager;
		NotificationService = notificationService;
		LanguageLocalizerManager = languageLocalizerManager;
	}

	public async Task<Result>
		SendNotificationForChangeStatusTo10Async(
			string categoryId, Domain.Profile profile, CancellationToken cancellationToken = default)
	{
		var result = new Result();

		var subSystem =
			await SubSystemManager
				.FindByNameAsync(domain: nameof(Domain.Category), cancellationToken);

		string language =
			ESH.Helpers.CurrentLanguage.Code();

		var languageLocalizer =
			await LanguageLocalizerManager
				.FindAsync(
					subSystemName: nameof(Domain.Category),
					relationId: categoryId,
					languageCode: language,
					Domain.Category.PropertyNameKey,
					cancellationToken);

		var dateTime = ESH.Utilities.DateTools.DateTimeNow();

		var parameters = new NotificationParameters(
			phoneNumber: profile.FullPhoneNumber,
			userId: profile.Id,
			serverId: Domain.Base.ServerKeyConstant.Key)
		{
			NotificationSection = NotificationSection.AnnouncementChangeToStatus10,

			Param1 = languageLocalizer!.Value,
			Param2 = dateTime.Date.ToString(format: "yyyy-MM-dd | HH:mm:ss"),
		};

		var resultNotification = await NotificationService
			.SendNotificationBySystemAsync(parameters);

		result.WithErrors(resultNotification!.Errors);

		return result;
	}

	public async Task<Result>
		SendNotificationForChangeStatusTo20Async(
			string categoryId,
			string needToEditReasonId,
			Domain.Profile profile, CancellationToken cancellationToken = default)
	{
		var result = new Result();

		var subSystem =
			await SubSystemManager
				.FindByNameAsync(domain: nameof(Domain.Category), cancellationToken);

		string language =
			ESH.Helpers.CurrentLanguage.Code();

		LanguageLocalizer? category =
			await LanguageLocalizerManager
				.FindAsync(
					subSystemName: nameof(Domain.Category),
					relationId: categoryId,
					languageCode: language,
					propertyName: Domain.Category.PropertyNameKey,
					cancellationToken);

		LanguageLocalizer? needToEditReason =
			await LanguageLocalizerManager
				.FindAsync(
					subSystemName: nameof(Domain.NeedToEditReason),
					relationId: needToEditReasonId,
					languageCode: language,
					propertyName: Domain.NeedToEditReason.TextPropertyLocalizer,
					cancellationToken);

		var dateTime = ESH.Utilities.DateTools.DateTimeNow();

		var parameters = new NotificationParameters(
			profile.FullPhoneNumber,
			profile.Id,
			serverId: Domain.Base.ServerKeyConstant.Key)
		{
			NotificationSection = NotificationSection.AnnouncementChangeToStatus20,

			Param1 = category!.Value,
			Param2 = needToEditReason!.Value,
			Param3 = dateTime.Date.ToString(format: "yyyy-MM-dd | HH:mm:ss"),
		};

		var resultNotification =
			await NotificationService
				.SendNotificationBySystemAsync(parameters);

		result.WithErrors(resultNotification!.Errors);

		return result;
	}

	public async Task<Result>
		SendNotificationForChangeStatusTo30Async(
			string categoryId, Domain.Profile profile, CancellationToken cancellationToken = default)
	{
		var result = new Result();

		var subSystem =
			await SubSystemManager
				.FindByNameAsync(domain: nameof(Domain.Category), cancellationToken);

		string language =
			ESH.Helpers.CurrentLanguage.Code();

		var languageLocalizer =
			await LanguageLocalizerManager
				.FindAsync(
					nameof(Domain.Category),
					categoryId,
					language,
					Domain.Category.PropertyNameKey,
					cancellationToken);

		var dateTime = ESH.Utilities.DateTools.DateTimeNow();

		var parameters = new NotificationParameters(
			profile.FullPhoneNumber,
			profile.Id,
			serverId: Domain.Base.ServerKeyConstant.Key)
		{
			NotificationSection = NotificationSection.AnnouncementChangeToStatus30,

			Param1 = languageLocalizer!.Value,
			Param2 = dateTime.Date.ToString(format: "yyyy-MM-dd | HH:mm:ss"),
		};

		var resultNotification = await NotificationService
			.SendNotificationBySystemAsync(parameters);

		result.WithErrors(resultNotification!.Errors);

		return result;
	}

	public async Task<Result>
		SendNotificationForChangeStatusTo40Async(
			string categoryId, Domain.Profile profile, CancellationToken cancellationToken = default)
	{
		var result = new Result();

		var subSystem =
			await SubSystemManager
				.FindByNameAsync(domain: nameof(Domain.Category), cancellationToken);

		string language =
			ESH.Helpers.CurrentLanguage.Code();

		var languageLocalizer =
			await LanguageLocalizerManager
				.FindAsync(
					nameof(Domain.Category),
					categoryId,
					language,
					Domain.Category.PropertyNameKey,
					cancellationToken);

		var dateTime = ESH.Utilities.DateTools.DateTimeNow();

		var parameters = new NotificationParameters(
			profile.FullPhoneNumber,
			profile.Id,
			serverId: Domain.Base.ServerKeyConstant.Key)
		{
			NotificationSection = NotificationSection.AnnouncementChangeToStatus40,

			Param1 = languageLocalizer!.Value,
			Param2 = dateTime.Date.ToString(format: "yyyy-MM-dd | HH:mm:ss"),
		};

		var resultNotification = await NotificationService
			.SendNotificationBySystemAsync(parameters);

		result.WithErrors(resultNotification!.Errors);

		return result;
	}

}
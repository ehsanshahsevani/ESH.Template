using Domain;
using ESH.SeedworkSystem.Persistence;


using ESH.BuildingBlocks.RequestFeatures;
using ESH.Utilities;
using ESH.ViewModels.Announcement;
using ESH.ViewModels.Announcement.ModelParameters;

namespace Persistence.Abstracts;

public interface IProfileRepository : IRepository<Profile>
{
	/// <summary>
	/// Finds and retrieves a list of UiSelectModel objects based on the provided list of profile IDs.
	/// </summary>
	/// <param name="ids">A list of profile IDs to search for.</param>
	/// <param name="cancellationToken"></param>
	/// <returns>A task representing the asynchronous operation, containing a list of UiSelectModel objects corresponding to the specified IDs.</returns>
	Task<List<UiSelectModel>> FindByIdsAsync(List<string> ids, CancellationToken cancellationToken = default);

	/// <summary>
	/// Checks whether a ReasonRegisterInSystem with the specified ID exists in the database.
	/// </summary>
	/// <param name="reasonRegisterInSystemId">The unique identifier of the ReasonRegisterInSystem to check for existence.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a boolean indicating whether the specified ReasonRegisterInSystem exists.</returns>
	Task<bool> IsReasonRegisterInSystemExistAsync(string reasonRegisterInSystemId);

	/// <summary>
	/// Returns paged list of profiles for admin purposes (includes inactive).
	/// </summary>
	/// <param name="parameters">Paging and filtering parameters.</param>
	/// <param name="cancellationToken"></param>
	/// <returns>Paged list of profiles (admin view).</returns>
	Task<PagedList<Profile>> GetAllInPageForAdminAsync(ProfileParameters parameters, CancellationToken cancellationToken = default);

	/// <summary>
	/// Find a profile by id ignoring the IsActive filter (admin use).
	/// </summary>
	/// <param name="id">Profile id</param>
	/// <param name="cancellationToken"></param>
	/// <returns>Profile or null</returns>
	Task<Profile?> FindByIdForAdminAsync(string id, CancellationToken cancellationToken = default);
	Task<UserBoxViewModel> GetUserBoxAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// get count all profiles
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<int> CountAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// get language code by user id / profile id
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<string?> FindLanguageCodeAsync(string id, CancellationToken cancellationToken = default);

	Task<List<Profile>> GetAllShowProfileInAnnouncementByIdsAsync(
		List<string> profileIds, CancellationToken cancellationToken = default);

	/// <summary>
	/// حذف دیتای این جدول برای یک کاربر در هنگام حذف حساب کاربری
	/// </summary>
	/// <param name="profileId"></param>
	/// <param name="cancellationToken"></param>
	Task DeleteAccountAsync(
		string profileId,
		CancellationToken cancellationToken = default);
}
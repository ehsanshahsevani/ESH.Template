using Domain;

using Persistence.Abstracts;
using Microsoft.EntityFrameworkCore;

using ESH.BuildingBlocks.RequestFeatures;
using ESH.Utilities;
using ESH.ViewModels.Announcement;
using ESH.ViewModels.Announcement.ModelParameters;
 

namespace Persistence.Repositories;

public class ProfileRepository
    : Repository<Profile>, IProfileRepository
{
    internal ProfileRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
    }

    /// <summary>
    /// Retrieves a single Profile entity based on the provided ID, ensuring that the profile is active and not marked as deleted.
    /// </summary>
    /// <param name="id">The identifier of the Profile entity to find.</param>
    /// <param name="isActive"></param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation, containing the Profile entity if found; otherwise, null.</returns>
    public override async Task<Profile?> FindAsync(object id, bool? isActive = true,  CancellationToken cancellationToken = default)
    {
        var result = await DbSet
            .Include(current => current.LanguageCode)
            .Where(current => current.IsDeleted == false)
            .Where(current => isActive.HasValue == false || current.IsActive == isActive.Value)
            .Where(current => current.Id == id.ToString())
            .FirstOrDefaultAsync(cancellationToken);

        return result;
    }

    /// <summary>
    /// Finds and retrieves a list of UiSelectModel objects based on the provided list of profile IDs.
    /// </summary>
    /// <param name="ids">A list of profile IDs to search for.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>A task representing the asynchronous operation, containing a list of UiSelectModel objects corresponding to the specified IDs.</returns>
    public async Task<List<UiSelectModel>> FindByIdsAsync(List<string> ids,
        CancellationToken cancellationToken = default)
    {
        var result = await DbSet
            .Where(current => current.IsDeleted == false)
            .Where(current => current.IsActive == true)
            .Where(current => ids.Contains(current.Id))
            .Select(current => new UiSelectModel(current.DisplayName, current.Id))
            .ToListAsync(cancellationToken);

        return result;
    }

    /// <summary>
    /// Checks whether a ReasonRegisterInSystem with the specified ID exists in the database.
    /// </summary>
    /// <param name="reasonRegisterInSystemId">The unique identifier of the ReasonRegisterInSystem to check for existence.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a boolean indicating whether the specified ReasonRegisterInSystem exists.</returns>
    public async Task<bool> IsReasonRegisterInSystemExistAsync(string reasonRegisterInSystemId)
    {
        var result = await DbSet.AnyAsync(current => current.Id == reasonRegisterInSystemId);
        return result;
    }

    /// <summary>
    /// Admin paged listing - does not filter out inactive profiles.
    /// Supports search across FirstName, LastName, PhoneNumber, UserId, City.Name and Province.Name using Contains for readability.
    /// </summary>
    public async Task<PagedList<Profile>> GetAllInPageForAdminAsync(ProfileParameters parameters,
        CancellationToken cancellationToken = default)
    {
        // include City and Province to allow searching by their names and to avoid extra queries when mapping
        var query = DbSet
            // .Include(p => p.City)
            // .ThenInclude(c => c.Province)
            .Where(current => current.IsDeleted == false)
            .AsQueryable();

        if (string.IsNullOrEmpty(parameters.Text) == false)
        {
            var search = parameters.Text.Trim();

            query = query.Where(current =>
                (string.IsNullOrEmpty(current.DisplayName) == false && current.DisplayName.Contains(search))
                || (current.FullPhoneNumber != null && current.FullPhoneNumber.Contains(search))
                || (current.UserId != null && current.UserId.Contains(search))
            );
        }

        var ordered = query.OrderByDescending(c => c.UpdateDateTime);

        var paged = await PagedList<Profile>.ToPagedList(ordered, parameters, cancellationToken);

        return paged;
    }

    /// <summary>
    /// Find by id for admin (ignores IsActive).
    /// </summary>
    public async Task<Profile?> FindByIdForAdminAsync(string id, CancellationToken cancellationToken = default)
    {
        var result = await DbSet
            .Where(current => current.IsDeleted == false)
            .Where(current => current.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        return result;
    }

    /// <summary>
    /// Returns basic statistics for users/profiles (count / active / deactive).
    /// </summary>
    public async Task<UserBoxViewModel> GetUserBoxAsync(CancellationToken cancellationToken = default)
    {
        var source = DbSet.Where(p => p.IsDeleted == false);

        var count = await source.CountAsync(cancellationToken);
        var active = await source.Where(p => p.IsActive == true).CountAsync(cancellationToken);
        var deActive = count - active;

        var box = new UserBoxViewModel
        {
            Count = count,
            Active = active,
            Suspended = deActive
        };

        return box;
    }

    /// <summary>
    /// get count all profiles
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        var result = await DbSet
            .Where(current => current.IsActive == true)
            .Where(current => current.IsDeleted == false)
            .CountAsync(cancellationToken);

        return result;
    }

    /// <summary>
    /// get language code by user id / profile id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string?> FindLanguageCodeAsync(
        string id, CancellationToken cancellationToken = default)
    {
        var result = await DbSet
            .Include(current => current.LanguageCode)
            .Where(current => current.IsDeleted == false)
            .Where(current => current.IsActive == true)
            .Where(current => current.Id == id)
            .Select(current => current.LanguageCode!.Code)
            .FirstOrDefaultAsync(cancellationToken);

        return result;
    }

    public async Task<List<Profile>> GetAllShowProfileInAnnouncementByIdsAsync(
        List<string> profileIds, CancellationToken cancellationToken = default)
    {
        var result = await DbSet
            .Where(current => current.IsDeleted == false)
            .Where(current => current.IsActive == true)
            .Where(current => profileIds.Contains(current.Id))
            .Where(current => current.ShowProfileInAnnouncement == true)
            .ToListAsync(cancellationToken);

        return result;
    }

    #region DeleteAccountAsync(string profileId)

    /// <summary>
    /// حذف دیتای این جدول برای یک کاربر در هنگام حذف حساب کاربری
    /// </summary>
    /// <param name="profileId"></param>
    /// <param name="cancellationToken"></param>
    public async Task DeleteAccountAsync(
        string profileId,
        CancellationToken cancellationToken = default)
    {
        await DatabaseContext.Attachments
            .Where(x => x.SubSystem.Name == nameof(Domain.Profile))
            .Where(x => x.RelationId == profileId)
            .ExecuteDeleteAsync(cancellationToken);

        await DbSet
            .Where(x => x.Id == profileId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    #endregion /DeleteAccountAsync(string profileId)
}
using Domain;
using Persistence.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class FieldValueAnnouncementRepository : Repository<FieldValueAnnouncement>, IFieldValueAnnouncementRepository
{
	internal FieldValueAnnouncementRepository(DatabaseContext databaseContext) : base(databaseContext)
	{
	}

	public async Task<List<FieldValueAnnouncement>> GetByAnnouncementIdAsync(
		string announcementId,
		CancellationToken cancellationToken = default)
	{
		var result = await DbSet
			.Where(current => current.IsDeleted == false)
			.Where(current => current.AnnouncementId == announcementId)
			.ToListAsync(cancellationToken);

		return result;
	}

	public async Task RemoveByAnnouncementIdAsync(
		string announcementId,
		CancellationToken cancellationToken = default)
	{
		var result = await DbSet

			.Where(current => current.AnnouncementId == announcementId)

			.ToListAsync(cancellationToken);

		await base.RemoveRangeAsync(result);
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
		await DbSet
			.Where(x => x.Announcement!.ProfileId == profileId)
			.ExecuteDeleteAsync(cancellationToken);
	}
	
	#endregion /DeleteAccountAsync(string profileId)
}

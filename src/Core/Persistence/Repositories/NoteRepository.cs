using Domain;
using Persistence.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class NoteRepository : Repository<Note>, INoteRepository
{
	internal NoteRepository(DatabaseContext databaseContext) : base(databaseContext)
	{
	}

	/// <summary>
	/// دریافت تمام یادداشت های یک آگهی
	/// </summary>
	/// <param name="announcementId">شناسه آگهی</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<Note?> GetByAnnouncementIdAsync(string announcementId, CancellationToken cancellationToken = default)
	{
		var result = await DbSet
			.Where(x => x.IsDeleted == false)
			.Where(x => x.AnnouncementId == announcementId)
			.FirstOrDefaultAsync(cancellationToken);

		return result;
	}

	/// <summary>
	/// دریافت تمام یادداشت های یک کاربر
	/// </summary>
	/// <param name="profileId">شناسه کاربر</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<List<Note>> GetByProfileIdAsync(string profileId, CancellationToken cancellationToken = default)
	{
		var result = await DbSet
			.Where(x => x.IsDeleted == false)
			.Where(x => x.ProfileId == profileId)
			.ToListAsync(cancellationToken);

		return result;
	}

	/// <summary>
	/// بررسی وجود یادداشت برای یک آگهی توسط یک کاربر
	/// </summary>
	/// <param name="announcementId">شناسه آگهی</param>
	/// <param name="profileId">شناسه کاربر</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<bool> ExistsAsync(string announcementId, string profileId, CancellationToken cancellationToken = default)
	{
		var result = await DbSet
			.Where(x => x.IsDeleted == false)
			.Where(x => x.AnnouncementId == announcementId)
			.Where(x => x.ProfileId == profileId)
			.AnyAsync(cancellationToken);

		return result;
	}

	/// <summary>
	/// دریافت یادداشت توسط آگهی و کاربر
	/// </summary>
	/// <param name="announcementId">شناسه آگهی</param>
	/// <param name="profileId">شناسه کاربر</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<List<Note>> GetByAnnouncementAndProfileAsync(
		string announcementId, string profileId, CancellationToken cancellationToken = default)
	{
		var result = await DbSet
			.Where(x => x.IsDeleted == false)
			.Where(x => x.AnnouncementId == announcementId)
			.Where(x => x.ProfileId == profileId)
			.ToListAsync(cancellationToken);

		return result;
	}

	public async Task<List<Note>> FindByAnnouncementAsync(
		string announcementId, string userId, CancellationToken cancellationToken = default)
	{
		var result = await DbSet

			.Where(current => current.IsDeleted == false)

			.Where(current => current.ProfileId == userId)

			.Where(current => current.AnnouncementId == announcementId)

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
		await DbSet
			.Where(x => x.Announcement.ProfileId == profileId || x.ProfileId == profileId)
			.ExecuteDeleteAsync(cancellationToken);
	}
	
	#endregion /DeleteAccountAsync(string profileId)
}

using Domain;
using ESH.SeedworkSystem.Persistence;

namespace Persistence.Abstracts;

// For FeildValueAnnouncement
public interface IFieldValueAnnouncementRepository : IRepository<FieldValueAnnouncement>
{
	Task<List<FieldValueAnnouncement>> GetByAnnouncementIdAsync(
		string announcementId, CancellationToken cancellationToken = default);

	Task RemoveByAnnouncementIdAsync(
		string announcementId, CancellationToken cancellationToken = default);

	/// <summary>
	/// حذف دیتای این جدول برای یک کاربر در هنگام حذف حساب کاربری
	/// </summary>
	/// <param name="profileId"></param>
	/// <param name="cancellationToken"></param>
	Task DeleteAccountAsync(
		string profileId,
		CancellationToken cancellationToken = default);
}

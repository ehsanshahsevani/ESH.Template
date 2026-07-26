using Domain;
using ESH.SeedworkSystem.Persistence;

namespace Persistence.Abstracts;

// For NeedToEditLog
public interface INeedToEditLogRepository : IRepository<NeedToEditLog>
{
    /// <summary>
    /// حذف دیتای این جدول برای یک کاربر در هنگام حذف حساب کاربری
    /// </summary>
    /// <param name="profileId"></param>
    /// <param name="cancellationToken"></param>
    Task DeleteAccountAsync(
        string profileId,
        CancellationToken cancellationToken = default);
}

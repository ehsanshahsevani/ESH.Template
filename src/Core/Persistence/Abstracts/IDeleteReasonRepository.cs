
using Domain;
using ESH.SeedworkSystem.Persistence;

using ESH.BuildingBlocks.RequestFeatures;
using ESH.Utilities;
using ESH.ViewModels.Announcement.ModelParameters;
 

namespace Persistence.Abstracts;

// For DeleteReason
public interface IDeleteReasonRepository : IRepository<DeleteReason>
{
	Task<DeleteReason?> FindAdminAsync(string id, CancellationToken cancellationToken = default);
	Task<DeleteReason?> FindByCodeAsync(int code, CancellationToken cancellationToken = default);
	Task<PagedList<DeleteReason>> GetAllWithPageAsync(DeleteReasonParameters parameter, CancellationToken cancellationToken = default);
	Task<List<UiSelectModel>> GetUiSelectAsync(CancellationToken cancellationToken = default);
}

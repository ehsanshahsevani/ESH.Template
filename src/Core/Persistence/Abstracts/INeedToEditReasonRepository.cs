using Domain;

using ESH.SeedworkSystem.Persistence;
using ESH.ViewModels.Announcement.ModelParameters;

using ESH.BuildingBlocks.RequestFeatures;
using ESH.Utilities;


namespace Persistence.Abstracts;

// For NeedToEditReason
public interface INeedToEditReasonRepository : IRepository<NeedToEditReason>
{
	Task<NeedToEditReason?> FindAdminAsync(string id, CancellationToken cancellationToken = default);
	Task<NeedToEditReason?> FindByCodeAsync(int code, CancellationToken cancellationToken = default);
	Task<List<NeedToEditReason>> GetAllAdminAsync(CancellationToken cancellationToken = default);
	Task<PagedList<NeedToEditReason>> GetAllWithPageAsync(NeedToEditReasonParameters parameter, CancellationToken cancellationToken = default);
	Task<List<UiSelectModel>> GetUiSelectsAsync(CancellationToken cancellationToken = default);
}

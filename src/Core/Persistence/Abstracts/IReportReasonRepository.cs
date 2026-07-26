

using Domain;
using ESH.BuildingBlocks.RequestFeatures;
using ESH.SeedworkSystem.Persistence;
using ESH.Utilities;
using ESH.ViewModels.Announcement.ModelParameters;


namespace Persistence.Abstracts;

// For ReportReason
public interface IReportReasonRepository : IRepository<ReportReason>
{
	Task<ReportReason?> FindAdminAsync(string id, CancellationToken cancellationToken = default);
	Task<ReportReason?> FindByCodeAsync(int code, CancellationToken cancellationToken = default);
	Task<PagedList<ReportReason>> GetAllWithPageAsync(ReportReasonParameters parameter, CancellationToken cancellationToken = default);
	Task<List<UiSelectModel>> GetUiSelectAsync(CancellationToken cancellationToken = default);
}

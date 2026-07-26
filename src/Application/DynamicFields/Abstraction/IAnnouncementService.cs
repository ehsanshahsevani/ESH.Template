using FluentResults;
using ESH.ViewModels.Shared;
using ESH.ViewModels.Announcement;
using ESH.ViewModels.Announcement.MapApp;
using ESH.ViewModels.Announcement.ModelParameters;

namespace DynamicFields.Abstraction;

public interface IAnnouncementService
{
	Task<Result<AnnouncementResponseViewModel>> CreateAsync(
		AnnouncementRequestViewModel model,
		CancellationToken cancellationToken = default);

	Task<Result<AnnouncementResponseViewModel>>
		GetByIdAsync(string id, string? profileId, bool isAdmin = false, CancellationToken cancellationToken = default);

	Task<Result<PagedListResult<AnnouncementMiniResponseViewModel>>> GetAllWithPageAsync(
		AnnouncementParameters parameters,
		CancellationToken cancellationToken = default);

	Task<Result<bool>> ChangeIsHiddenAsync(string id, string userId, CancellationToken cancellationToken = default);

	Task<Result<List<AnnouncementMiniResponseViewModel>>> GetMiniModelsByIdsAsync(
		List<string> announcementIds,
		CancellationToken cancellationToken = default);

	Task<Result<PagedListResult<AnnouncementMiniResponseViewModel>>> GetAnnouncementsWithNotesPopulatedAsync(
		string profileId,
		AnnouncementParameters parameters,
		CancellationToken cancellationToken = default);

	Task<Result<bool>> ChangeIsActiveAsync(string id, CancellationToken cancellationToken = default);
	Task<Result> DeleteAsync(
		DeleteLogRequestViewModel model,
		string profileId, CancellationToken cancellationToken = default);

	Task<Result> AcceptForPublishAsync(string? id, string profileId, CancellationToken cancellationToken = default);

	Task<Result> ChangeStatusNeedToEditAsync(
		NeetToEditLogRequestViewModel model,
		string userId, CancellationToken cancellationToken = default);
	Task<Result> UpdateAsync(AnnouncementUpdateRequestViewModel model, string userId, CancellationToken cancellationToken = default);
	Task<Result> ChangeStatusToRejectedAsync(string id, string userId, CancellationToken cancellationToken = default);
	Task<Result<AdminDashboardStatsViewModel>> GetAdminDashboardStatsAsync(string? statusId, CancellationToken cancellationToken = default);
	Task<Result<List<ChartDataViewModel>>> GetChartDataForStatusAsync(CancellationToken cancellationToken = default);
	Task<Result<List<MapCluster>>> GetClustersAsync(
		AnnouncementParameters parameters,
		CancellationToken cancellationToken = default);

	Task<Result<List<AnnouncementMiniResponseViewModel>>> ResentVisitAsync(
		AnnouncementParameters parameters,
		CancellationToken cancellationToken = default);

	Task<Result<List<AnnouncementMiniResponseViewModel>>> GetAllInListAsync(
		AnnouncementParameters parameters,
		CancellationToken cancellationToken = default);
}
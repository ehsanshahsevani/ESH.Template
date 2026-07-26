using Domain;

using Persistence.Abstracts;
using Microsoft.EntityFrameworkCore;
using ESH.ViewModels.Announcement.ModelParameters;

using ESH.BuildingBlocks.RequestFeatures;
using ESH.Utilities;


namespace Persistence.Repositories;

public class ReportReasonRepository : Repository<ReportReason>, IReportReasonRepository
{
	internal ReportReasonRepository(DatabaseContext databaseContext) : base(databaseContext)
	{
	}

	public override async Task<IEnumerable<ReportReason?>>
		GetAllAsync(CancellationToken cancellationToken = default)
	{
		var result = await DbSet
			.Where(current => current.IsDeleted == false)
			.Where(current => current.IsActive == true)

			.OrderBy(current => current.Ordering)
			.ThenByDescending(current => current.CreateDateTime)

			.ToListAsync(cancellationToken);

		return result;
	}

	public async Task<ReportReason?> FindByCodeAsync(
		int code,
		CancellationToken cancellationToken = default)
	{
		var result = await DbSet

			.Where(current => current.IsDeleted == false)
			.Where(current => current.Code == code)
			.FirstOrDefaultAsync(cancellationToken);

		return result;
	}

	public async Task<PagedList<ReportReason>> GetAllWithPageAsync(
		ReportReasonParameters parameter, CancellationToken cancellationToken = default)
	{
		var source = DbSet

			.Where(current => current.IsDeleted == false)

			.AsQueryable();

		if (string.IsNullOrEmpty(parameter.Text) == false)
		{
			var valueLocalizersIds =
				await DatabaseContext.LanguageLocalizers
					.Where(current => current.IsDeleted == false)
					.Where(current => current.SubSystem.Name == nameof(Domain.ReportReason))
					.Where(current => current.Value.Contains(parameter.Text))

					.Select(current => current.Id)

					.ToListAsync(cancellationToken);

			source = source.Where(current => valueLocalizersIds.Contains(current.Id));
		}

		source = source
			.OrderBy(current => current.Ordering)
			.ThenByDescending(current => current.CreateDateTime);

		var result = await PagedList<ReportReason>
			.ToPagedList(source, parameter, cancellationToken);

		return result;
	}

	public async Task<ReportReason?> FindAdminAsync(
	string id, CancellationToken cancellationToken = default)
	{
		var result = await DbSet

			.Where(current => current.IsDeleted == false)

			.Where(current => current.Id == id)

			.FirstOrDefaultAsync(cancellationToken);

		return result;
	}

	public async Task<List<UiSelectModel>>
		GetUiSelectAsync(CancellationToken cancellationToken = default)
	{
		var result = await DbSet
			.Where(current => current.IsDeleted == false)

			.Select(current => new  UiSelectModel(string.Empty, current.Id))

			.ToListAsync(cancellationToken);

		return result;
	}
}

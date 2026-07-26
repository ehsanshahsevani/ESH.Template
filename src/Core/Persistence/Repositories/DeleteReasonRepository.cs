

using Domain;
using ESH.BuildingBlocks.RequestFeatures;
using ESH.Utilities;
using ESH.ViewModels.Announcement.ModelParameters;
using Microsoft.EntityFrameworkCore;
using Persistence.Abstracts;
 

namespace Persistence.Repositories;

public class DeleteReasonRepository : Repository<DeleteReason>, IDeleteReasonRepository
{
	internal DeleteReasonRepository(DatabaseContext databaseContext) : base(databaseContext)
	{
	}

	public override async Task<IEnumerable<DeleteReason?>>
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

	public async Task<DeleteReason?> FindByCodeAsync(
		int code,
		CancellationToken cancellationToken = default)
	{
		var result = await DbSet

			.Where(current => current.IsDeleted == false)
			.Where(current => current.Code == code)
			.FirstOrDefaultAsync(cancellationToken);

		return result;
	}

	public async Task<PagedList<DeleteReason>> GetAllWithPageAsync(
		DeleteReasonParameters parameter, CancellationToken cancellationToken = default)
	{
		var source = DbSet

			.Where(current => current.IsDeleted == false)

			.AsQueryable();

		if (string.IsNullOrEmpty(parameter.Text) == false)
		{
			var valueLocalizersIds =
				await DatabaseContext.LanguageLocalizers
					.Where(current => current.IsDeleted == false)
					.Where(current => current.SubSystem.Name == nameof(DeleteReason))
					.Where(current => current.Value.Contains(parameter.Text))

					.Select(current => current.Id)

					.ToListAsync(cancellationToken);

			source = source.Where(current => valueLocalizersIds.Contains(current.Id));
		}

		source = source
			.OrderBy(current => current.Ordering)
			.ThenByDescending(current => current.CreateDateTime);

		var result = await PagedList<DeleteReason>
			.ToPagedList(source, parameter, cancellationToken);

		return result;
	}

	public async Task<DeleteReason?> FindAdminAsync(
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

			.Select(current => new UiSelectModel(string.Empty, current.Id))

			.ToListAsync(cancellationToken);

		return result;
	}
}

using Domain;

using Persistence.Abstracts;
using Microsoft.EntityFrameworkCore;
using ESH.ViewModels.Announcement.ModelParameters;

using ESH.BuildingBlocks.RequestFeatures;
using ESH.Utilities;


namespace Persistence.Repositories;

public class NeedToEditReasonRepository : Repository<NeedToEditReason>, INeedToEditReasonRepository
{
	internal NeedToEditReasonRepository(DatabaseContext databaseContext) : base(databaseContext)
	{
	}

	public override async Task<IEnumerable<NeedToEditReason?>>
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

	public async Task<List<NeedToEditReason>>
		GetAllAdminAsync(CancellationToken cancellationToken = default)
	{
		var result = await DbSet
			.Where(current => current.IsDeleted == false)

			.OrderBy(current => current.Ordering)
			.ThenByDescending(current => current.CreateDateTime)

			.ToListAsync(cancellationToken);

		return result;
	}

	public async Task<NeedToEditReason?> FindByCodeAsync(
		int code,
		CancellationToken cancellationToken = default)
	{
		var result = await DbSet

			.Where(current => current.IsDeleted == false)
			.Where(current => current.IsActive == true)
			.Where(current => current.Code == code)

			.FirstOrDefaultAsync(cancellationToken);

		return result;
	}

	public async Task<PagedList<NeedToEditReason>> GetAllWithPageAsync(
		NeedToEditReasonParameters parameter, CancellationToken cancellationToken = default)
	{
		var source = DbSet

			.Where(current => current.IsDeleted == false)

			.AsQueryable();

		if (string.IsNullOrEmpty(parameter.Text) == false)
		{
			var valueLocalizersIds =
				await DatabaseContext.LanguageLocalizers
					.Where(current => current.IsDeleted == false)
					.Where(current => current.SubSystem.Name == nameof(NeedToEditReason))
					.Where(current => current.Value.Contains(parameter.Text))

					.Select(current => current.Id)

					.ToListAsync(cancellationToken);

			source = source.Where(current => valueLocalizersIds.Contains(current.Id));
		}

		source = source
			.OrderBy(current => current.Ordering)
			.ThenByDescending(current => current.CreateDateTime);

		var result = await PagedList<NeedToEditReason>
			.ToPagedList(source, parameter, cancellationToken);

		return result;
	}

	public async Task<List<UiSelectModel>> GetUiSelectsAsync(CancellationToken cancellationToken = default)
	{
		var result = await DbSet

			.Where(current => current.IsDeleted == false)

			.Select(current => new UiSelectModel("", current.Id))

			.ToListAsync(cancellationToken);

		return result;
	}

	public async Task<NeedToEditReason?> FindAdminAsync(
		string id, CancellationToken cancellationToken = default)
	{
		var result = await DbSet
			
			.Where(current => current.IsDeleted == false)

			.Where(current => current.Id == id)

			.FirstOrDefaultAsync(cancellationToken);

		return result;
	}
}

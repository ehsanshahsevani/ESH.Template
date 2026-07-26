
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence.Abstracts;

using ESH.BuildingBlocks.RequestFeatures;
using ESH.Utilities;
using ESH.ViewModels.Announcement.ModelParameters;
 

namespace Persistence.Repositories;

public class DictionaryCheckerRepository
	: Repository<DictionaryChecker>, IDictionaryCheckerRepository
{
	internal DictionaryCheckerRepository(DatabaseContext databaseContext) : base(databaseContext)
	{
	}

	public override async Task<DictionaryChecker?>
		FindAsync(object id, bool? isActive = true,  CancellationToken cancellationToken = default)
	{
		var result = await DbSet

			.Where (x => x.IsDeleted == false)
			.Where(current => current.Id == id.ToString())

			.FirstOrDefaultAsync(cancellationToken);

		return result;
	}

	public async Task<PagedList<DictionaryChecker>> GetAllWithPageAsync(
		DictionaryCheckerParameters parameters,
		CancellationToken cancellationToken = default)
	{
		var source = DbSet.AsQueryable();

		source = source.Where(current => current.IsDeleted == false);

		if (parameters.IsActive.HasValue == true)
		{
			source = source.Where(current => current.IsActive == parameters.IsActive.Value);
		}

		if (string.IsNullOrEmpty(parameters.Text) == false)
		{
			source = source.Where(current => current.Text.Contains(parameters.Text));
		}

		source = source
			.OrderBy(current => current.Ordering)
			.ThenByDescending(current => current.CreateDateTime);

		var result =
			await PagedList<DictionaryChecker>.ToPagedList(
					source, parameters, cancellationToken);

		return result;
	}

	public async Task<bool> TextHasExistAsync(
		string? id, string text, CancellationToken cancellationToken = default)
	{
		var result = await DbSet

			.Where(current => current.IsDeleted == false)

			.Where(current => string.IsNullOrEmpty(id) == true || current.Id != id)

			.Where(current => current.Text.ToLower().Trim() == text.ToLower().Trim())

			.AnyAsync(cancellationToken);

		return result;
	}

	public async Task<List<UiSelectModel>>
		GetUiSelectAsync(CancellationToken cancellationToken = default)
	{
		var result = await DbSet
			.Where(current => current.IsDeleted == false)

			.Select(current => new UiSelectModel(current.Text, current.Id))

			.ToListAsync(cancellationToken);

		return result;
	}

	public async Task<bool> CheckTextsAsync(List<string> texts, CancellationToken cancellationToken = default)
	{
		var result = await DbSet
			.Where(current => current.IsDeleted == false)
			.Where(current => current.IsActive == true)
			.Where(current => texts.Contains(current.Text) == true)
			.AnyAsync(cancellationToken: cancellationToken);

		return result;
	}
}
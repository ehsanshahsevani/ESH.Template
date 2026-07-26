using Domain;
using Persistence.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class FieldRepository : Repository<Field>, IFieldRepository
{
	internal FieldRepository(DatabaseContext databaseContext) : base(databaseContext)
	{
	}

	public async Task<List<Field>> GetByCategoryIdAsync(
		string? categoryId,
		bool? isActive = true,
		CancellationToken cancellationToken = default)
	{
		var category = await DatabaseContext.Categories
			.Where(current => current.Id == categoryId)
			.FirstOrDefaultAsync(cancellationToken);

		var currentCategoryFieldsTask = DbSet
			.Include(current => current.Category!)
			.ThenInclude(current => current.CategoryType!)

			.Include(current => current.FieldType)

			.Where(current => isActive.HasValue == false || current.IsActive == isActive.Value)

			.Where(current => current.IsDeleted == false)
			.Where(current => string.IsNullOrEmpty(categoryId) == true || current.CategoryId == categoryId)

			.OrderBy(current => current.Ordering)
			.ThenByDescending(current => current.CreateDateTime)

			.ToListAsync(cancellationToken);

		var result = await currentCategoryFieldsTask;

		if (category?.ParentId is not null)
		{
			var parentCategoryFields =
				await GetByCategoryIdAsync(category.ParentId, isActive: isActive);

			result = result
				.Concat(parentCategoryFields)
				.DistinctBy(field => field.Id)
				.OrderBy(current => current.Ordering)
				.ThenByDescending(current => current.CreateDateTime)
				.ToList();
		}

		return result;
	}

	public async Task<Field?> GetByCodeAndCateogryIdAsync(
		string code, string cateogryId, CancellationToken cancellationToken = default)
	{
		var result = await DbSet

			.Include(current => current.FieldType)

			.Where(current => current.IsDeleted == false)

			.Where(current => current.CategoryId == cateogryId)
			.Where(current => current.FieldType!.Code == code)

			.FirstOrDefaultAsync(cancellationToken);

		return result;
	}

	public override async Task<Field?> FindAsync(object id, bool? isActive = true,  CancellationToken cancellationToken = default)
	{
		var result = await DbSet

			.Include(current => current.FieldType)

			.Where(current => current.IsDeleted == false)

			.Where(current => current.Id == id.ToString())

			.FirstOrDefaultAsync(cancellationToken);

		return result;
	}
}
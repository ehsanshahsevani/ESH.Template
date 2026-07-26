using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence.Abstracts;

namespace Persistence.Repositories;

public class CategoryTypeRepository : Repository<CategoryType>, ICategoryTypeRepository
{
	internal CategoryTypeRepository(DatabaseContext databaseContext) : base(databaseContext)
	{
	}

	public async Task<CategoryType?> FindByCodeAsync(
		string code, CancellationToken cancellationToken = default)
	{
		var result = await DbSet

			.Where(current => current.Code == code)

			.FirstOrDefaultAsync(cancellationToken);

		return result;
	}
}

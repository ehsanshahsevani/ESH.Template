using Domain;
using Persistence.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class PhoneOperatorRepository : Repository<PhoneOperator>, IPhoneOperatorRepository
{
	internal PhoneOperatorRepository(DatabaseContext databaseContext) : base(databaseContext)
	{
	}

	public override async Task<IEnumerable<PhoneOperator?>>
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
}
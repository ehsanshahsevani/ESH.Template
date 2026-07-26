using Microsoft.EntityFrameworkCore;
using ESH.BuildingBlocks.RequestFeatures;

namespace Persistence;

public class Repository<TEntity> : Base.Repository<TEntity> where TEntity : ESH.SeedworkSystem.Domain.BaseEntity
{
	internal Repository(DatabaseContext databaseContext) : base(databaseContext)
	{
	}

	public override async Task<bool> RemoveByIdAsync
		(object id, CancellationToken cancellationToken = default)
	{
		TEntity? entity = await DbSet

			.Where(current => current.Id == id.ToString())

			.FirstOrDefaultAsync(cancellationToken: cancellationToken);

		if (entity is null)
		{
			throw new ArgumentNullException(paramName: nameof(id));
		}

		entity.IsDeleted = true;

		return true;
	}

	public override async Task RemoveAsync
		(TEntity? entity, CancellationToken cancellationToken = default)
	{
		if (entity is null)
		{
			throw new ArgumentNullException(paramName: nameof(entity));
		}

		TEntity? searchEntity =

			await DbSet

			.Where(current => current.Id == entity.Id)

			.FirstOrDefaultAsync(cancellationToken: cancellationToken);

		if (searchEntity is null)
		{
			throw new ArgumentNullException(paramName: nameof(searchEntity));
		}

		searchEntity.IsDeleted = true;
	}

	public override async Task RemoveRangeAsync
		(IEnumerable<TEntity?> entities, CancellationToken cancellationToken = default)
	{
		foreach (var item in entities)
		{
			await RemoveAsync(item, cancellationToken);
		}
	}

	public override async Task
		UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
	{
		await Task.Run(() =>
		{
			var attachedEntity = DatabaseContext.Attach(entity: entity);

			if (attachedEntity.State != EntityState.Modified)
			{
				attachedEntity.State = EntityState.Modified;
			}
		}, cancellationToken: cancellationToken);
	}

	public override async Task<IEnumerable<TEntity?>>
		GetAllAsync(CancellationToken cancellationToken = default)
	{
		var result = await DbSet

			.Where(current => current.IsDeleted == false)
			
			.ToListAsync(cancellationToken: cancellationToken);

		return result;
	}

	public override async Task<TEntity?>
		FindAsync(object id, bool? isActive = true,  CancellationToken cancellationToken = default)
	{
		var result = await DbSet

			.Where(current => current.IsDeleted == false)
			.Where(current => isActive.HasValue == false || current.IsActive == isActive.Value)

			.Where(current => current.Id == id.ToString())

			.FirstOrDefaultAsync(cancellationToken: cancellationToken);

		return result;
	}

	public virtual async Task<PagedList<TEntity>> GetAllInPageAsync(
		RequestParameters parameters, CancellationToken cancellationToken = default)
	{
		var source = DbSet

			.Where(current => current.IsDeleted == false)

			.OrderByDescending(current => current.CreateDateTime)

			;

		var result = await PagedList<TEntity>
			.ToPagedList(source, parameters, cancellationToken);

		return result;
	}
}
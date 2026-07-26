using System.Collections;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ESH.SeedworkSystem.Persistence;
using ESH.BuildingBlocks.SampleResult;
using ESH.SeedworkSystem.Domain;

namespace Persistence.Base;

public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
	protected Repository
		(DatabaseContext databaseContext) : base()
	{
		DatabaseContext =
			databaseContext ??
			throw new ArgumentNullException(paramName: nameof(databaseContext));

		DbSet =
			DatabaseContext.Set<TEntity>();
	}

	// **********
	protected DbSet<TEntity> DbSet { get; }
	// **********

	// **********
	protected DatabaseContext DatabaseContext { get; }
	// **********

	public virtual async Task<Result> AddAsync(TEntity? entity, CancellationToken cancellationToken = default)
	{
		if (entity == null)
		{
			throw new ArgumentNullException(paramName: nameof(entity));
		}

		await DbSet.AddAsync
			(entity: entity, cancellationToken: cancellationToken);

		return new Result();
	}

	public virtual async Task
		AddRangeAsync(IEnumerable<TEntity?> entities, CancellationToken cancellationToken = default)
	{
		if (entities == null)
		{
			throw new ArgumentNullException(paramName: nameof(entities));
		}

		await DbSet.AddRangeAsync
			(entities: entities!, cancellationToken: cancellationToken);
	}

	public virtual async Task
		RemoveAsync(TEntity? entity, CancellationToken cancellationToken = default)
	{
		if (entity == null)
		{
			throw new ArgumentNullException(paramName: nameof(entity));
		}

		await Task.Run(() =>
		{
			DbSet.Remove(entity: entity);

			//var attachedEntity =
			//	DatabaseContext.Attach(entity: entity);

			//attachedEntity.State =
			//	Microsoft.EntityFrameworkCore.EntityState.Deleted;

		}, cancellationToken: cancellationToken);
	}

	public virtual async Task<bool>
		RemoveByIdAsync(object id, CancellationToken cancellationToken = default)
	{
		TEntity? entity =
			await FindAsync(id: id, cancellationToken: cancellationToken);

		await RemoveAsync
			(entity: entity, cancellationToken: cancellationToken);

		return true;
	}

	public virtual async Task
		RemoveRangeAsync(IEnumerable<TEntity?> entities, CancellationToken cancellationToken = default)
	{
		if (entities == null)
		{
			throw new ArgumentNullException(paramName: nameof(entities));
		}

		foreach (var entity in entities)
		{
			await RemoveAsync
				(entity: entity, cancellationToken: cancellationToken);
		}
	}

	public virtual async Task
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

	public virtual async Task<IEnumerable<TEntity?>>
		GetAllAsync(CancellationToken cancellationToken = default)
	{
		// ToListAsync -> Extension Method -> using Microsoft.EntityFrameworkCore;
		var result =
			await DbSet.ToListAsync(cancellationToken: cancellationToken);

		return result;
	}

	public virtual async Task<IEnumerable<TEntity?>>
		Find(Expression<Func<TEntity?, bool>> predicate, CancellationToken cancellationToken = default)
	{
		// ToListAsync -> Extension Method -> using Microsoft.EntityFrameworkCore;
		var result =
				await DbSet.Where(predicate: predicate)
					.ToListAsync(cancellationToken: cancellationToken);

		return result;
	}

	public virtual async Task<TEntity?>
		FindAsync(object id, bool? isActive = true, CancellationToken cancellationToken = default)
	{
		var result =
			await DbSet
				
				.Where(current => current.IsDeleted == false)
					
				.Where(current => isActive.HasValue == false || current.IsActive == isActive.Value)
				
				.FirstOrDefaultAsync(cancellationToken: cancellationToken);

		return result;
	}

	public virtual async Task<bool>
		CheckIsDeletedAsync<T>(object entityId,
			CancellationToken cancellationToken = default) where T : BaseEntity
	{
		if (string.IsNullOrEmpty(entityId.ToString()) == true)
		{
			return false;
		}

		// *****************************************
		System.Reflection.PropertyInfo[]
			properties = typeof(T).GetProperties();

		var genericProps =
			properties.Where
				(current => current.PropertyType.IsGenericType == true &&
					current.PropertyType.GetGenericTypeDefinition() != typeof(Nullable<>));

		var joinsString = new List<string>();

		foreach (var item in genericProps)
		{
			var typeGeneric =
				item.PropertyType
				.GenericTypeArguments.FirstOrDefault();

			if (typeGeneric == null)
			{
				return false;
			}
			else
			{
				joinsString.Add(item.Name);
			}
		}
		// *****************************************

		for (int index = 0; index < joinsString.Count; index++)
		{
			string? table = joinsString[index];

			var entity = await DatabaseContext.Set<T>()

				.AsNoTracking()

				.Include(table)

				.Where(current => current.IsDeleted == false)

				.Where(current => current.Id == entityId)

				.FirstOrDefaultAsync()

				;

			if (entity is null)
			{
				return false;
			}

			System.Reflection.PropertyInfo
				propertyInfo =
				 entity
				.GetType()
				.GetProperties()
				.Where(current => current.PropertyType.IsGenericType
									&& current.Name == table)
				.First()
				;

			if (propertyInfo == null)
			{
				return false;
			}

			var typeGeneric =
				propertyInfo.PropertyType
				.GenericTypeArguments.FirstOrDefault();

			if (typeGeneric == null)
			{
				return false;
			}

			var listGeneric = CreateList(typeGeneric);

			listGeneric = propertyInfo.GetValue(entity) as IList;

			if (listGeneric is null || listGeneric.Count == 0)
			{
				continue;
			}

			foreach (dynamic itemEntity in listGeneric)
			{
				if (itemEntity is null)
				{
					return false;
				}
				else
				{
					bool result = itemEntity.IsDeleted == false;

					if (result == true)
					{
						return !result;
					}

					continue;
				}
			}
		}

		return true;
	}

	private static IList CreateList(Type myType)
	{
		Type genericListType = typeof(List<>).MakeGenericType(myType);
		return (IList)Activator.CreateInstance(genericListType)!;
	}
}

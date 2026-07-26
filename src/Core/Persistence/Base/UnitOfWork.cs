using ESH.SeedworkSystem.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Persistence.Base;

public abstract class UnitOfWork : ESH.SeedworkSystem.Persistence.IUnitOfWork
{
	public UnitOfWork(UnitOfWorkOptions options) : base()
	{
		Options = options;
	}

	public UnitOfWork(DatabaseContext databaseContext) : base()
	{
		_databaseContext = databaseContext;
	}

	// **********
	protected UnitOfWorkOptions? Options { get; set; }
	// **********

	// **********
	// **********
	// **********
	private DatabaseContext? _databaseContext;
	// **********

	// **********
	/// <summary>
	/// Lazy Loading = Lazy Initialization
	/// </summary>
	internal DatabaseContext DatabaseContext
	{
		get
		{
			if (_databaseContext is null)
			{
				if (Options is null)
				{
					throw new InvalidOperationException(
						"DatabaseContext is not provided via DI and UnitOfWorkOptions is null.");
				}

				var optionsBuilder =
					new DbContextOptionsBuilder<DatabaseContext>();

				switch (Options.Provider)
				{
					case Provider.SqlServer:
						{
							optionsBuilder.UseSqlServer
								(connectionString: Options.ConnectionString);

							break;
						}

					case Provider.MySql:
						{
							//optionsBuilder.UseMySql
							//	(connectionString: Options.ConnectionString);

							break;
						}

					case Provider.Oracle:
						{
							//optionsBuilder.UseOracle
							//	(connectionString: Options.ConnectionString);

							break;
						}

					case Provider.PostgreSQL:
						{
							//optionsBuilder.UsePostgreSQL
							//	(connectionString: Options.ConnectionString);

							break;
						}

					case Provider.InMemory:
						{
							if (Options.DatabaseName is not null)
							{
								optionsBuilder.UseInMemoryDatabase(databaseName: Options.DatabaseName);
							}
							else
							{
								optionsBuilder.UseInMemoryDatabase(databaseName: "fakeDatabase");
								;
							}

							break;
						}
				}

				_databaseContext =
					new DatabaseContext(optionsBuilder.Options as DbContextOptions<DatabaseContext>);
			}

			return _databaseContext;
		}
	}

	// **********
	/// <summary>
	/// To detect redundant calls
	/// </summary>
	public bool IsDisposed { get; protected set; }
	// **********

	private IDbContextTransaction? Transaction { get; set; }

	// **********
	public async Task<IDbContextTransaction> BeginTransactionAsync()
	{
		Transaction ??=
			await DatabaseContext.Database.BeginTransactionAsync();

		return Transaction;
	}
	// **********

	// **********
	public async Task CommitAsync()
	{
		await Transaction?.CommitAsync()!;
	}
	// **********

	// **********
	public async Task MigrateAsync()
	{
		await DatabaseContext.Database.MigrateAsync();
	}
	// **********

	// **********
	public async Task DatabaseEnsureCreatedAsync()
	{
		await DatabaseContext.Database.EnsureCreatedAsync();
	}
	// **********

	/// <summary>
	/// Public implementation of Dispose pattern callable by consumers.
	/// </summary>
	public void Dispose()
	{
		Dispose(true);

		GC.SuppressFinalize(this);
	}

	/// <summary>
	/// https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose
	/// </summary>
	protected virtual void Dispose(bool disposing)
	{
		if (IsDisposed)
		{
			return;
		}

		if (disposing)
		{
			// TODO: dispose managed state (managed objects).

			if (DatabaseContext != null)
			{
				DatabaseContext.Dispose();
			}
		}

		// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
		// TODO: set large fields to null.

		IsDisposed = true;
	}

	public async Task<ESH.BuildingBlocks.SampleResult.Result> SaveAsync(CancellationToken cancellationToken = default)
	{
		await DatabaseContext.SaveChangesAsync(cancellationToken);

		// var service = new UserRelationService();

		var result = new ESH.BuildingBlocks.SampleResult.Result();
		// await service.SaveChangesAsync(ServerKeyConstant.Key);

		return result;
	}

	// // private IUserRoleRepository _userRoleRepository;
	// public IUserRoleRepository UserRoleRepository
	// {
	//     get
	//     {
	//         _userRoleRepository ??= new UserRoleRepository(DatabaseContext);
	//         return _userRoleRepository;
	//     }
	// }

	~UnitOfWork()
	{
		Dispose(false);
	}
}
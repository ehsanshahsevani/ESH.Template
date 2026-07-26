using ESH.SeedworkSystem.Domain;
using Persistence;
using Microsoft.Extensions.Configuration;
using UnitTest.TestData.cs.Helpers;

namespace Base;

public abstract class BaseTestWithDatabaseInMemory : object
{
	protected IUnitOfWork UnitOfWork { get; }

	#region Database_Constructor_InitialData

	protected BaseTestWithDatabaseInMemory() : base()
	{
		UnitOfWork = UnitOfWorkFactory.Create();

		// **************************************************
		// sub system data
		// find all domains in Domain
		List<string> domians =
			BaseEntity.DomainFinder(nameof(Domain));

		// insert to db if not exist ...
		// UnitOfWork.SubSystemRepository.AddByNamesAsync(domians).GetAwaiter().GetResult();

		UnitOfWork.SaveAsync().GetAwaiter().GetResult();
		// **************************************************

		// **************************************************
		var config = new ConfigurationBuilder()
			.AddInMemoryCollection()
			.Build();

		// var seeder = new InitialData(config, UnitOfWork);
		// **************************************************
	}

	#endregion /Database_Constructor_InitialData

	#region Private Functions

	#endregion
}
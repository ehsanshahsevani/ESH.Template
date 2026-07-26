using Xunit.Abstractions;

namespace UnitTest.Repositories;

public class DocumentRepositoryTest : Base.BaseTestWithDatabaseInMemory
{
	private readonly ITestOutputHelper _testOutputHelper;

	#region Database_Constructor_InitialDat

	public DocumentRepositoryTest(ITestOutputHelper testOutputHelper) : base()
	{
		_testOutputHelper = testOutputHelper;
	}

	#endregion /Database_Constructor_InitialData

	#region Private_Functions

	#endregion /Private_Functions
}
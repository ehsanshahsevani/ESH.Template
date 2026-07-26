namespace UnitTest.Repositories;

public class AccountCodingRepositoryTest : Base.BaseTestWithDatabaseInMemory
{
	#region Database_Constructor_InitialData

	public AccountCodingRepositoryTest() : base()
	{
	}

	#endregion /Database_Constructor_InitialData

	#region FindByCodeAsync

	///// <summary>
	///// بررسی اینکه متد FindByCodeAsync کد حساب را به درستی پیدا می‌کند
	///// </summary>
	//[Fact(DisplayName = "بررسی اینکه متد FindByCodeAsync کد حساب را به درستی پیدا می‌کند")]
	//public async Task FindByCodeAsync()
	//{
	//	// Arrange
	//	var code = AccountCoding.ReferalCode;

	//	// Act
	//	var result =
	//		await UnitOfWork.AccountCodingRepository.FindByCodeAsync(code);

	//	// Assert
	//	// Assert.NotNull(result);
	//	result.Should().NotBeNull();

	//	// Assert.Equal(code, result!.Code);
	//	result!.Code.Should().Be(code);
	//}

	#endregion /FindByCodeAsync
}
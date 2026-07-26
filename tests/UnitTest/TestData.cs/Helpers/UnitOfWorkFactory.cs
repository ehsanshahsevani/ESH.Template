using Persistence;
using ESH.SeedworkSystem.Persistence;

using IUnitOfWork = Persistence.IUnitOfWork;

namespace UnitTest.TestData.cs.Helpers;

public static class UnitOfWorkFactory
{
	public static IUnitOfWork Create()
	{
		var options = new UnitOfWorkOptions(connectionString: "fakeDatabase")
		{
			Provider = Provider.InMemory,

			// هر بار که تست ما اجرا میشود یک دیتابیس جدا برایش ساخته میشود
			DatabaseName = Guid.NewGuid().ToString()
		};

		return new UnitOfWork(options);
	}
}
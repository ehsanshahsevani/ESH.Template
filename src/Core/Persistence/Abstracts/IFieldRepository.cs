using Domain;
using ESH.SeedworkSystem.Persistence;

namespace Persistence.Abstracts;

// For Feild (assuming it's a typo for Field)
public interface IFieldRepository : IRepository<Field>
{
	/// <summary>
	/// دریافت فیلدهای مرتبط با یک دسته بندی و دسته بندی والد (اگر وجود داشت)
	/// شامل Include‌های لازم برای دریافت FieldType و Category
	/// </summary>
	Task<List<Field>> GetByCategoryIdAsync(
		string? categoryId,
		bool? isActive = true,
		CancellationToken cancellationToken = default);
	Task<Field?> GetByCodeAndCateogryIdAsync(string code, string cateogryId, CancellationToken cancellationToken = default);
}

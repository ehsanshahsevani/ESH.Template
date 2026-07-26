using DynamicFields.Models;

namespace DynamicFields.Seed;

/// <summary>
/// سرویس برای دریافت فیلدهای کامل بر اساس نوع دسته‌بندی
/// Service to get complete field data based on category type
/// </summary>
public sealed class CategoryTypeFieldProvider
{
	private readonly Dictionary<string, FieldSeedModel> _fieldLookup;

	public CategoryTypeFieldProvider()
	{
		// جمع‌آوری تمام فیلدها از تمام کلاس‌های سید دیتا
		// Collect all fields from all seed data classes
		_fieldLookup = new Dictionary<string, FieldSeedModel>(StringComparer.OrdinalIgnoreCase);

		// بارگذاری فیلدهای Plate
		var plateSeedData = new FieldPlateSeedData();
		foreach (var field in plateSeedData.Data)
		{
			_fieldLookup.TryAdd(field.Code, field);
		}

		// بارگذاری فیلدهای Phone
		var phoneSeedData = new FieldPhoneSeedData();
		foreach (var field in phoneSeedData.Data)
		{
			_fieldLookup.TryAdd(field.Code, field);
		}

		// بارگذاری فیلدهای Property
		var propertySeedData = new FieldPropertySeedData();
		foreach (var field in propertySeedData.Data)
		{
			_fieldLookup.TryAdd(field.Code, field);
		}

		// بارگذاری فیلدهای Other
		var otherSeedData = new FieldOtherSeedData();
		foreach (var field in otherSeedData.Data)
		{
			_fieldLookup.TryAdd(field.Code, field);
		}
	}

	/// <summary>
	/// دریافت لیست کامل فیلدها برای یک نوع دسته‌بندی خاص
	/// </summary>
	public IReadOnlyList<FieldSeedModel> GetFieldsForCategoryType(string categoryType)
	{
		var fieldCodes =
			CategoryTypeFieldMapping.GetFieldsForCategoryType(categoryType: categoryType);

		var fields = new List<FieldSeedModel>();

		foreach (var fieldCode in fieldCodes)
		{
			if (_fieldLookup.TryGetValue(key: fieldCode, value: out var field))
			{
				fields.Add(item: field);
			}
		}

		return fields.AsReadOnly();
	}

	/// <summary>
	/// دریافت فیلد خاص برای یک نوع دسته‌بندی
	/// </summary>
	public FieldSeedModel? GetFieldForCategoryType(string categoryType, string fieldCode)
	{
		var isValidForCategoryType =
			CategoryTypeFieldMapping
				.IsFieldValidForCategoryType(categoryType: categoryType, fieldCode: fieldCode);

		if (isValidForCategoryType == false)
		{
			return null;
		}

		var result =
			_fieldLookup.GetValueOrDefault(key: fieldCode);

		return result;
	}

	/// <summary>
	/// بررسی اعتبار فیلد برای نوع دسته‌بندی
	/// </summary>
	public bool ValidateField(string categoryType, string fieldCode)
	{
		var result = CategoryTypeFieldMapping
			.IsFieldValidForCategoryType(categoryType: categoryType, fieldCode: fieldCode);

		return result;
	}
}
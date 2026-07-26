using Domain.Constants;

namespace DynamicFields.Seed;

/// <summary>
/// تعیین فیلدهای مربوط به هر نوع دسته‌بندی
/// Mapping between CategoryType and their related Fields
/// </summary>
public static class CategoryTypeFieldMapping
{
	static CategoryTypeFieldMapping()
	{
	}

	/// <summary>
	/// دریافت لیست فیلدهای مرتبط با نوع دسته‌بندی
	/// Get list of field codes for a specific category type
	/// </summary>
	/// <param name="categoryType">نوع دسته‌بندی مثل PLATE، PHONE و غیره</param>
	/// <returns>لیست کدهای فیلدها</returns>
	public static IReadOnlyList<string> GetFieldsForCategoryType(string categoryType)
	{
		return categoryType switch
		{
			CategoryTypes.Plate => PlateFields,
			CategoryTypes.Phone => PhoneFields,
			CategoryTypes.Property => PropertyFields,
			CategoryTypes.Other => OtherFields,
			_ => throw new ArgumentException(message: $"Invalid category type: {categoryType}",
				paramName: nameof(categoryType))
		};
	}

	/// <summary>
	/// فیلدهای مربوط به پلاک‌های خودرو
	/// Fields for vehicle plates (Oman plates)
	/// </summary>
	private static readonly string[] PlateFields =
	[
		FieldTypes.PlateNumberPart,
		FieldTypes.PlateLetter,
		FieldTypes.PlateStatus,
		FieldTypes.Price,
		FieldTypes.Region,
	];

	/// <summary>
	/// فیلدهای مربوط به شماره تلفن
	/// Fields for phone numbers
	/// </summary>
	private static readonly string[] PhoneFields =
	[
		FieldTypes.PhoneBody,
		FieldTypes.PhoneOperator,
		FieldTypes.Price,
		FieldTypes.Region,
	];

	/// <summary>
	/// فیلدهای مربوط به املاک
	/// Fields for properties (real estate)
	/// </summary>
	private static readonly string[] PropertyFields =
	[
		FieldTypes.Title,
		FieldTypes.Price,
		FieldTypes.Location,
		FieldTypes.Region,
		FieldTypes.Attachment,
		FieldTypes.Description
	];

	/// <summary>
	/// فیلدهای مربوط به سایر دسته‌ها
	/// Fields for other categories
	/// </summary>
	private static readonly string[] OtherFields =
	[
		FieldTypes.Title,
		FieldTypes.Price,
		FieldTypes.Attachment,
	];

	/// <summary>
	/// بررسی اینکه آیا یک فیلد برای نوع دسته‌بندی خاصی معتبر است یا نه
	/// Check if a field is valid for a specific category type
	/// </summary>
	public static bool IsFieldValidForCategoryType(string categoryType, string fieldCode)
	{
		var validFields =
			GetFieldsForCategoryType(categoryType: categoryType);

		var result =
			validFields.Contains(value: fieldCode);

		return result;
	}

	/// <summary>
	/// دریافت همه نگاشت‌ها به صورت دیکشنری
	/// Get all mappings as dictionary
	/// </summary>
	public static IReadOnlyDictionary<string, IReadOnlyList<string>> GetAllMappings()
	{
		return new Dictionary<string, IReadOnlyList<string>>
		{
			{ CategoryTypes.Plate, PlateFields },
			{ CategoryTypes.Phone, PhoneFields },
			{ CategoryTypes.Property, PropertyFields },
			{ CategoryTypes.Other, OtherFields }
		};
	}
}
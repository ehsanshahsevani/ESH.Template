using Domain.Constants;
using DynamicFields.Seed;

namespace Infrastructure;

/// <summary>
/// سرویس کمکی برای مدیریت فیلدهای دسته‌بندی بر اساس نوع
/// Helper service for managing category fields based on category type
/// 
/// این کلاس متدهای static برای دریافت فیلدهای پیشنهادی هر نوع دسته‌بندی فراهم می‌کند
/// This class provides static methods to get recommended fields for each category type
/// </summary>
public static class CategoryFieldHelper
{
	/// <summary>
	/// دریافت لیست کدهای فیلدهای پیشنهادی برای یک نوع دسته‌بندی
	/// Get list of recommended field codes for a category type
	/// 
	/// مثال / Example:
	/// <code>
	/// var plateFields = CategoryFieldHelper.GetRecommendedFields(CategoryTypes.Plate);
	/// // Result: ["PLATE_NUMBER_PART", "PLATE_LETTER", "PLATE_STATUS", ...]
	/// </code>
	/// </summary>
	public static IReadOnlyList<string> GetRecommendedFields(string categoryType)
	{
		return CategoryTypeFieldMapping.GetFieldsForCategoryType(categoryType: categoryType);
	}

	/// <summary>
	/// دریافت اطلاعات کامل فیلدهای پیشنهادی برای یک نوع دسته‌بندی
	/// Get complete field information for a category type
	/// 
	/// این متد اطلاعات کامل شامل عنوان فارسی، انگلیسی، نوع داده و تنظیمات را برمی‌گرداند
	/// This method returns complete information including Persian title, English title, data type and configurations
	/// </summary>
	public static IReadOnlyList<DynamicFields.Models.FieldSeedModel> GetFieldDetails(string categoryType)
	{
		var provider = new CategoryTypeFieldProvider();
		return provider.GetFieldsForCategoryType(categoryType: categoryType);
	}

	/// <summary>
	/// بررسی اینکه آیا یک فیلد برای نوع دسته‌بندی خاص معتبر است یا نه
	/// Check if a field is valid for a specific category type
	/// </summary>
	public static bool IsFieldValidForType(string categoryType, string fieldCode)
	{
		return CategoryTypeFieldMapping.IsFieldValidForCategoryType(categoryType: categoryType, fieldCode: fieldCode);
	}

	/// <summary>
	/// تعیین اینکه یک فیلد برای نوع دسته‌بندی خاص اجباری است یا نه
	/// Determine if a field should be required for a specific category type
	/// </summary>
	public static bool IsFieldRequired(string fieldTypeCode, string categoryType)
	{
		switch (categoryType, fieldCode: fieldTypeCode)
		{
			// Plate - فیلدهای اجباری پلاک
			case (categoryType: CategoryTypes.Plate, fieldCode: FieldTypes.PlateNumberPart):
			case (categoryType: CategoryTypes.Plate, fieldCode: FieldTypes.PlateLetter):
			case (categoryType: CategoryTypes.Plate, fieldCode: FieldTypes.PlateStatus):
			case (categoryType: CategoryTypes.Plate, fieldCode: FieldTypes.Price):
			case (categoryType: CategoryTypes.Plate, fieldCode: FieldTypes.Region):

			// Phone - فیلدهای اجباری تلفن
			case (categoryType: CategoryTypes.Phone, fieldCode: FieldTypes.PhoneBody):
			case (categoryType: CategoryTypes.Phone, fieldCode: FieldTypes.PhoneOperator):
			case (categoryType: CategoryTypes.Phone, fieldCode: FieldTypes.Price):
			case (categoryType: CategoryTypes.Phone, fieldCode: FieldTypes.Region):

			// Property - فیلدهای اجباری املاک
			case (categoryType: CategoryTypes.Property, fieldCode: FieldTypes.Title):
			case (categoryType: CategoryTypes.Property, fieldCode: FieldTypes.Price):
			case (categoryType: CategoryTypes.Property, fieldCode: FieldTypes.Location):
			case (categoryType: CategoryTypes.Property, fieldCode: FieldTypes.Region):
			case (categoryType: CategoryTypes.Property, fieldCode: FieldTypes.Attachment):

			// Other - فیلدهای اجباری سایر
			case (categoryType: CategoryTypes.Other, fieldCode: FieldTypes.Title):
			case (categoryType: CategoryTypes.Other, fieldCode: FieldTypes.Price):
			case (categoryType: CategoryTypes.Other, fieldCode: FieldTypes.Attachment):
				return true;
			default:
				// بقیه فیلدها اختیاری هستند
				return false;
		}
	}

	public static int GetOrdering(string fieldTypeCode, string categoryType)
	{
		switch (categoryType, fieldCode: fieldTypeCode)
		{
			// Plate - فیلدهای اجباری پلاک
			case (categoryType: CategoryTypes.Plate, fieldCode: FieldTypes.PlateNumberPart):
				return 20;
			case (categoryType: CategoryTypes.Plate, fieldCode: FieldTypes.PlateLetter):
				return 30;
			case (categoryType: CategoryTypes.Plate, fieldCode: FieldTypes.PlateStatus):
				return 10;
			case (categoryType: CategoryTypes.Plate, fieldCode: FieldTypes.Price):
				return 40;
			case (categoryType: CategoryTypes.Plate, fieldCode: FieldTypes.Region):
				return 50;

			// Phone - فیلدهای اجباری تلفن
			case (categoryType: CategoryTypes.Phone, fieldCode: FieldTypes.PhoneBody):
				return 10;
			case (categoryType: CategoryTypes.Phone, fieldCode: FieldTypes.PhoneOperator):
				return 20;
			case (categoryType: CategoryTypes.Phone, fieldCode: FieldTypes.Price):
				return 40;
			case (categoryType: CategoryTypes.Phone, fieldCode: FieldTypes.Region):
				return 30;

			// Property - فیلدهای اجباری املاک
			case (categoryType: CategoryTypes.Property, fieldCode: FieldTypes.Title):
				return 10;
			case (categoryType: CategoryTypes.Property, fieldCode: FieldTypes.Price):
				return 20;
			case (categoryType: CategoryTypes.Property, fieldCode: FieldTypes.Location):
				return 40;
			case (categoryType: CategoryTypes.Property, fieldCode: FieldTypes.Region):
				return 30;
			case (categoryType: CategoryTypes.Property, fieldCode: FieldTypes.Attachment):
				return 50;
			case (categoryType: CategoryTypes.Other, fieldCode: FieldTypes.Description):
				return 60;

			// Other - فیلدهای اجباری سایر
			case (categoryType: CategoryTypes.Other, fieldCode: FieldTypes.Title):
				return 10;
			case (categoryType: CategoryTypes.Other, fieldCode: FieldTypes.Price):
				return 20;
			case (categoryType: CategoryTypes.Other, fieldCode: FieldTypes.Attachment):
				return 30;
			default:
				// بقیه فیلدها اختیاری هستند
				return 100_000;
		}
	}

	/// <summary>
	/// دریافت تمام نگاشت‌های نوع دسته‌بندی به فیلدها
	/// Get all category type to fields mappings
	/// </summary>
	public static IReadOnlyDictionary<string, IReadOnlyList<string>> GetAllMappings()
	{
		return CategoryTypeFieldMapping.GetAllMappings();
	}
}
using ESH.Constant;
using Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace Domain;

/// <summary>
/// نوع فیلدهای مربوط به فرم ساز
/// </summary>

public class FieldType : Base.BaseEntity
{
	private FieldType()
	{
		CurrentConfigVersion = 1;

		IsActive = true;
		IsDeleted = false;
		Ordering = 100_000;

		Fields = [];
	}

	public FieldType(string code) : base()
	{
		Code = code;
		CurrentConfigVersion = 1;

		IsActive = true;
		IsDeleted = false;
		Ordering = 100_000;

		Fields = [];
	}

	// *********************************************
	/// <summary>
	/// کد
	/// براساس این کد تمامی ولیدیشن ها و ارتباطات دیتابیسی تعیین میشود
	/// در نظر داشته باشید آیدی تایپ ها برای هر سرچی در دیتابیس روی آگهی ها مهم میشود و الزامی است
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.Code))]

	[Required(
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.RequiredError))]

	[MaxLength(
		length: MaxLength.Title,
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.MaxLengthError))]

	public string Code { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// کد
	/// براساس این کد تمامی ولیدیشن ها و ارتباطات دیتابیسی تعیین میشود
	/// در نظر داشته باشید آیدی تایپ ها برای هر سرچی در دیتابیس روی آگهی ها مهم میشود و الزامی است
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.DataType))]

	[Required(
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.RequiredError))]

	[MaxLength(
		length: MaxLength.Title,
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.MaxLengthError))]

	public string DataType { get; set; }
	// *********************************************

	// *********************************************
	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.JsonConfig))]

	public string? JsonConfig { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// آخرین نسخه فعال کانفیگ این نوع فیلد
	/// Backward Compatibility
	/// </summary>

	public short CurrentConfigVersion { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// لیست همه فیلد هایی از این نوع
	/// </summary>
	public List<Field> Fields { get; set; }
	// *********************************************

	// *********************************************
	public bool IsAttachment()
	{
		if (Code == FieldTypes.Attachment)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	// *********************************************

	// *********************************************
	public bool IsPrice()
	{
		if (Code == FieldTypes.Price)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	// *********************************************

	// *********************************************
	public bool IsLocation()
	{
		if (Code == FieldTypes.Location)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	// *********************************************
	
	// *********************************************
	public bool IsCustomValue()
	{
		if (Code == FieldTypes.CustomValues)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	// *********************************************

	// *********************************************
	public bool IsGeneralText()
	{
		if (Code == FieldTypes.Title
			|| Code == FieldTypes.Description
			|| Code == FieldTypes.PhoneBody
			|| Code == FieldTypes.PlateNumberPart
			|| Code == FieldTypes.Text
			|| Code == FieldTypes.String
		   )
		{
			return true;
		}
		else
		{
			return false;
		}
	}// *********************************************
}
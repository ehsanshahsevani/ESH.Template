using ESH.Constant;
using System.ComponentModel.DataAnnotations;

namespace Domain;

/// <summary>
/// فیلدهای داینامیکی که با استفاده از تایپ های پیشفرض ما توسط ادمین و تیم فنی ایجاد میشوند
/// </summary>
public class Field : Base.BaseEntity
{
	#region Constatnts

	public const string NamePropertyLocalizer = "Name";
	public const string HintPropertyLocalizer = "Hint";
	public const string DescriptionPropertyLocalizer = "FieldDescription";

	#endregion /Constrants

	public Field() : base()
	{
		FieldMultiValues = new List<FieldMultiValue>();
		FieldValueAnnouncements = new List<FieldValueAnnouncement>();
	}

	// *********************************************
	/// <summary>
	/// ولیدیشن هایی که برای کل این فیلد تایپ ها استفاده میشوند
	/// </summary>

	public string? JsonConfig { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// نسخه کانفیگ این فیلد در زمان ایجاد
	/// برای جلوگیری از ناسازگاری هنگام تغییر Config ها
	/// </summary>

	public short ConfigVersion { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// این فیلدها اصلا توسط بک اند برای فرانت هنگام ثبت آگهی ارسال نمیشوند
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.IsRequired))]

	public bool IsRequired { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// نوع مربوط به این فیلد
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.FieldType))]

	[Required(
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.RequiredError))]

	[MaxLength(
		length: FixedLength.Guid,
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.MaxLengthError))]

	public string FieldTypeId { get; set; }

	public FieldType? FieldType { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// دسته بندی
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.Category))]

	[Required(
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.RequiredError))]

	[MaxLength(
		length: FixedLength.Guid,
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.MaxLengthError))]

	public string CategoryId { get; set; }

	public Category? Category { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// مقادیر پیشفرض فیلدهایی که نوع آنها 
	/// MultiValue
	/// می باشد
	/// </summary>

	public List<FieldMultiValue> FieldMultiValues { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// لیست مقدار فیلد های مربوط به این آگهی در صورتی که توسط کاربری آگهی ثبت شده باشد
	/// </summary>

	public List<FieldValueAnnouncement> FieldValueAnnouncements { get; set; }
	// *********************************************
}
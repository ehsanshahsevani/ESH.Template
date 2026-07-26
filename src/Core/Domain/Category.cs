using System.ComponentModel.DataAnnotations;
using ESH.Constant;

namespace Domain;

/// <summary>
/// دسته بندی
/// </summary>
public class Category : Base.BaseEntity
{
	#region Constants

	public const string PropertyNameKey = "Name";

	#endregion /Constants

#pragma warning disable CS8618, CS9264
	public Category() : base()
#pragma warning restore CS8618, CS9264
	{
		Feilds = new List<Field>();
		Children = new List<Category>();
		Announcements = new List<Announcement>();
	}

	// *********************************************
	/// <summary>
	/// کد دسته بندی
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.Code))]

	[MaxLength(
		length: 50,
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.MaxLengthError))]

	public string? Code { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// دسته بندی والد
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.Parent))]

	[MaxLength(
		length: FixedLength.Guid,
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.MaxLengthError))]

	public string? ParentId { get; set; }

	public Category? Parent { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// زیر دسته ها
	/// </summary>
	public List<Category> Children { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// نوع دسته بندی
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.CategoryType))]

	[Required(
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.RequiredError))]

	[MaxLength(
		length: FixedLength.Guid,
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.MaxLengthError))]

	public string CategoryTypeId { get; set; }

	public CategoryType? CategoryType { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// پین در صفحه اصلی ترجیحا وب سایت
	/// </summary>

	// [Display(
	// 	ResourceType = typeof(ESH.Resources.DataDictionary),
	// 	Name = nameof(ESH.Resources.DataDictionary.pin))]

	public int? PinInHome { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// لیست آگهی های مربوط به این دسته بندی
	/// </summary>
	public List<Announcement> Announcements { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// لیست فیلدهای مربوط به این دسته بندی آگهی
	/// </summary>
	public List<Field> Feilds { get; set; }
	// *********************************************
}
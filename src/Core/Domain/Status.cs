using System.ComponentModel.DataAnnotations;

namespace Domain;

/// <summary>
/// وضعیت آگهی
/// </summary>
public class Status : Base.BaseEntity
{
	#region Constatnts

	public const string TitleProperty = "Title";

	#endregion /Constrants

	private Status() { }

	public Status(int code) : base()
	{
		Code = code;
		Ordering = code;
		Announcements = new List<Announcement>();
	}

	// *********************************************
	/// <summary>
	/// کد مربوط به روند کار سیستم
	///  برای بررسی و آپدیت و اعمال مورد چندزبانگی
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.Code))]

	[Required(
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.RequiredError))]

	public int Code { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// لیست آگهی های مربوط به این وضعیت
	/// </summary>
	public List<Announcement> Announcements { get; set; }
	// *********************************************
}
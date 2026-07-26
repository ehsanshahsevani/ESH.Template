using ESH.Constant;
using System.ComponentModel.DataAnnotations;
using ESH.SeedworkSystem.Domain.MultiLanguage;

namespace Domain;

/// <summary>
/// آگهی
/// </summary>
public class Announcement : Base.BaseEntity
{
	public Announcement() : base()
	{
		Notes = new List<Note>();
		Favorites = new List<Favorite>();
		ReportLogs = new List<ReportLog>();
		NeedToEditLogs = new List<NeedToEditLog>();
		AnnouncementViews = new List<AnnouncementViews>();
		FieldValueAnnouncements = new List<FieldValueAnnouncement>();
	}

	// *********************************************
	/// <summary>
	/// نمایش دکمه مشاهده پروفایل در جزئیات آگهی کاربر
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.IsShowProfileInAnnouncement))]

	public bool IsShowProfileInAnnouncement { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// اگر در فیلدهای این آگهی قیمت باشد همزمان به این نقظه ارسال میشود
	/// فقط برای مرتب سازی استفاده میشود
	/// </summary>
	public int Price { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// کاربر میتواند خودش انتخاب کند که آگهی اش برای بقیه قابل مشاهده باشد یا خیر؟
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.IsHidden))]

	public bool IsHidden { get; private set; }

	public void SetIsHidden(bool isHidden)
	{
		IsHidden = isHidden;
		UpdateDateTime = ESH.Utilities.DateTools.DateTimeNow();
	}
	// *********************************************

	// *********************************************
	/// <summary>
	/// هشداری مربوط به دیکشنری وجود دارد یا خیر؟
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.HasWarningDictionaryChecker))]

	public bool HasWarningDictionaryChecker { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// پروفایل کاربر
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.Profile))]

	[Required(
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.RequiredError))]

	[MaxLength(
		length: FixedLength.Guid,
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.MaxLengthError))]

	public string ProfileId { get; set; }

	public Profile? Profile { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// کلمه غیراخلاقی
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.DictionaryChecker))]

	[MaxLength(
		length: FixedLength.Guid,
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.MaxLengthError))]

	public string? DictionaryCheckerId { get; set; }

	public DictionaryChecker? DictionaryChecker { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// دسته بندی
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.Name))]

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
	/// وضعیت آگهی
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.Status))]

	[Required(
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.RequiredError))]

	[MaxLength(
		length: FixedLength.Guid,
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.MaxLengthError))]

	public string StatusId { get; private set; }

	public Status? Status { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// دلیل حذف آگهی
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.DeleteReason))]

	[MaxLength(
		length: FixedLength.Guid,
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.MaxLengthError))]

	public string? DeleteReasonId { get; set; }

	/// <summary>
	/// زمانی که دلیل نیاز به توضیح دارد باید این فیلد پر شود
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.DeleteReason))]

	[MaxLength(
		length: MaxLength.Description,
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.MaxLengthError))]

	public string? DeleteReasonDescription { get; set; }

	public DeleteReason? DeleteReason { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// کد زبان
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.LanguageCode))]

	[Required(
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.RequiredError))]

	[MaxLength(
		length: FixedLength.Guid,
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.MaxLengthError))]

	public string LanguageCodeId { get; set; }

	public LanguageCode? LanguageCode { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// لیست یادداشت های مربوط به آگهی ها
	/// </summary>
	public List<Note> Notes { get; set; }

	/// <summary>
	/// لیست علاقه مندی ها
	/// </summary>
	public List<Favorite> Favorites { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// لیست رکوردهایی که از این دلیل ویرایش استفاده کرده اند
	/// </summary>
	public List<NeedToEditLog> NeedToEditLogs { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// لیست آگهی های گزارش شده
	/// </summary>
	public List<ReportLog> ReportLogs { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// لیست مقدار فیلد های مربوط به این آگهی در صورتی که توسط کاربری آگهی ثبت شده باشد
	/// </summary>

	public List<FieldValueAnnouncement> FieldValueAnnouncements { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// لیست مقدار فیلد های مربوط به این آگهی در صورتی که توسط کاربری آگهی ثبت شده باشد
	/// </summary>

	public List<AnnouncementViews> AnnouncementViews { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// طول جغرافیایی
	/// </summary>
	public double? Latitude { get; set; }
	
	/// <summary>
	/// عرض جغرافیایی
	/// </summary>
	public double? Longitude { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// طول جغرافیایی
	/// </summary>

	public bool? BlurPlateLetters { get; set; }
	// *********************************************

	// *********************************************
	public void SetIsActive(bool isActive)
	{
		IsActive = isActive;
		
		UpdateDateTime =
			ESH.Utilities.DateTools.DateTimeNow();
	}

	public void SetStatusId(string statusId, bool? isHidden = null)
	{
		StatusId = statusId;

		if (isHidden.HasValue == true)
		{
			IsHidden = isHidden.Value;
		}
		
		UpdateDateTime =
			ESH.Utilities.DateTools.DateTimeNow();
	}
}
using ESH.Constant;
using System.ComponentModel.DataAnnotations;
using ESH.SeedworkSystem.Domain.MultiLanguage;

namespace Domain;

/// <summary>
/// پروفایل اشخاص
/// </summary>
public class Profile : Base.BaseProfile
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
	private Profile() : base()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
	{
		Notes = new List<Note>();
		BookMarks = new List<Favorite>();

		Announcements = new List<Announcement>();

		ReportLogs = new List<ReportLog>();
		NeedToEditLogs = new List<NeedToEditLog>();
	}

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
	public Profile(string userId, string phoneNumber)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
	{
		SetId(userId);

		Notes = new List<Note>();
		BookMarks = new List<Favorite>();

		ReportLogs = new List<ReportLog>();
		NeedToEditLogs = new List<NeedToEditLog>();

		UserId = userId ?? throw new ArgumentNullException(nameof(userId));
		FullPhoneNumber = phoneNumber ?? throw new ArgumentNullException(nameof(phoneNumber));
	}

	// *********************************************
	/// <summary>
	/// شناسه کاربر
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.User))]

	[Required(
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.RequiredError))]

	[MaxLength(
		length: FixedLength.Guid,
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.MaxLengthError))]

	public override string UserId { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// نام
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.DisplayName))]

	[MaxLength(
		length: MaxLength.Title,
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.MaxLengthError))]

	public string? DisplayName { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// شماره تلفن
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.FullPhoneNumber))]

	[Required(
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.RequiredError))]

	public override string FullPhoneNumber { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// نمایش پروفایل در هر آگهی
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.PhoneNumber))]

	public bool ShowProfileInAnnouncement { get; private set; }
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
	/// لیست یادداشت های مربوط به آگهی های این شخص
	/// </summary>
	public List<Note> Notes { get; set; }

	/// <summary>
	/// لیست آگهی های ذخیره شده ی این شخص
	/// </summary>
	public List<Favorite> BookMarks { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// آگهی های گزارش شده توسط این شخص
	/// </summary>
	public List<ReportLog> ReportLogs { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// لیست رکوردهایی که از این دلیل ویرایش استفاده کرده اند
	/// </summary>
	public List<NeedToEditLog> NeedToEditLogs { get; set; }
	// *********************************************

	// *********************************************
	public List<Announcement> Announcements { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// بازدید های مربوط به یک آگهی
	/// </summary>
	public List<AnnouncementViews> AnnouncementViews { get; set; }
	// *********************************************

	// *********************************************
	public void SetShowProfileInAnnouncement(bool value)
	{
		this.ShowProfileInAnnouncement = value;
		UpdateDateTime = ESH.Utilities.DateTools.DateTimeNow();
	}
	// *********************************************
}
using System.ComponentModel.DataAnnotations;
using ESH.Constant;

namespace Domain;

/// <summary>
/// رکوردهای مربوط به گزارشات کاربران
/// </summary>
public class ReportLog : Base.BaseEntity
{
	public ReportLog() : base()
	{
	}

	// *********************************************
	/// <summary>
	/// ریلیشن به جدول NeedToEditReason
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.NeedToEditReason))]

	[Required(
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.RequiredError))]

	[MaxLength(
		length: FixedLength.Guid,
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.MaxLengthError))]

	public string ReportReasonId { get; set; }
	public ReportReason ReportReason { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// ریلیشن به جدول Announcement
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.Announcement))]

	[Required(
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.RequiredError))]

	[MaxLength(
		length: FixedLength.Guid,
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.MaxLengthError))]

	public string AnnouncementId { get; set; }
	public Announcement Announcement { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// ریلیشن به جدول Profile
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
	public Profile Profile { get; set; }
	// *********************************************
}
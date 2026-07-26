using ESH.Constant;
using System.ComponentModel.DataAnnotations;

namespace Domain;

public class AnnouncementViews : Base.BaseEntity
{
	public AnnouncementViews() : base()
	{
	}

	// *********************************************
	/// <summary>
	/// آگهی
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

	public Announcement? Announcement { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// پروفایل کاربر
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.Profile))]

	[MaxLength(
		length: FixedLength.Guid,
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.MaxLengthError))]

	public string? ProfileId { get; set; }

	public Profile? Profile { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// آدس آی پی
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.Profile))]

	[MaxLength(
		length: MaxLength.IP,
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.MaxLengthError))]

	public string? IpAddress { get; set; }
	// *********************************************
}

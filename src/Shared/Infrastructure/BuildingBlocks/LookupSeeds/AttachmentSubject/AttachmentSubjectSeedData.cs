using ESH.Constant.Attachment.Announcement;

namespace Infrastructure.BuildingBlocks.LookupSeeds.AttachmentSubject;

public class AttachmentSubjectSeedData
{
	private static readonly AttachmentSubjectModel[] _data =
	[
		new(
			Code: AnnouncementAttachmentSubjectKeys.ProfileImageSmall,
			DisplayName: nameof(AnnouncementAttachmentSubjectKeys.ProfileImageSmall)
		),
		new(
			Code: AnnouncementAttachmentSubjectKeys.ProfileImageLarge,
			DisplayName: nameof(AnnouncementAttachmentSubjectKeys.ProfileImageLarge)
		),
		new(
			Code: AnnouncementAttachmentSubjectKeys.AnnouncementImage,
			DisplayName: nameof(AnnouncementAttachmentSubjectKeys.AnnouncementImage)
		),
		new(
			Code: AnnouncementAttachmentSubjectKeys.CategoryImageSmall,
			DisplayName: nameof(AnnouncementAttachmentSubjectKeys.CategoryImageSmall)
		),
		new(
			Code: AnnouncementAttachmentSubjectKeys.CategoryImageLarge,
			DisplayName: nameof(AnnouncementAttachmentSubjectKeys.CategoryImageLarge)
		),
		new(
			Code: AnnouncementAttachmentSubjectKeys.PhoneOperator,
			DisplayName: nameof(AnnouncementAttachmentSubjectKeys.PhoneOperator)
		)
	];

	public IReadOnlyList<AttachmentSubjectModel> Data => _data;
}

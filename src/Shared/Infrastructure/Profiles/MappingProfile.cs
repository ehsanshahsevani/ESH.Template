using Domain;
using ESH.ViewModels.Shared;
using ESH.ViewModels.Announcement;
using ESH.BuildingBlocks.Localization.Contract;
using ESH.SeedworkSystem.Domain.SubSystem;
using ESH.SeedworkSystem.ViewModel.Localizer;
using FieldRequestViewModel = ESH.ViewModels.Announcement.FieldRequestViewModel;

namespace Infrastructure.Profiles;

public class MappingProfile : AutoMapper.Profile
{
	public MappingProfile() : base()
	{
		// **************************************************
		CreateMap<Profile, MiniProfileResponseViewModel>()

			.ForMember(destinationMember: dest => dest.LanguageCodeDisplayName,
				option =>
				{
					option.MapFrom(profile => profile.LanguageCode!.Code);
				})

			.ReverseMap()

			;

		CreateMap<ProfileRequestViewModel, Profile>()
			.ForMember(destinationMember: dest => dest.Id,
				memberOptions: opt =>
					opt.MapFrom(mapExpression: src =>
						string.IsNullOrEmpty(src.Id) == true ? Guid.NewGuid().ToString() : src.Id))
			.ForMember(destinationMember: dest => dest.UpdateDateTime,
				memberOptions: opt =>
					opt.MapFrom(mapExpression: src => DateTime.Now))
			;
		// **************************************************

		// **************************************************
		CreateMap<Category, CategoryResponseViewModel>()

			.ForMember(destinationMember: viewModel => viewModel.CategoryTypeCode,
				memberOptions: option =>
				{
					option.MapFrom(mapExpression: model => model.CategoryType!.Code);
				})

			.ReverseMap()

			;

		CreateMap<CategoryRequestViewModel, Category>()
			
			.ForMember(destinationMember: dest => dest.Id,
				memberOptions: opt =>
					opt.MapFrom(mapExpression: src =>
						string.IsNullOrEmpty(src.Id) == true ? Guid.NewGuid().ToString() : src.Id))
			
			.ForMember(destinationMember: dest => dest.UpdateDateTime,
				memberOptions: opt =>
					opt.MapFrom(mapExpression: src => DateTime.Now))

			.ReverseMap();
		// **************************************************
		
		// **************************************************
		CreateMap<ContactUs, ContactUsResponseViewModel>()

			.ReverseMap()

			;

		CreateMap<ContactUsRequestViewModel, ContactUs>()
			
			.ForMember(destinationMember: dest => dest.Id,
				memberOptions: opt =>
					opt.MapFrom(mapExpression: src =>
						string.IsNullOrEmpty(src.Id) == true ? Guid.NewGuid().ToString() : src.Id))
			
			.ForMember(destinationMember: dest => dest.UpdateDateTime,
				memberOptions: opt =>
					opt.MapFrom(mapExpression: src => DateTime.Now))

			.ReverseMap();
		// **************************************************

		// **************************************************
		// CreateMap<Attachment, AttachmentResponseViewModel>()
		// 	.ForMember(destinationMember: x => x.AttachmentSubjectDisplayName,
		// 		memberOptions: opt =>
		// 		{
		// 			opt.MapFrom(mapExpression: attachment => attachment.AttachmentSubject!.DisplayName);
		// 		})
		// 	;
		//
		// CreateMap<AttachmentRequestViewModel, Attachment>()
		// 	.ForMember(destinationMember: dest => dest.Id,
		// 		memberOptions: opt =>
		// 			opt.MapFrom(mapExpression: src =>
		// 				string.IsNullOrEmpty(src.Id) == true ? Guid.NewGuid().ToString() : src.Id))
		// 	.ForMember(destinationMember: dest => dest.UpdateDateTime,
		// 		memberOptions: opt =>
		// 			opt.MapFrom(mapExpression: src => DateTime.Now))
		// 	.ReverseMap();
		// **************************************************

		// **************************************************
		CreateMap<SubSystem, SubSystemResponseViewModel>()

			.ReverseMap()

			;
		// **************************************************

		// **************************************************
		CreateMap<Status, StatusResponseViewModel>();

		CreateMap<CounterDataPack<Status>, CounterDataPack<StatusResponseViewModel>>()
			.ForMember(destinationMember: dest => dest.Data, memberOptions: opt => opt.MapFrom(mapExpression: src => src.Data));

		// **************************************************

		// **************************************************
		CreateMap<ReportReason, ReportReasonResponseViewModel>()
			.ReverseMap()
			;

		CreateMap<ReportReasonRequestViewModel, ReportReason>()

			.ForMember(destinationMember: domain => domain.Id, memberOptions: option =>
			{
				option.MapFrom(mapExpression: viewModel => viewModel.Id ?? Guid.NewGuid().ToString());
			})

			;
		// **************************************************

		// **************************************************
		CreateMap<ValueLocalizer, ValueLocalizerViewModel>()
			.ReverseMap()
			;
		// **************************************************

		// **************************************************
		CreateMap<NeedToEditReason, NeedToEditReasonResponseViewModel>()
			.ReverseMap()
			;

		CreateMap<NeedToEditReasonRequestViewModel, NeedToEditReason>()

			.ForMember(destinationMember: domain => domain.Id, memberOptions: option =>
			{
				option.MapFrom(mapExpression: viewModel => viewModel.Id ?? Guid.NewGuid().ToString());
			})

			;

		// **************************************************

		// **************************************************
		CreateMap<DeleteReason, DeleteReasonResponseViewModel>()

			.ReverseMap()

			;

		CreateMap<DeleteReasonRequestViewModel, DeleteReason>()


			.ForMember(destinationMember: domain => domain.Id, memberOptions: option =>
			{
				option.MapFrom(mapExpression: viewModel => viewModel.Id ?? Guid.NewGuid().ToString());
			})

			;
		// **************************************************

		// **************************************************
		CreateMap<CategoryType, CategoryTypeResponseViewModel>()
			.ReverseMap();
		// **************************************************

		// **************************************************
		CreateMap<Region, RegionResponseViewModel>()
			.ReverseMap();
		// **************************************************

		// **************************************************
		CreateMap<PlateStatus, PlateStatusResponseViewModel>()
			.ReverseMap();
		// **************************************************

		// **************************************************
		CreateMap<PlateCode, PlateCodeResponseViewModel>()
			.ReverseMap();
		// **************************************************

		// **************************************************
		CreateMap<ReportLog, ReportLogResponseViewModel>()

			.ForMember(current => current.Profile,
				option =>
				{
					option.MapFrom(domain => domain.Profile);
				})

			.ReverseMap()

			;

		CreateMap<ReportLog, ReportLogRequestViewModel>()
			
			.ReverseMap()
			
			;
		// **************************************************

		// **************************************************
		CreateMap<PhoneOperator, PhoneOperatorResponseViewModel>()
			.ReverseMap();
		// **************************************************

		// **************************************************
		CreateMap<FieldType, FieldTypeResponseViewModel>()
			.ReverseMap();
		// **************************************************

		// **************************************************
		CreateMap<FieldType, FieldTypeResponseViewModel>()
			.ReverseMap();
		// **************************************************

		// **************************************************
		CreateMap<Note, NoteResponseViewModel>()

			.ReverseMap()

			;
		// **************************************************

		// **************************************************
		CreateMap<Note, NoteRequestViewModel>()

			.ReverseMap()

			;
		// **************************************************

		// **************************************************
		CreateMap<Announcement, AnnouncementResponseViewModel>()
			.ForMember(destinationMember: current => current.CategoryTypeCode, memberOptions: option =>
			{
				option.MapFrom(mapExpression: current => current.Category!.CategoryType!.Code);
			})

			.ForMember(destinationMember: current => current.CategoryTypeId, memberOptions: option =>
			{
				option.MapFrom(mapExpression: current => current.Category!.CategoryType!.Id);
			})

			.ForMember(destinationMember: current => current.Profile, memberOptions: option =>
			{
				option.MapFrom(mapExpression: current => current.Profile);
			})

			.ForMember(destinationMember: current => current.Fields, memberOptions: option =>
			{
				option.MapFrom(mapExpression: current => current.FieldValueAnnouncements);
			})

			.ForPath(destinationMember: current => current.Profile!.LanguageCodeDisplayName,
				memberOptions: option =>
					{
						option.MapFrom(
							current => current.Profile!.LanguageCode!.Code);
					})

			.ReverseMap()

			;

		CreateMap<Announcement, AnnouncementRequestViewModel>()

			.ReverseMap()

			.ForMember(destinationMember: domain => domain.Id, memberOptions: option =>
			{
				option.MapFrom(mapExpression: viewModel => viewModel.Id ?? Guid.NewGuid().ToString());
			})

		;

		CreateMap<Announcement, AnnouncementMiniResponseViewModel>()

			.ReverseMap()

		;

		CreateMap<FieldValueAnnouncement, FieldValueAnnouncementResponseViewModel>()

			.ForMember(destinationMember: current => current.FieldTypeId, memberOptions: option =>
			{
				option.MapFrom(mapExpression: current => current.Field!.FieldTypeId);
			})

			.ForMember(destinationMember: current => current.FieldTypeCode, memberOptions: option =>
			{
				option.MapFrom(mapExpression: current => current.Field!.FieldType!.Code);
			})

			.ForMember(destinationMember: current => current.Ordering, memberOptions: option =>
			{
				option.MapFrom(mapExpression: current => current.Field!.Ordering);
			})

			.ReverseMap()

			;
		// **************************************************

		// **************************************************
		CreateMap<Field, FieldResponseViewModel>()

			.ForMember(destinationMember: current => current.FieldTypeCode, memberOptions: option =>
			{
				option.MapFrom(mapExpression: current => current.FieldType!.Code);
			})

			.ForMember(destinationMember: current => current.FieldTypeDataType, memberOptions: option =>
			{
				option.MapFrom(mapExpression: current => current.FieldType!.DataType);
			})

			.ForMember(destinationMember: current => current.CategoryCode, memberOptions: option =>
			{
				option.MapFrom(mapExpression: current => current.Category!.Code);
			})

			.ReverseMap()

			;

		CreateMap<Field, FieldRequestViewModel>()

			.ReverseMap()

			.ForMember(destinationMember: domain => domain.Id, memberOptions: option =>
			{
				option.MapFrom(mapExpression: viewModel => viewModel.Id ?? Guid.NewGuid().ToString());
			})

			;

		CreateMap<DictionaryChecker, DictionaryCheckerResponseViewModel>()

			.ReverseMap()

			.ForMember(destinationMember: domain => domain.Id, memberOptions: option =>
			{
				option.MapFrom(mapExpression: viewModel => viewModel.Id ?? Guid.NewGuid().ToString());
			})

		;

		CreateMap<DictionaryChecker, DictionaryCheckerRequestViewModel>()

			.ReverseMap()

			.ForMember(destinationMember: domain => domain.Id, memberOptions: option =>
			{
				option.MapFrom(mapExpression: viewModel => viewModel.Id ?? Guid.NewGuid().ToString());
			})

		;
		// **************************************************
	}
}
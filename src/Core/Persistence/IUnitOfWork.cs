using Persistence.Abstracts;

namespace Persistence;

public interface IUnitOfWork :
	ESH.SeedworkSystem.Persistence.IUnitOfWork
{
	public ICategoryRepository CategoryRepository { get; }
	public IProfileRepository ProfileRepository { get; }
	public ICategoryTypeRepository CategoryTypeRepository { get; }
	public IFieldTypeRepository FieldTypeRepository { get; }
	public IFieldRepository FieldRepository { get; }
	public IFieldMultiValueRepository FieldMultiValueRepository { get; }
	public IFieldValueAnnouncementRepository FieldValueAnnouncementRepository { get; }
	public IStatusRepository StatusRepository { get; }
	public IAnnouncementRepository AnnouncementRepository { get; }
	public INoteRepository NoteRepository { get; }
	public IFavoriteRepository FavoriteRepository { get; }
	public IDeleteReasonRepository DeleteReasonRepository { get; }
	public INeedToEditReasonRepository NeedToEditReasonRepository { get; }
	public INeedToEditLogRepository NeedToEditLogRepository { get; }
	public IReportReasonRepository ReportReasonRepository { get; }
	public IReportLogRepository ReportLogRepository { get; }
	public IDictionaryCheckerRepository DictionaryCheckerRepository { get; }
	public IRegionRepository RegionRepository { get; }
	public IPhoneOperatorRepository PhoneOperatorRepository { get; }
	public IPlateCodeRepository PlateCodeRepository { get; }
	public IPlateStatusRepository PlateStatusRepository { get; }
	public IAnnouncementViewsRepository AnnouncementViewsRepository { get; }
	public IContactUsRepository ContactUsRepository { get; }
	ICaptchaHistoryRepository CaptchaHistoryRepository{ get; }
}
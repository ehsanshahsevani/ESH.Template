using Persistence.Abstracts;
using Persistence.Repositories;

namespace Persistence;

public class UnitOfWork : Base.UnitOfWork, IUnitOfWork
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public UnitOfWork(ESH.SeedworkSystem.Persistence.UnitOfWorkOptions options) : base(options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	{
	}

	public UnitOfWork(Persistence.DatabaseContext databaseContext) : base(databaseContext)
	{
	}

	private ICategoryRepository _categoryRepository;

	public ICategoryRepository CategoryRepository
	{
		get
		{
			_categoryRepository ??= new CategoryRepository(DatabaseContext);
			return _categoryRepository;
		}
	}
	// **************************************************

	// **************************************************
	private IProfileRepository _profileRepository;

	public IProfileRepository ProfileRepository
	{
		get
		{
			_profileRepository ??= new ProfileRepository(DatabaseContext);
			return _profileRepository;
		}
	}
	// **************************************************

	private ICategoryTypeRepository _categoryTypeRepository;

	public ICategoryTypeRepository CategoryTypeRepository
	{
		get
		{
			_categoryTypeRepository ??= new CategoryTypeRepository(DatabaseContext);
			return _categoryTypeRepository;
		}
	}

	private IFieldTypeRepository _fieldTypeRepository;

	public IFieldTypeRepository FieldTypeRepository
	{
		get
		{
			_fieldTypeRepository ??= new FieldTypeRepository(DatabaseContext);
			return _fieldTypeRepository;
		}
	}

	private IFieldRepository _fieldRepository;

	public IFieldRepository FieldRepository
	{
		get
		{
			_fieldRepository ??= new FieldRepository(DatabaseContext);
			return _fieldRepository;
		}
	}

	private IFieldMultiValueRepository _fieldMultiValueRepository;

	public IFieldMultiValueRepository FieldMultis
	{
		get
		{
			_fieldMultiValueRepository ??= new FieldMultiValueRepository(DatabaseContext);
			return _fieldMultiValueRepository;
		}
	}

	private IStatusRepository _statusRepository;

	public IStatusRepository StatusRepository
	{
		get
		{
			_statusRepository ??= new StatusRepository(DatabaseContext);
			return _statusRepository;
		}
	}

	private IAnnouncementRepository _announcementRepository;

	public IAnnouncementRepository AnnouncementRepository
	{
		get
		{
			_announcementRepository ??= new AnnouncementRepository(DatabaseContext);
			return _announcementRepository;
		}
	}

	private INoteRepository _noteRepository;

	public INoteRepository NoteRepository
	{
		get
		{
			_noteRepository ??= new NoteRepository(DatabaseContext);
			return _noteRepository;
		}
	}

	private IFavoriteRepository favoriteRepository;

	public IFavoriteRepository FavoriteRepository
	{
		get
		{
			favoriteRepository ??= new FavoriteRepository(DatabaseContext);
			return favoriteRepository;
		}
	}

	private IDeleteReasonRepository _deleteReasonRepository;

	public IDeleteReasonRepository DeleteReasonRepository
	{
		get
		{
			_deleteReasonRepository ??= new DeleteReasonRepository(DatabaseContext);
			return _deleteReasonRepository;
		}
	}

	private INeedToEditReasonRepository _needToEditReasonRepository;

	public INeedToEditReasonRepository NeedToEditReasonRepository
	{
		get
		{
			_needToEditReasonRepository ??= new NeedToEditReasonRepository(DatabaseContext);
			return _needToEditReasonRepository;
		}
	}

	private INeedToEditLogRepository _needToEditLogRepository;

	public INeedToEditLogRepository NeedToEditLogRepository
	{
		get
		{
			_needToEditLogRepository ??= new NeedToEditLogRepository(DatabaseContext);
			return _needToEditLogRepository;
		}
	}

	private IReportReasonRepository _reportReasonRepository;

	public IReportReasonRepository ReportReasonRepository
	{
		get
		{
			_reportReasonRepository ??= new ReportReasonRepository(DatabaseContext);
			return _reportReasonRepository;
		}
	}

	private IReportLogRepository _reportLogRepository;

	public IReportLogRepository ReportLogRepository
	{
		get
		{
			_reportLogRepository ??= new ReportLogRepository(DatabaseContext);
			return _reportLogRepository;
		}
	}

	private IDictionaryCheckerRepository _dictionaryCheckerRepository;

	public IDictionaryCheckerRepository DictionaryCheckerRepository
	{
		get
		{
			_dictionaryCheckerRepository ??= new DictionaryCheckerRepository(DatabaseContext);
			return _dictionaryCheckerRepository;
		}
	}

	public IFieldMultiValueRepository FieldMultiValueRepository
	{
		get
		{
			_fieldMultiValueRepository ??= new FieldMultiValueRepository(DatabaseContext);
			return _fieldMultiValueRepository;
		}
	}

	private IFieldValueAnnouncementRepository _fieldValueAnnouncementRepository;

	public IFieldValueAnnouncementRepository FieldValueAnnouncementRepository
	{
		get
		{
			_fieldValueAnnouncementRepository ??= new FieldValueAnnouncementRepository(DatabaseContext);
			return _fieldValueAnnouncementRepository;
		}
	}

	private IRegionRepository _regionRepository;
	public IRegionRepository RegionRepository
	{
		get
		{
			_regionRepository ??= new RegionRepository(DatabaseContext);
			return _regionRepository;
		}
	}

	private IPhoneOperatorRepository _phoneOperatorRepository;
	public IPhoneOperatorRepository PhoneOperatorRepository
	{
		get
		{
			_phoneOperatorRepository ??= new PhoneOperatorRepository(DatabaseContext);
			return _phoneOperatorRepository;
		}
	}

	private IPlateCodeRepository _plateCodeRepository;
	public IPlateCodeRepository PlateCodeRepository
	{
		get
		{
			_plateCodeRepository ??= new PlateCodeRepository(DatabaseContext);
			return _plateCodeRepository;
		}
	}

	private IPlateStatusRepository _plateStatusRepository;
	public IPlateStatusRepository PlateStatusRepository
	{
		get
		{
			_plateStatusRepository ??= new PlateStatusRepository(DatabaseContext);
			return _plateStatusRepository;
		}
	}

	private IAnnouncementViewsRepository announcementViewsRepository;
	public IAnnouncementViewsRepository AnnouncementViewsRepository
	{
		get
		{
			announcementViewsRepository ??= new AnnouncementViewsRepository(DatabaseContext);
			return announcementViewsRepository;
		}
	}
	
	private IContactUsRepository _contactUsRepository;
	
	public IContactUsRepository ContactUsRepository
	{
		get
		{
			_contactUsRepository ??= new ContactUsRepository(DatabaseContext);
			return _contactUsRepository;
		}
	}
	
	
	// **************************************************
	private ICaptchaHistoryRepository _captchaHistoryRepository;

	public ICaptchaHistoryRepository CaptchaHistoryRepository
	{
		get
		{
			_captchaHistoryRepository ??= new CaptchaHistoryRepository(DatabaseContext);
			return _captchaHistoryRepository;
		}
	}
	// **************************************************
}
using Domain;
using ESH.Constant.Attachment.Announcement;
using DynamicFields.Models;
using DynamicFields.Seed;
using Microsoft.Extensions.Configuration;
using ESH.BuildingBlocks.Attachments.Abstraction;
using ESH.BuildingBlocks.Attachments.Contract;
using ESH.BuildingBlocks.Localization.Abstraction;
using ESH.BuildingBlocks.Localization.Contract;
using ESH.BuildingBlocks.SubSystem.Contract;
using ESH.SeedworkSystem.Domain.Attachment;
using ESH.SeedworkSystem.Domain.MultiLanguage;
using Infrastructure.BuildingBlocks.LookupSeeds.Status;
using Infrastructure.BuildingBlocks.LookupSeeds.Category;
using Infrastructure.BuildingBlocks.LookupSeeds.PlateCode;
using Infrastructure.BuildingBlocks.LookupSeeds.PlateStatus;
using Infrastructure.BuildingBlocks.LookupSeeds.CommonReason;
using Infrastructure.BuildingBlocks.LookupSeeds.CategoryType;
using Infrastructure.BuildingBlocks.LookupSeeds.AttachmentSubject;
using FieldType = Domain.FieldType;
using IUnitOfWork = Persistence.IUnitOfWork;
using Region = Domain.Region;

namespace Infrastructure;

public class InitialData : object
{
	#region Settings

	public InitialData(
		IConfiguration configuration,
		IUnitOfWork unitOfWork, ISubSystemManager subSystemManager,
		ILanguageLocalizerManager languageLocalizerManager,
		ILanguageCodeManager languageCodeManager,
		IAttachmentSubjectManager attachmentSubjectManager,
		ILanguageService languageService,
		IAttachmentService attachmentService) : base()
	{
		SubSystemManager = subSystemManager;
		LanguageCodeManager = languageCodeManager;
		LanguageLocalizerManager = languageLocalizerManager;
		AttachmentSubjectManager = attachmentSubjectManager;
		LanguageService = languageService;
		AttachmentService = attachmentService;

		UnitOfWork = unitOfWork ?? throw new ArgumentNullException(paramName: nameof(unitOfWork));
		Configuration = configuration ?? throw new ArgumentNullException(paramName: nameof(configuration));
	}

	private IUnitOfWork UnitOfWork { get; }
	private IConfiguration Configuration { get; }
	private ILanguageService LanguageService { get; }
	private ISubSystemManager SubSystemManager { get; }
	private IAttachmentService AttachmentService { get; }
	private ILanguageCodeManager LanguageCodeManager { get; }
	private ILanguageLocalizerManager LanguageLocalizerManager { get; }
	private IAttachmentSubjectManager AttachmentSubjectManager { get; }

	#endregion /Settings

	#region Constants

	private const string languageCodeOman = "ar-OM";
	private const string languageCodeEnglishUs = "en-US";

	#endregion /Constants

	#region Status

	public async Task CreateStatusAsync()
	{
		var subSystem =
			await SubSystemManager
				.FindByNameAsync(domain: nameof(Status));

		if (subSystem is null)
		{
			throw new NullReferenceException(message: nameof(subSystem));
		}

		var languageCodeEntityOman =
			await LanguageCodeManager.FindLanguageByCodeAsync(code: languageCodeOman);

		if (languageCodeEntityOman is null)
		{
			throw new NullReferenceException(message: nameof(languageCodeEntityOman));
		}

		var languageCodeEntityEnglishUs =
			await LanguageCodeManager
				.FindLanguageByCodeAsync(code: languageCodeEnglishUs);

		if (languageCodeEntityEnglishUs is null)
		{
			throw new NullReferenceException(message: nameof(languageCodeEntityEnglishUs));
		}

		var statusEnumerable =
			await UnitOfWork.StatusRepository.GetAllAsync();

		var existingStatuses = statusEnumerable.ToList();

		var existingLocalizers =
			await LanguageLocalizerManager.FindBySubSystemIdAsync(subSystemId: subSystem.Id);

		var seedData = new StatusSeedData();

		foreach (var seed in seedData.Data)
		{
			var statusEntity = existingStatuses
				.FirstOrDefault(predicate: s => s != null && s.Code == seed.Code);

			if (statusEntity is null)
			{
				statusEntity = new Status(code: seed.Code)
				{
					Description = $"{seed.ArOm} | {seed.EnUs}"
				};

				await UnitOfWork.StatusRepository.AddAsync(entity: statusEntity);
				existingStatuses.Add(item: statusEntity);
			}

			var hasLocalizer =
				existingLocalizers.Any(predicate: l => l.RelationId == statusEntity.Id);

			if (hasLocalizer == false)
			{
				var localizers = new List<LanguageLocalizer>
				{
					new(
						subSystemId: subSystem.Id,
						relationId: statusEntity.Id,
						propertyName: Status.TitleProperty,
						value: seed.ArOm, languageCodeId: languageCodeEntityOman.Id),
					new(
						subSystemId: subSystem.Id,
						relationId: statusEntity.Id,
						propertyName: Status.TitleProperty,
						value: seed.EnUs, languageCodeId: languageCodeEntityEnglishUs.Id)
				};

				await LanguageLocalizerManager.AddRangeAsync(localizers: localizers);
				existingLocalizers.AddRange(collection: localizers);
			}
		}

		await UnitOfWork.SaveAsync();
	}

	#endregion /Status

	#region ReportReason

	public async Task CreateReportReasonAsync()
	{
		var subSystem =
			await SubSystemManager
				.FindByNameAsync(domain: nameof(ReportReason));

		if (subSystem is null)
		{
			throw new NullReferenceException(message: nameof(subSystem));
		}

		var languageCodeEntityOman =
			await LanguageCodeManager.FindLanguageByCodeAsync(code: languageCodeOman);

		if (languageCodeEntityOman is null)
		{
			throw new NullReferenceException(message: nameof(languageCodeEntityOman));
		}

		var languageCodeEntityEnglishUs =
			await LanguageCodeManager.FindLanguageByCodeAsync(code: languageCodeEnglishUs);

		if (languageCodeEntityEnglishUs is null)
		{
			throw new NullReferenceException(message: nameof(languageCodeEntityEnglishUs));
		}

		var reportReasonEnumerable =
			await UnitOfWork.ReportReasonRepository.GetAllAsync();

		var existingReasons = reportReasonEnumerable.ToList();

		var existingLocalizers =
			await LanguageLocalizerManager.FindBySubSystemIdAsync(subSystemId: subSystem.Id);

		var seedData = new CommonReasonSeedData();

		var listSeed = seedData.Data
			.Where(predicate: s => s.Type == ReasonType.Report);

		foreach (var seed in listSeed)
		{
			var reasonEntity =
				existingReasons.FirstOrDefault(predicate: r => r!.Code == seed.Code);

			if (reasonEntity is null)
			{
				reasonEntity = new ReportReason(code: seed.Code)
				{
					Ordering = seed.Code,
					HasDescription = seed.HasDescription,
					Description = $"{seed.ArOm} | {seed.EnUs}"
				};

				await UnitOfWork.ReportReasonRepository.AddAsync(entity: reasonEntity);
				existingReasons.Add(item: reasonEntity);
			}

			var hasLocalizer = existingLocalizers
				.Any(predicate: l => l.RelationId == reasonEntity.Id);

			if (hasLocalizer == false)
			{
				var localizers = new List<LanguageLocalizer>
				{
					new(
						subSystemId: subSystem.Id,
						relationId: reasonEntity.Id,
						propertyName: ReportReason.TextPropertyLocalizer,
						value: seed.ArOm,
						languageCodeId: languageCodeEntityOman.Id),
					new(
						subSystemId: subSystem.Id,
						relationId: reasonEntity.Id,
						propertyName: ReportReason.TextPropertyLocalizer,
						value: seed.EnUs,
						languageCodeId: languageCodeEntityEnglishUs.Id)
				};

				await LanguageLocalizerManager.AddRangeAsync(localizers: localizers);
				existingLocalizers.AddRange(collection: localizers);
			}

			await UnitOfWork.SaveAsync();
		}
	}

	#endregion /ReportReason

	#region DeleteReason

	public async Task CreateDeletedReasonAsync()
	{
		var subSystem =
			await SubSystemManager
				.FindByNameAsync(domain: nameof(DeleteReason));

		if (subSystem is null)
		{
			throw new NullReferenceException(message: nameof(subSystem));
		}

		var languageCodeEntityOman =
			await LanguageCodeManager
				.FindLanguageByCodeAsync(code: languageCodeOman);

		if (languageCodeEntityOman is null)
		{
			throw new NullReferenceException(message: nameof(languageCodeEntityOman));
		}

		var languageCodeEntityEnglishUs =
			await LanguageCodeManager
				.FindLanguageByCodeAsync(code: languageCodeEnglishUs);

		if (languageCodeEntityEnglishUs is null)
		{
			throw new NullReferenceException(message: nameof(languageCodeEntityEnglishUs));
		}

		// preload reasons
		var reasonEnumerable =
			await UnitOfWork.DeleteReasonRepository.GetAllAsync();

		var existingReasons = reasonEnumerable.ToList();

		var existingLocalizers =
			await LanguageLocalizerManager
				.FindBySubSystemIdAsync(subSystemId: subSystem.Id);

		var seedData = new CommonReasonSeedData();

		var list =
			seedData.Data
				.Where(predicate: s => s.Type == ReasonType.UserDeleted)
				.ToList();

		foreach (var seed in list)
		{
			var reasonEntity = existingReasons
				.FirstOrDefault(predicate: r => r!.Code == seed.Code);

			if (reasonEntity is null)
			{
				reasonEntity = new DeleteReason(code: seed.Code)
				{
					Ordering = seed.Code,

					HasDescription = seed.HasDescription,
					Description = $"{seed.ArOm} | {seed.EnUs}",
				};

				await UnitOfWork.DeleteReasonRepository.AddAsync(entity: reasonEntity);
				existingReasons.Add(item: reasonEntity);
			}

			var hasLocalizer = existingLocalizers
				.Any(predicate: l => l.RelationId == reasonEntity.Id);

			if (hasLocalizer == false)
			{
				var localizers = new List<LanguageLocalizer>
				{
					new(
						subSystemId: subSystem.Id,
						relationId: reasonEntity.Id,
						propertyName: DeleteReason.TextPropertyLocalizer,
						value: seed.ArOm, languageCodeId: languageCodeEntityOman.Id),
					new(
						subSystemId: subSystem.Id,
						relationId: reasonEntity.Id,
						propertyName: DeleteReason.TextPropertyLocalizer,
						value: seed.EnUs, languageCodeId: languageCodeEntityEnglishUs.Id)
				};

				await LanguageLocalizerManager.AddRangeAsync(localizers: localizers);
				existingLocalizers.AddRange(collection: localizers);
			}

			await UnitOfWork.SaveAsync();
		}
	}

	#endregion /DeleteReason

	#region NeedToEditReason

	public async Task CreateNeedToEditReasonAsync()
	{
		var subSystem =
			await SubSystemManager
				.FindByNameAsync(domain: nameof(NeedToEditReason));

		if (subSystem is null)
		{
			throw new NullReferenceException(message: nameof(subSystem));
		}

		var languageCodeEntityOman =
			await LanguageCodeManager
				.FindLanguageByCodeAsync(code: languageCodeOman);

		if (languageCodeEntityOman is null)
		{
			throw new NullReferenceException(message: nameof(languageCodeEntityOman));
		}

		var languageCodeEntityEnglishUs =
			await LanguageCodeManager.FindLanguageByCodeAsync(code: languageCodeEnglishUs);

		if (languageCodeEntityEnglishUs is null)
		{
			throw new NullReferenceException(message: nameof(languageCodeEntityEnglishUs));
		}

		// preload reasons
		var reasonEnumerable =
			await UnitOfWork.NeedToEditReasonRepository.GetAllAsync();

		var existingReasons = reasonEnumerable.ToList();

		var existingLocalizers =
			await LanguageLocalizerManager
				.FindBySubSystemIdAsync(subSystemId: subSystem.Id);

		var seedData = new CommonReasonSeedData();

		foreach (var seed in seedData.Data
					 .Where(predicate: s => s.Type == ReasonType.Edit))
		{
			var reasonEntity = existingReasons
				.FirstOrDefault(predicate: r => r!.Code == seed.Code);

			if (reasonEntity is null)
			{
				reasonEntity = new NeedToEditReason(code: seed.Code)
				{
					Ordering = seed.Code,
					Description = $"{seed.ArOm} | {seed.EnUs}",
					HasDescription = seed.HasDescription,
				};

				await UnitOfWork.NeedToEditReasonRepository.AddAsync(entity: reasonEntity);

				existingReasons.Add(item: reasonEntity);
			}

			var hasLocalizer = existingLocalizers
				.Any(predicate: l => l.RelationId == reasonEntity.Id);

			if (hasLocalizer == false)
			{
				var localizers = new List<LanguageLocalizer>
				{
					new(
						subSystemId: subSystem.Id,
						relationId: reasonEntity.Id,
						propertyName: NeedToEditReason.TextPropertyLocalizer,
						value: seed.ArOm, languageCodeId: languageCodeEntityOman.Id),
					new(
						subSystemId: subSystem.Id,
						relationId: reasonEntity.Id,
						propertyName: NeedToEditReason.TextPropertyLocalizer,
						value: seed.EnUs, languageCodeId: languageCodeEntityEnglishUs.Id)
				};

				await LanguageLocalizerManager.AddRangeAsync(localizers: localizers);
				existingLocalizers.AddRange(collection: localizers);
			}

			await UnitOfWork.SaveAsync();
		}
	}

	#endregion /NeedToEditReason

	#region CategoryType

	public async Task CreateCategoryTypeAsync()
	{
		var categoryEnumerable =
			await UnitOfWork.CategoryTypeRepository.GetAllAsync();

		var existingCategories = categoryEnumerable.ToList();

		var seedData = new CategoryTypeSeedData();

		foreach (var seed in seedData.Data)
		{
			var categoryEntity =
				existingCategories.FirstOrDefault(predicate: c => c!.Code == seed.Code);

			if (categoryEntity is null)
			{
				categoryEntity =
					new CategoryType(code: seed.Code, hasAccessToChild: seed.HasAccessToChild);

				await UnitOfWork.CategoryTypeRepository.AddAsync(entity: categoryEntity);
				existingCategories.Add(item: categoryEntity);
			}
		}

		await UnitOfWork.SaveAsync();
	}

	#endregion /CategoryType

	#region AttachmentSubject

	public async Task CreateAttachmentSubjectAsync()
	{
		var getAll =
			await AttachmentSubjectManager.GetAllAsync();

		var existing =
			getAll.ToDictionary(keySelector: x => x.Code, comparer: StringComparer.OrdinalIgnoreCase);

		var seedData = new AttachmentSubjectSeedData();

		foreach (var seed in seedData.Data)
		{
			if (existing.ContainsKey(key: seed.Code) == true)
			{
				continue;
			}

			var entity = new AttachmentSubject(
				code: seed.Code,
				codeDisplay: seed.DisplayName);

			await AttachmentSubjectManager.AddAsync(entity: entity);

			existing.Add(key: seed.Code, value: entity);
		}

		await UnitOfWork.SaveAsync();
	}

	#endregion /AttachmentSubject

	#region FieldType

	public async Task CreateFieldTypeAsync()
	{
		var entities =
			await UnitOfWork.FieldTypeRepository.GetAllAsync();

		var existing =
			entities.ToDictionary(keySelector: x => x!.Code, comparer: StringComparer.OrdinalIgnoreCase);

		// جمع‌آوری تمام فیلدها از تمام کلاس‌های سید دیتا
		// Collect all fields from all seed data classes
		var allSeeds = new List<FieldSeedModel>();

		allSeeds.AddRange(new FieldPlateSeedData().Data);
		allSeeds.AddRange(new FieldPhoneSeedData().Data);
		allSeeds.AddRange(new FieldPropertySeedData().Data);
		allSeeds.AddRange(new FieldOtherSeedData().Data);

		// حذف تکراری‌ها بر اساس Code
		var uniqueSeeds = allSeeds
			.GroupBy(x => x.Code, StringComparer.OrdinalIgnoreCase)
			.Select(g => g.First())
			.ToList();

		foreach (var seed in uniqueSeeds)
		{
			if (existing.ContainsKey(key: seed.Code) == true)
			{
				continue;
			}

			var entity = new FieldType(code: seed.Code)
			{
				DataType = seed.DataType,
				JsonConfig = seed.JsonConfig,
			};

			await UnitOfWork.FieldTypeRepository.AddAsync(entity: entity);

			var valueLocalizers = new List<ValueLocalizer>()
			{
				new(Value: seed.TitleAr, LanguageCode: languageCodeOman),
				new(Value: seed.TitleEn, LanguageCode: languageCodeEnglishUs),
			};

			existing.Add(key: seed.Code, value: entity);
		}

		await UnitOfWork.SaveAsync();
	}

	#endregion /FieldType

	#region PlateCode

	public async Task CreatePlateCodeAsync()
	{
		var entities =
			await UnitOfWork.PlateCodeRepository.GetAllAsync();

		var existing =
			entities.ToDictionary(keySelector: x =>
				$"{x!.TypeCode}-{x.EnUs}-{x.ArOm}", comparer: StringComparer.OrdinalIgnoreCase);

		var seedData = new PlateCodeSeedData();

		foreach (var seed in seedData.Data)
		{
			var code = $"{seed.TypeCode}-{seed.EnUs}-{seed.ArOm}";

			if (existing.ContainsKey(key: code) == true)
			{
				continue;
			}

			var entity =
				new PlateCode(
					typeCode: seed.TypeCode,
					arOm: seed.ArOm,
					enUs: seed.EnUs);

			await UnitOfWork.PlateCodeRepository.AddAsync(entity: entity);

			existing.Add(key: code, value: entity);
		}

		await UnitOfWork.SaveAsync();
	}

	#endregion /PlateCode

	#region PhoneOperator

	public async Task CreatePhoneOperatorAsync()
	{
		var entities =
			await UnitOfWork.PhoneOperatorRepository.GetAllAsync();

		var existing = entities.ToDictionary(
			keySelector: x => x!.Code,
			comparer: StringComparer.OrdinalIgnoreCase);

		var seedData = new PhoneOperatorSeedData();

		foreach (var seed in seedData.Data)
		{
			if (existing.ContainsKey(key: seed.Code) == true)
			{
				continue;
			}

			var entity = new PhoneOperator(code: seed.Code)
			{
				Ordering = seed.Ordering,
				Prefix = string.Join(separator: ',', values: seed.Prefixes),
			};

			await UnitOfWork.PhoneOperatorRepository.AddAsync(entity: entity);

			var valueLocalizers = new List<ValueLocalizer>()
			{
				new(Value: seed.NameAr, LanguageCode: languageCodeOman),
				new(Value: seed.NameEn, LanguageCode: languageCodeEnglishUs),
			};

			var owner = new LocalizationOwner(
				SubSystemName: nameof(PhoneOperator),
				RelationId: entity.Id,
				PropertyName: PhoneOperator.NamePropertyLocalizer);

			await LanguageService.AddAsync(owner, localizers: valueLocalizers);

			var ownerAttachmentLarge =
				new AttachmentOwner(
					SubSystemName: nameof(PhoneOperator),
					RelationId: entity.Id,
					ServerId: Domain.Base.ServerKeyConstant.Key,
					SubjectCode: AnnouncementAttachmentSubjectKeys.PhoneOperator);

			await AttachmentService.SyncAsync(
				owner: ownerAttachmentLarge, fileOriginalName: seed.ImageNameOnAttachmentServer,
				replacePolicy: AttachmentReplacePolicy.ReplaceAll);

			existing.Add(key: seed.Code, value: entity);
		}

		await UnitOfWork.SaveAsync();
	}

	#endregion /PhoneOperator

	#region Region

	public async Task CreateRegionAsync()
	{
		var entities =
			await UnitOfWork.RegionRepository.GetAllAsync();

		var existing =
			entities.ToDictionary(keySelector: x => x!.Code, comparer: StringComparer.OrdinalIgnoreCase);

		var seedData = new RegionSeedData();

		foreach (var root in seedData.Data)
		{
			await CreateRegionRecursive(
				seed: root,
				parent: null,
				existing: existing!);
		}

		await UnitOfWork.SaveAsync();
	}

	private async Task CreateRegionRecursive(
		RegionSeedModel seed,
		Region? parent,
		IDictionary<string, Region> existing)
	{
		if (existing.TryGetValue(key: seed.Code, value: out var entity) == false)
		{
			entity = new Region(code: seed.Code)
			{
				Parent = parent
			};

			await UnitOfWork.RegionRepository.AddAsync(entity: entity);
			existing.Add(key: seed.Code, value: entity);

			List<ValueLocalizer> valueLocalizers =
			[
				new(Value: seed.NameAr, LanguageCode: languageCodeOman),
				new(Value: seed.NameEn, LanguageCode: languageCodeEnglishUs)
			];

			var owner = new LocalizationOwner(
				SubSystemName: nameof(Region),
				RelationId: entity.Id,
				PropertyName: Region.PropertyNameKey);

			await LanguageService.AddAsync(owner, localizers: valueLocalizers);
		}

		if (seed.Children.Count == 0)
		{
			return;
		}

		foreach (var child in seed.Children)
		{
			await CreateRegionRecursive(
				seed: child,
				parent: entity,
				existing: existing
			);
		}
	}

	#endregion /Region

	#region PlateStatus

	public async Task CreatePlateStatusAsync()
	{
		var entities =
			await UnitOfWork.PlateStatusRepository.GetAllAsync();

		var existing = entities.ToDictionary(
			keySelector: x => x!.Code,
			comparer: StringComparer.OrdinalIgnoreCase);

		var seedData = new PlateStatusSeedData();

		foreach (var seed in seedData.Data)
		{
			if (existing.ContainsKey(key: seed.Code) == true)
			{
				continue;
			}

			var entity =
				new PlateStatus(code: seed.Code, isDefault: seed.IsDefault)
				{
					Description = $"{seed.ArOm} | {seed.EnUs}"
				};

			await UnitOfWork.PlateStatusRepository.AddAsync(entity: entity);

			existing.Add(key: seed.Code, value: entity);

			var valueLocalizers = new List<ValueLocalizer>
			{
				new(Value: seed.ArOm, LanguageCode: languageCodeOman),
				new(Value: seed.EnUs, LanguageCode: languageCodeEnglishUs)
			};

			var owner = new LocalizationOwner(
				SubSystemName: nameof(PlateStatus),
				RelationId: entity.Id,
				PropertyName: PlateStatus.PropertyNameKey);

			await LanguageService.AddAsync(localizationOwner: owner, localizers: valueLocalizers);
		}
	}

	#endregion /PlateStatus

	#region Category

	public async Task CreateCategoryAsync()
	{
		var categoryTypeEnumerable =
			await UnitOfWork.CategoryTypeRepository.GetAllAsync();

		var existingCategoryTypes =
			categoryTypeEnumerable.ToDictionary(
				keySelector: x => x!.Code,
				comparer: StringComparer.OrdinalIgnoreCase);

		var categoryEnumerable =
			await UnitOfWork.CategoryRepository.GetAllAsync();
		
		var existingCategories =
			categoryEnumerable
				.Where(x => string.IsNullOrEmpty(x!.Code) == false)
					.ToDictionary(
						keySelector: x => x!.Code ?? string.Empty,
						comparer: StringComparer.OrdinalIgnoreCase);

		var seedData = new CategorySeedData();

		foreach (var seed in seedData.Data)
		{
			if (existingCategoryTypes
					.TryGetValue(key: seed.CategoryTypeCode, value: out var categoryType) == false)
			{
				var errorMessage = string.Format(
					format: ESH.Resources.Messages.NoRulesError, arg0: seed.CategoryTypeCode);

				throw new InvalidOperationException(message: errorMessage);
			}

			if (existingCategories
					.TryGetValue(key: seed.Code, value: out var categoryEntity) == false)
			{
				categoryEntity = new Category
				{
					Code = seed.Code,
					Ordering = seed.Ordering,
					CategoryTypeId = categoryType!.Id,
					Description = $"{seed.NameAr} | {seed.NameEn}"
				};

				await UnitOfWork.CategoryRepository.AddAsync(entity: categoryEntity);
				existingCategories.Add(key: seed.Code, value: categoryEntity);

				var valueLocalizers = new List<ValueLocalizer>
				{
					new(Value: seed.NameAr, LanguageCode: languageCodeOman),
					new(Value: seed.NameEn, LanguageCode: languageCodeEnglishUs)
				};

				var owner = new LocalizationOwner(
					SubSystemName: nameof(Category),
					RelationId: categoryEntity.Id,
					PropertyName: Category.PropertyNameKey);

				await LanguageService.AddAsync(localizationOwner: owner, localizers: valueLocalizers);

				var ownerAttachmentLarge =
					new AttachmentOwner(
						SubSystemName: nameof(Category),
						RelationId: categoryEntity.Id,
						ServerId: Domain.Base.ServerKeyConstant.Key,
						SubjectCode: AnnouncementAttachmentSubjectKeys.CategoryImageLarge);

				await AttachmentService.SyncAsync(
					owner: ownerAttachmentLarge, fileOriginalName: seed.FileNameLarge,
					replacePolicy: AttachmentReplacePolicy.ReplaceAll);

				var ownerAttachmentSmall =
					new AttachmentOwner(
						SubSystemName: nameof(Category),
						RelationId: categoryEntity.Id,
						ServerId: Domain.Base.ServerKeyConstant.Key,
						SubjectCode: AnnouncementAttachmentSubjectKeys.CategoryImageSmall);

				await AttachmentService.SyncAsync(
					owner: ownerAttachmentSmall,
					fileOriginalName: seed.FileNameSmall, replacePolicy: AttachmentReplacePolicy.ReplaceAll);
			}
			else
			{
				categoryEntity!.Ordering = seed.Ordering;
				categoryEntity.CategoryTypeId = categoryType!.Id;
			}
		}

		await UnitOfWork.SaveAsync();

		foreach (var seed in seedData.Data)
		{
			if (string.IsNullOrWhiteSpace(value: seed.ParentCode) == true)
			{
				continue;
			}

			if (existingCategories
					.TryGetValue(key: seed.Code, value: out var childCategory) == false)
			{
				continue;
			}

			if (existingCategories
					.TryGetValue(key: seed.ParentCode, value: out var parentCategory) == false)
			{
				var errorMessage =
					$"Parent category with code '{seed.ParentCode}' for '{seed.Code}' was not found.";

				throw new InvalidOperationException(message: errorMessage);
			}

			childCategory!.ParentId = parentCategory!.Id;
		}

		await UnitOfWork.SaveAsync();
	}

	#endregion /Category

	#region Field

	public async Task CreateFieldAsync()
	{
		var categories =
			await UnitOfWork.CategoryRepository.GetAllWithCodeAsync();

		if (categories.Any() == false)
		{
			throw new InvalidOperationException
				(message: "No categories found. Please create categories before creating fields.");
		}

		foreach (var category in categories)
		{
			List<Field> fields = await UnitOfWork
				.FieldRepository.GetByCategoryIdAsync(categoryId: category.Id);

			if (fields.Any() == false)
			{
				var provider =
					new CategoryTypeFieldProvider();

				var fieldData =
					provider.GetFieldsForCategoryType(categoryType: category.CategoryType!.Code);

				var codes =
					fieldData.Select(selector: fd => fd.Code).ToList();

				var fieldTypes = await UnitOfWork
					.FieldTypeRepository.GetByCodesAsync(codes: codes);

				if (fieldTypes.Any() == false)
				{
					throw new InvalidOperationException(
						message:
						$"No field types found for category '{category.Code}'. Please create field types before creating fields.");
				}

				foreach (var item in fieldData)
				{
					if (item.IsField == false)
					{
						continue;
					}

					var fieldType = fieldTypes
						.First(predicate: ft => ft.Code == item.Code);

					var newField = new Field
					{
						FieldTypeId = fieldType.Id,
						CategoryId = category.Id,

						IsRequired = CategoryFieldHelper
							.IsFieldRequired(fieldTypeCode: fieldType.Code, categoryType: category.CategoryType.Code),

						Ordering = CategoryFieldHelper
							.GetOrdering(fieldTypeCode: fieldType.Code, categoryType: category.CategoryType.Code),

						JsonConfig = item.JsonConfig,
						ConfigVersion = item.Version,
					};

					await UnitOfWork.FieldRepository.AddAsync(entity: newField);

					#region Name In Localizer
					var nameLanguageLocalizers = new List<ValueLocalizer>()
					{
						new(Value: item.TitleAr, LanguageCode: languageCodeOman),
						new(Value: item.TitleEn, LanguageCode: languageCodeEnglishUs),
					};

					var titleOwner = new LocalizationOwner(
						SubSystemName: nameof(Field),
						RelationId: newField.Id,
						PropertyName: Field.NamePropertyLocalizer
					);

					await LanguageService
						.AddAsync(titleOwner, localizers: nameLanguageLocalizers);
					#endregion /Name In Localizer

					#region Hint In Localizer
					var hintLanguageLocalizers = new List<ValueLocalizer>()
					{
						new(Value: item.HintAr, LanguageCode: languageCodeOman),
						new(Value: item.HintEn, LanguageCode: languageCodeEnglishUs),
					};

					var hintOwner = new LocalizationOwner(
						SubSystemName: nameof(Field),
						RelationId: newField.Id,
						PropertyName: Field.HintPropertyLocalizer
					);

					await LanguageService
						.AddAsync(localizationOwner: hintOwner, localizers: hintLanguageLocalizers);
					#endregion /Hint In Localizer

					#region Description In Localizer
					var descriptionLanguageLocalizers = new List<ValueLocalizer>()
					{
						new(Value: item.HintAr, LanguageCode: languageCodeOman),
						new(Value: item.HintEn, LanguageCode: languageCodeEnglishUs),
					};

					var descriptionOwner = new LocalizationOwner(
						SubSystemName: nameof(Field),
						RelationId: newField.Id,
						PropertyName: Field.DescriptionPropertyLocalizer
					);

					await LanguageService
						.AddAsync(localizationOwner: descriptionOwner, localizers: descriptionLanguageLocalizers);
					#endregion /Description In Localizer
				}
			}
		}

		await UnitOfWork.SaveAsync();
	}

	#endregion /Field

	#region RunQuery

	public Task RunQuery()
	{
		return Task.CompletedTask;
	}

	#endregion /RunQuery
}
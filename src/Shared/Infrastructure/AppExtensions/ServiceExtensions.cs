using AutoMapper;
using System.Reflection;
using Domain.Base;
using ESH.BuildingBlocks.ActionCodeGuard.Abstraction;
using ESH.BuildingBlocks.ActionCodeGuard.Utilities;
using ESH.BuildingBlocks.Sedding;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using ESH.BuildingBlocks.Attachments.Abstraction;
using ESH.BuildingBlocks.Localization.Abstraction;
using ESH.BuildingBlocks.SubSystem.Contract;
using ESH.Enums.Shared;
using ESH.SeedworkSystem.Domain.MultiLanguage;
using ESH.SeedworkSystem.Domain.SubSystem;
using Microsoft.Extensions.DependencyInjection;
using ESH.ViewModels.Shared;
using BaseEntity = ESH.SeedworkSystem.Domain.BaseEntity;

namespace Infrastructure.AppExtensions;

public static class ServiceExtensions
{
	public static async Task AddStartupTasks(this WebApplication app, Assembly assembly)
	{
		using (var scope = app.Services.CreateScope())
		{
			var mapper =
				scope.ServiceProvider.GetRequiredService<IMapper>();

			var subSystemManager =
				scope.ServiceProvider.GetRequiredService<ISubSystemManager>();

			var languageCodeManager =
				scope.ServiceProvider.GetRequiredService<ILanguageCodeManager>();

			var languageLocalizerManager =
				scope.ServiceProvider.GetRequiredService<ILanguageLocalizerManager>();

			var unitOfWork =
				scope.ServiceProvider.GetRequiredService<Persistence.IUnitOfWork>();

			var attachmentSubjectManager =
				scope.ServiceProvider.GetRequiredService<IAttachmentSubjectManager>();

			var languageService =
				scope.ServiceProvider.GetRequiredService<ILanguageService>();

			var attachmentService =
				scope.ServiceProvider.GetRequiredService<IAttachmentService>();

			var subSystemHttpService =
				scope.ServiceProvider.GetRequiredService
					<ESH.HttpServices.Abstraction.ProjectManager.ISubSystemHttpService>();

			var actionService =
				scope.ServiceProvider.GetRequiredService<IActionHttpService>();

			// await unitOfWork!.DatabaseEnsureCreatedAsync();

			// Domains
			var domains =
				BaseEntity.DomainFinder(assemblyName: nameof(Domain));

			await subSystemManager.AddByNamesAsync(
				domains: domains, serverId: ServerKeyConstant.Key);

			await unitOfWork.SaveAsync();

			List<SubSystem?> subSystems =
				await subSystemManager.GetAllAsync();

			var subSystemViewModels =
				mapper.Map<List<SubSystemResponseViewModel>>(source: subSystems);

			var result = await subSystemHttpService.AddAsync(
				model: subSystemViewModels,
				projectType: ProjectType.Announcement,
				serverId: ServerKeyConstant.Key);

			if (result!.IsFailed == true)
			{
				throw new Exception(message: string.Join(separator: ',', values: result.Errors));
			}

			var actions =
				ActionScanner.ScanCodedActionsOnly(
					assembly: assembly, serverKey: ServerKeyConstant.Key);

			var resultSaveAction = await actionService.AddAsync(
				model: actions, serverId: ServerKeyConstant.Key,
				projectType: ProjectType.Announcement);

			if (resultSaveAction!.IsFailed == true)
			{
				throw new Exception(message: string.Join(separator: ',', values: resultSaveAction.Errors));
			}

			// create language codes
			// **************************************************
			List<LanguageCode> languageCodes =
				LanguageCodeInitializer.GetLanguageCodeModels();

			foreach (var item in languageCodes)
			{
				var search =
					await languageCodeManager.FindLanguageByCodeAsync(code: item.Code);

				if (search is null)
				{
					await languageCodeManager.AddAsync(item: item);
				}
				else
				{
					search.Name = item.Name;
					search.IsRtl = item.IsRtl;
					search.IsDefault = item.IsDefault;
					search.Description = item.Description;
				}
			}

			await unitOfWork.SaveAsync();
			// **************************************************
			// Initial Data Seeding
			var initialData =
				new InitialData(
					configuration: app.Configuration, unitOfWork: unitOfWork,
					subSystemManager: subSystemManager, languageLocalizerManager: languageLocalizerManager,
					languageCodeManager: languageCodeManager, attachmentSubjectManager: attachmentSubjectManager,
					languageService: languageService, attachmentService: attachmentService);
			// **************************************************

			// **************************************************
			var seedInitial = app.Configuration.GetValue<bool>(key: "SeedInitial");

			if (seedInitial == true)
			{
				// call function in initialData class file!
				await initialData.CreateStatusAsync();
				await initialData.CreateReportReasonAsync();
				await initialData.CreateDeletedReasonAsync();
				await initialData.CreateNeedToEditReasonAsync();
				await initialData.CreateCategoryTypeAsync();
				await initialData.CreateAttachmentSubjectAsync();
				await initialData.CreateFieldTypeAsync();
				await initialData.CreatePhoneOperatorAsync();
				await initialData.CreateRegionAsync();
				await initialData.CreatePlateCodeAsync();
				await initialData.CreatePlateStatusAsync();
				await initialData.CreateCategoryAsync();
				await initialData.CreateFieldAsync();
				await initialData.RunQuery();
			}
			// **************************************************

		}
	}
}
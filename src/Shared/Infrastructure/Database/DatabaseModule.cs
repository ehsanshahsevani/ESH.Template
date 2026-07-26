using Persistence;

using DynamicFields.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ESH.BuildingBlocks.ActionCodeGuard.Abstraction;
using ESH.BuildingBlocks.ActionCodeGuard.HttpServices;
using Microsoft.Extensions.DependencyInjection;
using ESH.BuildingBlocks.Application.Abstraction;
using ESH.BuildingBlocks.Attachments.Infrastructure;
using ESH.BuildingBlocks.Localization.Infrastructure;
using ESH.BuildingBlocks.Logging.Infrastructure;
using ESH.BuildingBlocks.NotificationCenter.Infrastructure;
using ESH.BuildingBlocks.SubSystem.Infrastructure;
using ESH.HttpServices.Abstraction.ProjectManager;
using ESH.HttpServices.ProjectManager;
using ESH.SeedworkSystem.Infrastructure.Abstractions;
using ActionPageHttpService = ESH.BuildingBlocks.ActionCodeGuard.HttpServices.ActionPageHttpService;
using IActionPageHttpService = ESH.BuildingBlocks.ActionCodeGuard.Abstraction.IActionPageHttpService;

namespace Infrastructure.Database;

public class DatabaseModule : IServiceModule
{
	public int Order => 20;

	public void Register(IServiceCollection services, IConfiguration configuration)
	{
		var connectionString =
			configuration.GetConnectionString(name: "connection")!;

		services.AddDbContext<DatabaseContext>(optionsAction: options =>
		{
			options.UseSqlServer(connectionString: connectionString);
		});

		services.AddScoped<IUnitOfWork, UnitOfWork>(implementationFactory: sp =>
		{
			DatabaseContext context = sp.GetRequiredService<DatabaseContext>();

			var result = new UnitOfWork(databaseContext: context);

			return result;
		});
		
		
		services.AddLogManagerServices<DatabaseContext>();
		services.AddSubSystemServices<DatabaseContext>();
		services.AddLocalizerServices<DatabaseContext>();

		// services.AddFileStorage();
		services.AddAttachmentServices<DatabaseContext>();

		services.AddScoped<ILanguageCodeResolver, Services.LanguageCodeResolver>();
		
		services.AddScoped<IActionHttpService, ActionHttpService>();
		services.AddScoped<ISubSystemHttpService, SubSystemHttpService>();
		services.AddScoped<IActionPageHttpService, ActionPageHttpService>();

		services.AddScoped<IDeleteAccountQueueHttpService, DeleteAccountQueueHttpService>();
		
		services.AddDynamicFieldsService();
		services.AddEshNotificationCenterServices();
	}
}
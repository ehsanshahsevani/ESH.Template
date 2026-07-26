using DynamicFields.Services;
using DynamicFields.Validator;
using DynamicFields.Abstraction;
using OnlineTranslate.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicFields.Infrastructure;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddDynamicFieldsService(this IServiceCollection services)
	{
		services.AddScoped<IFieldService, FieldService>();
		services.AddScoped<ICategoryService, CategoryService>();
		services.AddScoped<IAnnouncementService, AnnouncementService>();

		services.AddScoped<INotificationAnnouncementService, NotificationAnnouncementService>();

		services.AddScoped<FieldValidatorFactory>();

		services.AddOnlineTranslateService();

		return services;
	}
}
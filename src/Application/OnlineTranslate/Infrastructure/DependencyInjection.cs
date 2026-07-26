using OnlineTranslate.Services;
using OnlineTranslate.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace OnlineTranslate.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddOnlineTranslateService(this IServiceCollection services)
	{
		services.AddScoped<ITranslateService, GoogleTranslatorProvider>();

		return services;
	}
}
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PollingServices;

namespace Infrastructure.Polling;

internal class PollingModule
	: ESH.SeedworkSystem.Infrastructure.Abstractions.IServiceModule
{
	public int Order => 21;

	public void Register(IServiceCollection services, IConfiguration configuration)
	{
		services.AddHostedService<PollingProfileCheckerService>();
		services.AddHostedService<PollingAnnouncementExpireService>();
		services.AddHostedService<PollingDeleteAccountService>();
	}
}

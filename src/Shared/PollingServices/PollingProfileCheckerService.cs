using Domain;
using Domain.Base;
using ESH.Helpers;
using Persistence;
using ESH.ViewModels.Shared;


using Microsoft.Extensions.DependencyInjection;
using ESH.BuildingBlocks.Localization.Abstraction;
using ESH.BuildingBlocks.Logging.Contracts;
using ESH.HttpServices.ProjectManager;
using ESH.SeedworkSystem.Domain.Log;

namespace PollingServices;

using Microsoft.Extensions.Hosting;

public sealed class PollingProfileCheckerService : BackgroundService
{
	private IServiceScopeFactory ScopeFactory { get; }

	public PollingProfileCheckerService(IServiceScopeFactory scopeFactory)
	{
		ScopeFactory = scopeFactory;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		Console.ForegroundColor = ConsoleColor.Green;
		Console.WriteLine($@"[🔄️] namespace {nameof(PollingServices)} → {nameof(PollingProfileCheckerService)} → PeriodicTimer(TimeSpan.FromSeconds(15))");
		Console.ResetColor();

		using var timer = new PeriodicTimer(TimeSpan.FromSeconds(15));

		while (await timer.WaitForNextTickAsync(stoppingToken))
		{
			using var scope = ScopeFactory.CreateScope();

			var unitOfWork =
				scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

			var languageCodeManager =
				scope.ServiceProvider.GetRequiredService<ILanguageCodeManager>();

			var accountingService =
				scope.ServiceProvider.GetRequiredService<AccountingService>();

			var serverLogManager =
				scope.ServiceProvider
					.GetService(typeof(ILogServerManager)) as ILogServerManager;
			try
			{
				var result =
					await accountingService
						.GetQueueAsync(ServerKeyConstant.Key);

				if (result.IsSuccess == true)
				{
					var requestViewModels = new List<ProfileQueueRequestViewModel>();

					List<ProfileQueueResponseViewModel> queue = result.Value!;

					foreach (var queueItem in queue)
					{
						try
						{
							var defaultLanguageCode =
								await languageCodeManager
									.FindLanguageByCodeAsync
										(CurrentLanguage.Code(), stoppingToken);

							if (defaultLanguageCode == null)
							{
								throw new ArgumentNullException(nameof(defaultLanguageCode));
							}

							var profile = new Profile(queueItem.UserId, queueItem.PhoneNumber)
							{
								LanguageCodeId = defaultLanguageCode.Id
							};

							var checkProfile =
								await unitOfWork
									.ProfileRepository.FindAsync(queueItem.UserId, cancellationToken:stoppingToken);

							if (checkProfile is null)
							{
								await unitOfWork.ProfileRepository.AddAsync(profile, stoppingToken);

								await unitOfWork.SaveAsync(stoppingToken);
							}

							var req = new ProfileQueueRequestViewModel
								(serverId: ServerKeyConstant.Key, queueItem.UserId);

							requestViewModels.Add(req);
						}
						catch (Exception e)
						{
							var req = new ProfileQueueRequestViewModel
							(serverId: ServerKeyConstant.Key,
								queueItem.UserId, exception: e.Message);

							requestViewModels.Add(req);
						}
					}

					_ = await accountingService.UpdateQueueAsync(requestViewModels);
				}
			}
			catch (Exception ex)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(ex);
				Console.ResetColor();

				var serverLog = new LogServer
				{
					IsDeleted = false,
					Exceptions = ex.ToString(),
					Message = ex.Message,
					RequestPath = $"{nameof(PollingProfileCheckerService)}.cs → send request to project manager",
					Description = ex.Message,
					MethodName = "ExecuteAsync",
					ClassName = nameof(PollingProfileCheckerService),
					Namespace = nameof(PollingServices),
					RemoteIP = "Not Set",
					PortIP = "Not Set",
					HttpReferrer = "Not Set",
				};

				await serverLogManager!.CreateAsync(serverLog);
			}
		}
	}
}
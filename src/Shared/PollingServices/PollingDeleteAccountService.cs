using Domain.Base;
using Persistence;

using Microsoft.Extensions.Hosting;

using ESH.BuildingBlocks.Localization.Abstraction;
using ESH.BuildingBlocks.Logging.Contracts;
using ESH.HttpServices.Abstraction.ProjectManager;
using ESH.SeedworkSystem.Domain.Log;

using Microsoft.Extensions.DependencyInjection;

namespace PollingServices;

public class PollingDeleteAccountService : BackgroundService
{
	private IServiceScopeFactory ScopeFactory { get; }

	public PollingDeleteAccountService(IServiceScopeFactory scopeFactory)
	{
		ScopeFactory = scopeFactory;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		Console.ForegroundColor = ConsoleColor.Green;
		Console.WriteLine($@"[🔄] namespace {nameof(PollingServices)} → {nameof(PollingProfileCheckerService)} → PeriodicTimer(TimeSpan.FromSeconds(60))");
		Console.ResetColor();

		using var timer = new PeriodicTimer(period: TimeSpan.FromSeconds(120));

		while (await timer.WaitForNextTickAsync(stoppingToken))
		{
			using var scope = ScopeFactory.CreateScope();

			var unitOfWork =
				scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

			var languageCodeManager =
				scope.ServiceProvider.GetRequiredService<ILanguageCodeManager>();

			var deleteAccountQueueService =
				scope.ServiceProvider.GetRequiredService<IDeleteAccountQueueHttpService>();

			var serverLogManager =
				scope.ServiceProvider
					.GetService(typeof(ILogServerManager)) as ILogServerManager;
			try
			{
				var result =
					await deleteAccountQueueService
						.GetAsync(ServerKeyConstant.Key);

				if (result.IsSuccess == false)
				{
					throw new Exception($"[{nameof(PollingDeleteAccountService)}] project manager server is down!");
				}
				
				if (result.IsSuccess == true)
				{
					List<string> userIdsDeleted = [];
					
					foreach (string userId in result.Value!)
					{
						try
						{
							// delete all data for this userId

							Console.BackgroundColor = ConsoleColor.White;
							Console.ForegroundColor = ConsoleColor.Red;
							Console.Write($"this account '{userId}' has Deleted!");
							Console.ResetColor();

							Console.WriteLine("");

							await unitOfWork.FieldValueAnnouncementRepository.DeleteAccountAsync(userId, stoppingToken);
							await unitOfWork.ReportLogRepository.DeleteAccountAsync(userId, stoppingToken);
							await unitOfWork.NeedToEditLogRepository.DeleteAccountAsync(userId, stoppingToken);
							await unitOfWork.FavoriteRepository.DeleteAccountAsync(userId, stoppingToken);
							await unitOfWork.NoteRepository.DeleteAccountAsync(userId, stoppingToken);
							await unitOfWork.AnnouncementViewsRepository.DeleteAccountAsync(userId, stoppingToken);
							await unitOfWork.AnnouncementRepository.DeleteAccountAsync(userId, stoppingToken);
							await unitOfWork.ProfileRepository.DeleteAccountAsync(userId, stoppingToken);

							userIdsDeleted.Add(userId);
						}
						catch (Exception e)
						{
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine(e);
							Console.ResetColor();
						}
					}
					
					await unitOfWork.SaveAsync(stoppingToken);

					_ = await deleteAccountQueueService.CheckAsync(
							userIdsDeleted, serverKey: ServerKeyConstant.Key);
				}
			}
			catch (Exception ex)
			{
				try
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
				catch (Exception e)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine(e);
					Console.ResetColor();
				}
			}
		}
	}
}
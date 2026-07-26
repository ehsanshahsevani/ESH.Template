using ESH.BuildingBlocks.Logging.Contracts;
using ESH.SeedworkSystem.Domain.Log;
using Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace PollingServices;

using ESH.Constant.Announcement;
using Microsoft.Extensions.Hosting;

public sealed class PollingAnnouncementExpireService : BackgroundService
{
	private readonly IServiceScopeFactory _scopeFactory;

	public PollingAnnouncementExpireService(IServiceScopeFactory scopeFactory)
	{
		_scopeFactory = scopeFactory;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		Console.ForegroundColor = ConsoleColor.Green;
		Console.WriteLine($@"[🔄️] namespace {nameof(PollingServices)} → {nameof(PollingAnnouncementExpireService)} → new PeriodicTimer(TimeSpan.FromHours(24))");
		Console.ResetColor();

		var timer =
			new PeriodicTimer(TimeSpan.FromHours(24));

		var timeDay = 30 * 3;

		while (await timer.WaitForNextTickAsync(stoppingToken))
		{
			using var scope = _scopeFactory.CreateScope();

			var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
			var serverLogManager = scope.ServiceProvider.GetService<ILogServerManager>() as ILogServerManager;

			try
			{
				var repository = unitOfWork.AnnouncementRepository;

				// آگهی‌های فعال را بگیر
				var activeAnnouncements =
					await repository.GetAnnouncementsByStatusIdAsync
						(AnnouncementStatusCodes.Publish ,stoppingToken);

				var now = DateTime.UtcNow;

				var status =
					await unitOfWork.StatusRepository
						.FindByCodeAsync(AnnouncementStatusCodes.Expired, stoppingToken);

				if (status is null)
				{
					throw new NullReferenceException(nameof(status));
				}

				foreach (var item in activeAnnouncements)
				{
					var diff = now - item.UpdateDateTime;

					if (diff.TotalDays >= timeDay)
					{
						item.SetStatusId(status.Id);
					}
				}

				await unitOfWork.SaveAsync(stoppingToken);

				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine($"[OK] Expire scan completed at {now}. Expired {activeAnnouncements.Count} items (if needed).");
				Console.ResetColor();
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
					RequestPath = $"{nameof(PollingAnnouncementExpireService)}.cs → send request to project manager",
					Description = ex.Message,
					MethodName = "ExecuteAsync",
					ClassName = nameof(PollingAnnouncementExpireService),
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


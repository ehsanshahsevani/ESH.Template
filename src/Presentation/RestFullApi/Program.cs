using ESH.BuildingBlocks.ActionCodeGuard.Infrastructure;
using ESH.BuildingBlocks.ActionCodeGuard.Middlewares;
using ESH.HttpServices.Announcement;
using Infrastructure.Database;
using Infrastructure.AppExtensions;
using ESH.SeedworkSystem.Infrastructure.Extensions;
using Infrastructure.Filters.FilterActions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiDefaults();

builder.Services.AddInfrastructure(builder.Configuration, typeof(DatabaseModule).Assembly);

builder.Services.AddProjectAutoInjectionsByScrutor(
	typeof(CheckCategoryIdActionFilter),
	typeof(SubSystemHttpService)
);

builder.Services.AddActionCodeGuardService(
	typeof(Program).Assembly, serverKey: Domain.Base.ServerKeyConstant.Key);

// Build app
// ==========================
var app = builder.Build();

// Pre-routing
// ==========================
app.UseInfrastructure(ESH.SeedworkSystem.Infrastructure.Abstractions.WebPipelineStage.PreRouting);

app.UseStaticFiles();
// app.UseHttpsRedirection();
app.UseRouting();

// Post-routing
// ==========================
app.UseInfrastructure(ESH.SeedworkSystem.Infrastructure.Abstractions.WebPipelineStage.PostRouting);

app.UseMiddleware<ActionAccessMiddleware>();

// Controllers
// ==========================
app.MapControllers();

// Initial Data / Setup
// ==========================
await app.AddStartupTasks(System.Reflection.Assembly.GetExecutingAssembly());

app.Run();
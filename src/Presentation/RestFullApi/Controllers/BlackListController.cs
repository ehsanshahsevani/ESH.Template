using AutoMapper;
using ESH.BuildingBlocks.ActionCodeGuard;
using Persistence;
using Microsoft.AspNetCore.Mvc;
using ESH.BuildingBlocks.Localization.Abstraction;
using ESH.BuildingBlocks.Logging.Contracts;

namespace RestFullApi.Controllers;

public class BlackListController : Infrastructure.BaseControllerApi
{
	#region Constructor

	public IMapper Mapper { get; }
	public HttpClient HttpClient { get; }
	public IConfiguration Configuration { get; }
	public IHttpContextAccessor HttpContextAccessor { get; }
	public IUnitOfWork UnitOfWork { get; }
	public ILogDetailManager LogDetailManager { get; }
	public ILogServerManager LogServerManager { get; }
	public ILanguageCodeManager LanguageCodeManager { get; }
	private TokenRevocationGuard TokenRevocationGuard { get; }
	
	public BlackListController(
		IMapper mapper, HttpClient httpClient, IConfiguration configuration,
		IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork,
		ILogDetailManager logDetailManager, ILogServerManager logServerManager,
		ILanguageCodeManager languageCodeManager,
		TokenRevocationGuard tokenRevocationGuard
	) : base()
	{
		Mapper = mapper;
		HttpClient = httpClient;
		Configuration = configuration;
		HttpContextAccessor = httpContextAccessor;
		UnitOfWork = unitOfWork;
		LogDetailManager = logDetailManager;
		LogServerManager = logServerManager;
		LanguageCodeManager = languageCodeManager;
		TokenRevocationGuard = tokenRevocationGuard;
	}

	#endregion /Constructor

	#region [HttpPost(template: "notify")]

	/// <summary>
	/// هر لحظه از سرورهای مورد انتظار اطلاع داده میشود که چه توکن هایی زودتر از موعد سوختند!
	/// </summary>
	/// <param name="revokedTokens"></param>
	/// <returns></returns>
	[HttpPost(template: "notify")]
	public async Task<IActionResult> NotifyRevoked([FromBody] List<string> revokedTokens)
	{
		TokenRevocationGuard.AddToRevoked(revokedTokens);
		
		foreach (string token in revokedTokens)
		{
			Console.WriteLine(token);
		}
		
		var logMessage =
			$"[HttpPost(template: \"notify\")] called with {revokedTokens.Count}";

		await LogServerManager.CreateAsync(logMessage);
		
		return ToSampleResult(new FluentResults.Result());
	}

	#endregion /[HttpPost(template: "notify")]
}
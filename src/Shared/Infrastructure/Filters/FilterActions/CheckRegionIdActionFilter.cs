using ESH.BuildingBlocks.SampleResult;
using ESH.Constant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Persistence;

namespace Infrastructure.Filters.FilterActions;

public class CheckRegionIdActionFilter : ESH.SeedworkSystem.Infrastructure.ActionFilter.IBaseAsyncActionFilter
{
	public IUnitOfWork UnitOfWork { get; }

	public CheckRegionIdActionFilter(IUnitOfWork unitOfWork)
	{
		UnitOfWork = unitOfWork;
	}

	public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
	{
		var result = new FluentResults.Result();

		var id =
			context.ActionArguments.FirstOrDefault
			(predicate: current =>
				current.Value is string).Value as string;

		if (string.IsNullOrWhiteSpace(value: id) || id == Guid.NewGuid().ToString())
		{
			var errorMessage = string.Format(
				format: ESH.Resources.Messages.NotFoundError, arg0: ESH.Resources.DataDictionary.Guid);

			result.WithError(errorMessage: errorMessage);
		}
		else
		{
			var Region =
				await UnitOfWork
					.RegionRepository.FindAsync(id: id);

			if (Region is null)
			{
				var errorMessage = string.Format(
					format: ESH.Resources.Messages.NotFoundError, arg0: ESH.Resources.DataDictionary.Region);

				result.WithError(errorMessage: errorMessage);
			}
			else
			{
				context.HttpContext.Items[key: ProjectKeyName.ObjectKey] = Region;
			}
		}

		if (result.IsSuccess == true)
		{
			await next();
		}

		if (result.IsFailed == true)
		{
			var sampleResult = result.ConvertToSampleResult();

			context.Result =
				new BadRequestObjectResult(error: sampleResult);
		}
	}
}
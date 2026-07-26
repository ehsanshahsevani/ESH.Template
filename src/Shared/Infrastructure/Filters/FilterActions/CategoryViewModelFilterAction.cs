using ESH.BuildingBlocks.SampleResult;
using ESH.Constant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Persistence;
using ESH.Resources;
using ESH.ViewModels.Announcement;
  

namespace Infrastructure.Filters.FilterActions;

public class CategoryViewModelFilterAction :
	ESH.SeedworkSystem.Infrastructure.ActionFilter.IBaseAsyncActionFilter
{
	private IUnitOfWork UnitOfWork { get; }

	public CategoryViewModelFilterAction(IUnitOfWork unitOfWork)
	{
		UnitOfWork = unitOfWork;
	}

	public async Task OnActionExecutionAsync(
		ActionExecutingContext context, ActionExecutionDelegate next)
	{
		var result = new FluentResults.Result();

		object? action =
			context.RouteData.Values[key: ProjectKeyName.ActionKey];

		object? controller
			= context.RouteData.Values[key: ProjectKeyName.ControllerKey];

		// **************************************************
		bool tryGetValue =
			context.ActionArguments
				.TryGetValue(key: "model", value: out object? modelObject);
		// **************************************************

		if (tryGetValue == true && modelObject is CategoryRequestViewModel model)
		{
			var resultModel = model.Validate();

			result.WithErrors(errors: resultModel.Errors);

			if (result.IsSuccess == true)
			{
				if (string.IsNullOrEmpty(value: model.Id) == false)
				{
					var entity = await UnitOfWork
						.CategoryRepository.FindAsync(id: model.Id);

					if (entity is null)
					{
						var errorMessage = string.Format(
							format: ESH.Resources.Messages.NotFoundError,
							arg0: ESH.Resources.DataDictionary.Category);

						result.WithError(errorMessage: errorMessage);
					}
					else
					{
						context.HttpContext.Items[key: ProjectKeyName.ObjectKey] = entity;
					}
				}
			}
		}
		else
		{
			result.WithError(errorMessage: ResponseErrors.RequestNotValid400);
		}

		if (result.IsSuccess)
		{
			await next();
		}
		else
		{
			var sampleResult = result.ConvertToSampleResult();

			context.Result =
				new BadRequestObjectResult(error: sampleResult);
		}
	}
}
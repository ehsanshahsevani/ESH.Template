using ESH.BuildingBlocks.SampleResult;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Infrastructure.Filters.FilterActions;

public class CheckuPhoneNumberFilterAction :
	ESH.SeedworkSystem.Infrastructure.ActionFilter.IBaseAsyncActionFilter
{
	// public UserManager<User> UserManager { get; }
	//
	// public CheckuPhoneNumberFilterAction(UserManager<User> userManager)
	// {
	//     UserManager = userManager;
	// }

	public async Task OnActionExecutionAsync(
		ActionExecutingContext context, ActionExecutionDelegate next)
	{
		var result = new FluentResults.Result();

		//         object? action =
		//             context.RouteData.Values[Constants.ProjectKeyName.ActionKey];
		//
		//         object? controller
		//             = context.RouteData.Values[Constants.ProjectKeyName.ControllerKey];
		//
		//         // **************************************************
		//         bool hasNumberPhone =
		//             context.ActionArguments.TryGetValue("phoneNumber", out object? numberPhoneValue);
		// // **************************************************
		//
		//         if (hasNumberPhone == false)
		//         {
		//             if (hasNumberPhone == false)
		//             {
		//                 var errorMessage = string.Format(
		//                     ESH.Resources.Messages.RequiredError,
		//                     ESH.Resources.DataDictionary.PhoneNumber);
		//
		//                 result.WithError(errorMessage);
		//             }
		//         }
		//         else
		//         {
		//             string? numberPhone = numberPhoneValue as string;
		//
		//             if (string.IsNullOrWhiteSpace(numberPhone))
		//             {
		//                 var errorMessage = string.Format(
		//                     ESH.Resources.Messages.RequiredError,
		//                     ESH.Resources.DataDictionary.PhoneNumber);
		//
		//                 result.WithError(errorMessage);
		//             }
		//             
		//             var checkRegex = Regex.IsMatch(
		//                 numberPhone ?? string.Empty,
		//                 Constants.RegularExpression.CellPhoneNumber);
		//
		//             if (string.IsNullOrEmpty(numberPhone) == false && checkRegex == false)
		//             {
		//                 result.WithError(ESH.Resources.Messages.PhoneNumberStartWithError);
		//             }
		//
		//             var user = await UserManager.FindByNameAsync(numberPhone!);
		//
		//             if (user is null)
		//             {
		//                 result.WithError(ESH.Resources.Messages.UserNotFound);
		//             }
		//             else
		//             {
		//                 context.HttpContext.Items[Constants.ProjectKeyName.ObjectKey] = user;
		//             }
		//         }

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
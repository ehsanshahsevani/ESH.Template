using System.Text.RegularExpressions;
using ESH.BuildingBlocks.SampleResult;
using ESH.Constant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Infrastructure.Filters.FilterActions;

public class LoginFilterAction :
	ESH.SeedworkSystem.Infrastructure.ActionFilter.IBaseAsyncActionFilter
{
	public async Task OnActionExecutionAsync(
		ActionExecutingContext context, ActionExecutionDelegate next)
	{
		var result = new FluentResults.Result();

		object? action =
			context.RouteData.Values[key: ProjectKeyName.ActionKey];

		object? controller
			= context.RouteData.Values[key: ProjectKeyName.ControllerKey];

		// **************************************************
		bool hasNumberPhone =
			context.ActionArguments.TryGetValue(key: "phoneNumber", value: out object? numberPhoneValue);

		bool hasCaptchaCode =
			context.ActionArguments.TryGetValue(key: "captchaCode", value: out object? captchaCodeValue);
		// **************************************************

		if (hasNumberPhone == false || hasCaptchaCode == false)
		{
			if (hasNumberPhone == false)
			{
				var errorMessage = string.Format(
					format: ESH.Resources.Messages.RequiredError,
					arg0: ESH.Resources.DataDictionary.PhoneNumber);

				result.WithError(errorMessage: errorMessage);
			}

			if (hasCaptchaCode == false)
			{
				var errorMessage = string.Format(
					format: ESH.Resources.Messages.RequiredError,
					arg0: ESH.Resources.DataDictionary.Captcha);

				result.WithError(errorMessage: errorMessage);
			}
		}
		else
		{
			string? numberPhone = numberPhoneValue as string;
			string? captchaCode = captchaCodeValue as string;

			if (string.IsNullOrWhiteSpace(value: numberPhone))
			{
				var errorMessage = string.Format(
					format: ESH.Resources.Messages.RequiredError,
					arg0: ESH.Resources.DataDictionary.PhoneNumber);

				result.WithError(errorMessage: errorMessage);
			}

			if (string.IsNullOrWhiteSpace(value: captchaCode))
			{
				var errorMessage = string.Format(
					format: ESH.Resources.Messages.RequiredError,
					arg0: ESH.Resources.DataDictionary.Captcha);

				result.WithError(errorMessage: errorMessage);
			}

			var checkRegex = Regex.IsMatch(
				input: numberPhone ?? string.Empty,
				pattern: RegularExpression.CellPhoneNumber);

			if (string.IsNullOrEmpty(value: numberPhone) == false && checkRegex == false)
			{
				result.WithError(errorMessage: ESH.Resources.Messages.PhoneNumberStartWithError);
			}
		}

		if (result.IsSuccess == true)
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
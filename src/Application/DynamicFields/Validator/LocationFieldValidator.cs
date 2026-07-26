using FluentResults;
using DynamicFields.Configs;
using DynamicFields.Abstraction;
using ESH.ViewModels.Abstraction;

namespace DynamicFields.Validator;

public class LocationFieldValidator : IFieldValidator
{
	public async Task<Result> Validate(object? value, IFieldTypeConfig config)
	{
		var locationConfig = (LocationConfig)config;

		if (value is null)
		{
			var errorMessage = string.Format(
				ESH.Resources.Messages.RequiredError,
				ESH.Resources.DataDictionary.Location);

			var result = Result.Fail(errorMessage);

			return result;
		}

		if (locationConfig.AllowMap == true
			&& (value is Location) == false)
		{
			var errorMessage =
				ESH.Resources.Messages.LocationMustBeMapped;

			var result = Result.Fail(errorMessage);

			return result;
		}

		var location = value as Location;

		if (locationConfig.AddressSummary &&
			string.IsNullOrWhiteSpace(location?.AddressSummary) == true)
		{
			var errorMessage = string.Format(
				ESH.Resources.Messages.RequiredError,
				ESH.Resources.DataDictionary.AddressSummary);

			var result = Result.Fail(errorMessage);

			return result;
		}

		return Result.Ok();
	}
}
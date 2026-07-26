using Persistence;
using FluentResults;
using DynamicFields.Abstraction;
using ESH.ViewModels.Abstraction;

namespace DynamicFields.Validator;

public class RegionValidator : IFieldValidator
{
	private IUnitOfWork UnitOfWork { get; }

	public RegionValidator(IUnitOfWork unitOfWork)
	{
		UnitOfWork = unitOfWork;
	}

	public async Task<Result> Validate(object value, IFieldTypeConfig config)
	{
		var result = new Result();

		var regionId = value.ToString();

		var tryToGuid =
			Guid.TryParse(regionId, out _);

		if (tryToGuid == false)
		{
			var errorMessage =
				ESH.Resources.ResponseErrors.RequestNotValid400;

			result.WithError(errorMessage);

			return result;
		}

		var regionSearch =
			await UnitOfWork.RegionRepository.FindAsync(regionId!);

		if (regionSearch is null)
		{
			var errorMessage = string.Format(
				ESH.Resources.Messages.NotFoundError,
				ESH.Resources.DataDictionary.Region);

			result.WithError(errorMessage);
		}

		return result;
	}
}

using Persistence;
using FluentResults;
using DynamicFields.Abstraction;
using ESH.ViewModels.Abstraction;

namespace DynamicFields.Validator;

public class PlateStatusValidator : IFieldValidator
{
	private IUnitOfWork UnitOfWork { get; }

	public PlateStatusValidator(IUnitOfWork unitOfWork)
	{
		UnitOfWork = unitOfWork;
	}

	public async Task<Result> Validate(object value, IFieldTypeConfig config)
	{
		var result = new Result();

		var plateStatusId = value.ToString();

		var tryToGuid =
			Guid.TryParse(plateStatusId, out _);

		if (tryToGuid == false)
		{
			var errorMessage =
				ESH.Resources.ResponseErrors.RequestNotValid400;

			result.WithError($"{errorMessage}");

			return result;
		}

		var regionSearch =
			await UnitOfWork.PlateStatusRepository.FindAsync(plateStatusId!);

		if (regionSearch is null)
		{
			var errorMessage = string.Format(
				ESH.Resources.Messages.NotFoundError,
				ESH.Resources.DataDictionary.PlateStatus);

			result.WithError($"{errorMessage}");
		}

		return result;
	}
}
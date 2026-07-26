using ESH.ViewModels.Abstraction;
using FluentResults;

namespace DynamicFields.Abstraction;

public interface IFieldValidator
{
	Task<Result> Validate(object value, IFieldTypeConfig config);
}
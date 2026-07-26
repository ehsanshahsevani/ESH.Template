using ESH.Utilities;
using FluentResults;
using ESH.ViewModels.Announcement;

namespace DynamicFields.Abstraction;

public interface IFieldService
{
	Task<Result<List<FieldResponseViewModel>>> GetByCategoryIdAsync(
		string? categoryId,
		bool? isActive = true,
		CancellationToken cancellationToken = default);
	Task<Result<List<FieldResponseViewModel>>> GetFiltersByCategoryIdAsync(string? categoryId, CancellationToken cancellationToken = default);
	Task<Result> CreatePriceForCategoryAsync(FieldReadyRequestViewModel model, CancellationToken cancellationToken = default);

	Task<Result> CreateDescriptionForCategoryAsync(
		FieldReadyRequestViewModel model,
		CancellationToken cancellationToken = default);

	Task<Result> CreateImageFieldForCategoryAsync(
		FieldReadyRequestViewModel model,
		CancellationToken cancellationToken = default);

	Task<Result> CreateTitleForCategoryAsync(
		FieldReadyRequestViewModel model,
		CancellationToken cancellationToken = default);

	Task<Result> CreateLocationFieldForCategoryAsync(
		FieldReadyRequestViewModel model,
		CancellationToken cancellationToken = default);

	Task<Result> CreateRegionFieldForCategoryAsync(
		FieldReadyRequestViewModel model,
		CancellationToken cancellationToken = default);
	Task<Result> UpdateFieldAsync(FieldReadyRequestViewModel model, CancellationToken cancellationToken = default);
	Task<Result> ChangeActivationAsync(string id, CancellationToken cancellationToken = default);
	Task<Result> CreateFieldWithValuesAsync(FieldCustomValuesRequestViewModel model, CancellationToken cancellationToken = default);
	Task<Result<List<UiSelectModel>>> GetCustomValuesAsync(string fieldId, bool? isActive);
}
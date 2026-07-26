using ESH.Utilities;
using FluentResults;
using ESH.ViewModels.Shared;
using Microsoft.AspNetCore.Http;
using ESH.ViewModels.Announcement;
using ESH.SeedworkSystem.ViewModel.Localizer;
using ESH.ViewModels.Announcement.ModelParameters;

namespace DynamicFields.Abstraction;

public interface ICategoryService
{
	Task<Result<CategoryResponseViewModel>> ChangeActivationAsync(string id, CancellationToken cancellationToken = default);
	Task<Result> CreateAsync(CategoryRequestViewModel model, IFormFile fileLarge, IFormFile fileSmall, CancellationToken cancellationToken = default);
	Task<Result<string>> DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<List<CategoryResponseViewModel>>> GetAllPinInHomeAsync(bool? isActive = true, bool withAnnouncement = true, CancellationToken cancellationToken = default);
    Task<Result<PagedListResult<CategoryResponseViewModel>>> GetAllWithPageAsync(CategoryParameters parameters, CancellationToken cancellationToken = default);
	Task<Result<List<CategoryResponseViewModel>>> GetAsync(CancellationToken cancellationToken = default);
	Task<Result<CategoryRequestViewModel>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
	Task<Result<List<CategoryResponseViewModel>>> GetChildrenAsync(string parentId, CancellationToken cancellationToken = default);
	Task<Result<List<UiSelectModel>>> GetDropDownDataAsync(CancellationToken cancellationToken = default);
	Task<Result<List<CategoryResponseViewModel>>> GetParentsAsync(CancellationToken cancellationToken = default);
	Task<Result<List<CategoryResponseViewModel>>> SearchByTextAsync(string text, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(CategoryRequestViewModel categoryRequestViewModel, CancellationToken cancellationToken = default);
    Task<Result> UpdateImageAsync(string attachmentSubjectKey, string id, IFormFile file, CancellationToken cancellationToken = default);
	Task<Result> UpdateNameAsync(string id, List<ValueLocalizerViewModel> name, CancellationToken cancellationToken = default);
}
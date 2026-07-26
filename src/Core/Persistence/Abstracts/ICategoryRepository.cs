using Domain;
using ESH.BuildingBlocks.RequestFeatures;
using ESH.SeedworkSystem.Persistence;
using ESH.Utilities;
using ESH.ViewModels.Announcement;
using ESH.ViewModels.Announcement.ModelParameters;


namespace Persistence.Abstracts;

public interface ICategoryRepository : IRepository<Category>
{
	Task<List<UiSelectModel>> GetSelectValues(CancellationToken cancellationToken = default);

	/// <summary>
	/// دریافت دسته بندی ها به صورت صفحه بندی با قابلیت سرچ پیشرفته
	/// </summary>
	/// <param name="parameters"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<PagedList<Category>> GetAllInPageAsync(
		CategoryParameters parameters, CancellationToken cancellationToken = default);

	/// <summary>
	/// دریافت دسته بندی ها به صورت صفحه بندی با قابلیت سرچ پیشرفته برای ادمین
	/// </summary>
	/// <param name="parameters"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<PagedList<Category>> GetAllInPageForAdminAsync(
		CategoryParameters parameters, CancellationToken cancellationToken = default);
	Task<CategoryBoxViewModel> GetProductCategoryBoxAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// دریافت فقط دسته بندی های والد
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<List<Category>> GetParentCategoriesAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// دریافت زیر دسته بندی های یک دسته بندی خاص
	/// </summary>
	/// <param name="parentId">شناسه دسته بندی والد</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<List<Category>> GetChildrenCategoriesAsync(string parentId, CancellationToken cancellationToken = default);

	Task<List<Category>> GetAllWithAnnouncementCheckAsync(
		List<string> ids,
		CancellationToken cancellationToken = default);

	Task<List<Category>> GetAllWithCodeAsync(CancellationToken cancellationToken = default);
	Task<Category?> FindAdminAsync(object id, CancellationToken cancellationToken = default);
    Task<bool> HasAnnouncementAsync(string categoryId, CancellationToken cancellationToken = default);
    Task<bool> HasChildAsync(string categoryId, CancellationToken cancellationToken = default);
    Task<List<Category>> GetAllPinInHomeAsync(bool? isActive = true, int takeAnnouncement = 5, CancellationToken cancellationToken = default);
}
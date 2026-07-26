using Domain;
using ESH.Utilities;

using Persistence.Abstracts;
using ESH.ViewModels.Announcement;
using Microsoft.EntityFrameworkCore;

using ESH.BuildingBlocks.RequestFeatures;
using ESH.Constant.Announcement;
using ESH.ViewModels.Announcement.ModelParameters;
  
 

namespace Persistence.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
	internal CategoryRepository(DatabaseContext databaseContext) : base(databaseContext)
	{
	}

	/// <summary>
	/// نمایش DropDown های جدول
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<List<UiSelectModel>> GetSelectValues(CancellationToken cancellationToken = default)
	{
		var result = await DbSet
			.Where(p => p.IsDeleted == false)
			.Select(p => new UiSelectModel("", p.Id))
			.ToListAsync(cancellationToken);

		return result;
	}

	/// <summary>
	/// دریافت دسته بندی ها به صورت صفحه بندی با قابلیت سرچ پیشرفته
	/// </summary>
	/// <param name="parameters"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<PagedList<Category>> GetAllInPageAsync(
		CategoryParameters parameters, CancellationToken cancellationToken = default)
	{
		var date = parameters.Text.StringToDateTimeMiladi();

		var monthNumberShamsi =
			parameters.Text.ChangeMonthNameShamsiToNumberMonth();

		int? monthNumberMiladi = null;

		if (monthNumberShamsi.HasValue == true)
		{
			var dateString = $"1403/{monthNumberShamsi.Value.ToString().PadLeft(2, '0')}/01";

			monthNumberMiladi = dateString.StringToDateTimeMiladi()!.Value.Month;
		}

		var source = DbSet
			.Where(current => current.IsDeleted == false)
			.Where(current => current.IsActive == true)
			.Where(current =>
				string.IsNullOrEmpty(parameters.Text) == true
				||
				current.Id.Contains(parameters.Text) == true
				// conect to localizer
				//||
				//(
				//    string.IsNullOrEmpty(current.Name) == false
				//    && current.Name.Contains(parameters.Text)
				//)
				||
				(
					string.IsNullOrEmpty(current.Description) == false
					&& current.Description.Contains(parameters.Text))
			)
			.Where(current =>
				date.HasValue == false
				|| current.CreateDateTime == date.Value
				|| current.CreateDateTime == date.Value
				|| monthNumberMiladi.HasValue == false
				|| current.CreateDateTime.Month == monthNumberMiladi.Value
				|| current.CreateDateTime.Month == monthNumberMiladi.Value)
			.OrderBy(o => o.Ordering)
			.ThenByDescending(p => p.CreateDateTime);

		var result = await PagedList
			<Category>.ToPagedList(source, parameters, cancellationToken);

		return result;
	}

	/// <summary>
	/// Returns basic statistics for product categories (count / active / deactive).
	/// </summary>
	public async Task<CategoryBoxViewModel> GetProductCategoryBoxAsync(CancellationToken cancellationToken = default)
	{
		var source = DbSet.Where(x => x.IsDeleted == false);

		var count = await source.CountAsync(cancellationToken);
		var active = await source.Where(x => x.IsActive == true).CountAsync(cancellationToken);
		var deActive = count - active;

		return new CategoryBoxViewModel
		{
			Count = count,
			Active = active,
			DeActive = deActive
		};
	}

	/// <summary>
	/// دریافت دسته بندی ها به صورت صفحه بندی با قابلیت سرچ پیشرفته برای ادمین
	/// </summary>
	/// <param name="parameters"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<PagedList<Category>> GetAllInPageForAdminAsync(
		CategoryParameters parameters, CancellationToken cancellationToken = default)
	{
		var source = DbSet
				
			.Include(current => current.CategoryType)
			.Include(current => current.Parent)
			
			.Where(current => current.IsDeleted == false)
			
			.Where(current =>
				string.IsNullOrEmpty(parameters.Text) == true
				||
				current.Id.Contains(parameters.Text) == true
				||
				(string.IsNullOrEmpty(current.Description) == false
				 && current.Description.Contains(parameters.Text))
			)
			
			.OrderBy(o => o.Ordering)
			.ThenByDescending(p => p.CreateDateTime);

		var result =
			await PagedList<Category>.ToPagedList(
				source, parameters, cancellationToken);

		return result;
	}

	/// <summary>
	/// Checks whether the category name exists anywhere within the same tree (parent to children).
	/// </summary>
	private async Task<bool> IsNameExist(CategoryRequestViewModel entity,
		CancellationToken cancellationToken = default)
	{
		var isExist = await DbSet
			.Where(x => x.Id != entity.Id)
			.Where(x => x.IsDeleted == false)
			// conect to localizer
			// .Where(x => x.Name == entity.Name)
			.AnyAsync(cancellationToken);

		return isExist;
	}

	/// <summary>
	/// دریافت فقط دسته بندی های والد
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<List<Category>> GetParentCategoriesAsync(CancellationToken cancellationToken = default)
	{
		var result = await DbSet
			.Include(current => current.CategoryType)
			.Where(current => current.IsDeleted == false)
			.Where(current => current.IsActive == true)
			.Where(current => current.ParentId == null)
			.OrderBy(o => o.Ordering)
			.ThenByDescending(p => p.CreateDateTime)
			.ToListAsync(cancellationToken);

		return result;
	}

	/// <summary>
	/// دریافت زیر دسته بندی های یک دسته بندی خاص
	/// </summary>
	/// <param name="parentId">شناسه دسته بندی والد</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<List<Category>> GetChildrenCategoriesAsync(string parentId,
		CancellationToken cancellationToken = default)
	{
		var result = await DbSet
			.Include(current => current.CategoryType)
			.Where(current => current.IsDeleted == false)
			.Where(current => current.IsActive == true)
			.Where(current => current.ParentId == parentId)
			.OrderBy(o => o.Ordering)
			.ThenByDescending(p => p.CreateDateTime)
			.ToListAsync(cancellationToken);

		return result;
	}

	public override async Task<IEnumerable<Category?>> GetAllAsync(CancellationToken cancellationToken = default)
	{
		var result = await DbSet
			.Include(current => current.CategoryType)
			.Where(current => current.IsDeleted == false)
			.Where(current => current.IsActive == true)
			.OrderBy(o => o.Ordering)
			.ThenByDescending(p => p.CreateDateTime)
			.ToListAsync(cancellationToken);

		return result;
	}

	public async Task<List<Category>> GetAllWithAnnouncementCheckAsync(
		List<string> ids,
		CancellationToken cancellationToken = default)
	{
		var result = await DbSet
			.Include(current => current.CategoryType)
			.Where(current => current.IsDeleted == false)
			.Where(current => current.IsActive == true)

			.Where(current => ids.Contains(current.Id) == true)

			.Where(current => current.Announcements.Any() == true)

			.OrderBy(o => o.Ordering)
			.ThenByDescending(p => p.CreateDateTime)
			
			.ToListAsync(cancellationToken);

		return result;
	}

	public async Task<List<Category>> GetAllWithCodeAsync(CancellationToken cancellationToken = default)
	{
		var result = await DbSet

			.Include(current => current.CategoryType)

			.Where(current => current.IsDeleted == false)
			.Where(current => current.IsActive == true)

			.Where(current => string.IsNullOrEmpty(current.Code) == false)

			.OrderBy(o => o.Ordering)
			.ThenByDescending(p => p.CreateDateTime)

			.ToListAsync(cancellationToken);

		return result;
	}

	public override async Task<Category?> FindAsync(object id, bool? isActive = true,  CancellationToken cancellationToken = default)
	{
		var result = await DbSet

			.Include(current => current.CategoryType)

			.Where(current => current.IsDeleted == false)
			.Where(current => isActive.HasValue == false || current.IsActive == isActive.Value)

			.Where(current => current.Id == id.ToString())

			.FirstOrDefaultAsync(cancellationToken);

		return result;
	}

	public async Task<Category?> FindAdminAsync(object id, CancellationToken cancellationToken = default)
	{
		var result = await DbSet

			.Include(current => current.CategoryType)

			.Where(current => current.IsDeleted == false)

			.Where(current => current.Id == id.ToString())

			.FirstOrDefaultAsync(cancellationToken);

		return result;
	}

    public async Task<bool> HasAnnouncementAsync(
		string categoryId, CancellationToken cancellationToken)
	{
		var result = await DbSet
			
			.Where(current => current.IsDeleted == false)
			
			.Where(current => current.Id == categoryId)

			.Select(current => current.Announcements.Any())

			.FirstOrDefaultAsync(cancellationToken)

			;

		return result;
	}

    public async Task<bool> HasChildAsync(
		string categoryId, CancellationToken cancellationToken)
	{
		var result = await DbSet
			
			.Where(current => current.IsDeleted == false)
			
			.Where(current => current.Id == categoryId)

			.Select(current => current.Children.Where(x => x.IsDeleted == false).Any() == true)

			.FirstOrDefaultAsync(cancellationToken)

			;

		return result;
	}

    public async Task<List<Category>> GetAllPinInHomeAsync(
		bool? isActive = true,
		int takeAnnouncement = 5,
		CancellationToken cancellationToken = default)
    {
		var result = await DbSet

			.Include(current => current.CategoryType)

			.Where(current => current.IsDeleted == false)
			.Where(current => isActive.HasValue == false || current.IsActive == isActive)
			.Where(current=> current.PinInHome.HasValue == true)

			.Where(current => current.Announcements
				.Where(ann => isActive.HasValue == false || ann.IsActive == isActive)
				.Where(ann => ann.Status!.Code == AnnouncementStatusCodes.Publish)
				.Any() == true)

			.OrderBy(current => current.PinInHome)
			.ThenByDescending(current=> current.UpdateDateTime)

			.ToListAsync(cancellationToken);

		return result;
    }
}
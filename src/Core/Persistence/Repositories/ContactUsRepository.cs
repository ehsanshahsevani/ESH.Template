
using Domain;
using ESH.BuildingBlocks.RequestFeatures;
using ESH.ViewModels.Announcement.ModelParameters;
using Persistence.Abstracts;
 

namespace Persistence.Repositories;

public class ContactUsRepository : Repository<Domain.ContactUs>, IContactUsRepository
{
	#region Constructor

	internal ContactUsRepository(DatabaseContext databaseContext) : base(databaseContext)
	{
	}

	#endregion /Constructor

	#region GetWithPageAsync

	public async Task<PagedList<ContactUs>> GetWithPageAsync(
		ContactUsParameters parameters, CancellationToken cancellationToken = default)
	{
		var source = DbSet
			
				.Where(current => current.IsDeleted == false)
				
				.Where(current => current.IsActive == true)
				
				.AsQueryable()
			;

		if (string.IsNullOrEmpty(parameters.Text) == false)
		{
			source = source.Where(current =>
				(string.IsNullOrEmpty(current.FirstName) == false && current.FirstName.Contains(parameters.Text))
				|| (string.IsNullOrEmpty(current.LastName) == false && current.LastName.Contains(parameters.Text))
				|| (string.IsNullOrEmpty(current.EmailAddress) == false && current.EmailAddress.Contains(parameters.Text))
				|| (string.IsNullOrEmpty(current.PhoneNumber) == false && current.PhoneNumber.Contains(parameters.Text)));
		}
		
		PagedList<ContactUs> result =
			await PagedList<ContactUs>
				.ToPagedList(source, parameters, cancellationToken);

		return result;
	}

	#endregion /GetWithPageAsync
}
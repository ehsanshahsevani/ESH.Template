using Domain;
using ESH.SeedworkSystem.Persistence;
using ESH.BuildingBlocks.RequestFeatures;
using ESH.ViewModels.Announcement.ModelParameters;


namespace Persistence.Abstracts;

public interface IContactUsRepository : IRepository<ContactUs>
{
	Task<PagedList<ContactUs>> GetWithPageAsync(
		ContactUsParameters parameters, CancellationToken cancellationToken = default);
}
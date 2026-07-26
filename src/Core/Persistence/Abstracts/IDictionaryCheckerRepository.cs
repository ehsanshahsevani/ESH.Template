
using Domain;
using ESH.SeedworkSystem.Persistence;

using ESH.BuildingBlocks.RequestFeatures;
using ESH.Utilities;
using ESH.ViewModels.Announcement.ModelParameters;
 

namespace Persistence.Abstracts;

// For DictionaryChecker
public interface IDictionaryCheckerRepository : IRepository<DictionaryChecker>
{
	Task<PagedList<DictionaryChecker>> GetAllWithPageAsync(
		DictionaryCheckerParameters parameters, CancellationToken cancellationToken = default);
	Task<List<UiSelectModel>> GetUiSelectAsync(CancellationToken cancellationToken = default);
	Task<bool> TextHasExistAsync(string? id, string text, CancellationToken cancellationToken = default);
	Task<bool> CheckTextsAsync(List<string> texts, CancellationToken cancellationToken = default);
}
using Persistence;
using System.Collections.Concurrent;
using ESH.BuildingBlocks.Application.Abstraction;

namespace Infrastructure.Services
{
	public class LanguageCodeResolver : ILanguageCodeResolver
	{
		private readonly IUnitOfWork _unitOfWork;

		private static readonly
			ConcurrentDictionary
				<string, (string LanguageCode, DateTimeOffset LastUpdated)> _cache = new();

		private const int CacheDurationSeconds = 0;

		public LanguageCodeResolver(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<string?> GetLanguageCodeAsync(string userId)
		{
			if (string.IsNullOrEmpty(value: userId) == true)
			{
				return null;
			}

			if (_cache.TryGetValue(key: userId, value: out var cached) == true)
			{
				if ((DateTime.UtcNow - cached.LastUpdated).TotalSeconds < CacheDurationSeconds)
				{
					return cached.LanguageCode;
				}
			}

			var result = await _unitOfWork
				.ProfileRepository.FindLanguageCodeAsync(id: userId);

			if (string.IsNullOrEmpty(value: result) == false)
			{
				_cache[key: userId] = (result, DateTime.UtcNow);
			}

			return result;
		}
	}
}

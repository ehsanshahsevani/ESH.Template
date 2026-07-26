using FluentResults;
using OnlineTranslate.Abstraction;

namespace OnlineTranslate.Services;

public class GoogleTranslatorProvider : ITranslateService
{
	public Task<Result<string>> TranslateAsync(string text, string fromLanguage, string toLanguage)
	{
		throw new NotImplementedException();
	}
}
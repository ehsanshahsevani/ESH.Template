using FluentResults;

namespace OnlineTranslate.Abstraction;

public interface ITranslateService
{
	Task<Result<string>> TranslateAsync(string text, string fromLanguage, string toLanguage);
}
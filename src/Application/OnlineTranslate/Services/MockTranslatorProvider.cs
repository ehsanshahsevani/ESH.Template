using FluentResults;
using OnlineTranslate.Abstraction;

namespace OnlineTranslate.Services;

public class MockTranslatorProvider : ITranslateService
{
	public async Task<Result<string>> TranslateAsync(string text, string fromLanguage, string toLanguage)
	{
		var result = new Result<string>();

		if (result.IsSuccess is true)
		{
			result.WithValue(text);
		}

		return result;
	}
}
using DragonSpark.Model.Selection.Alterations;
using Sluggy;
using Sluggy.Strategies;

namespace DragonSpark.Application;

public class UniqueResourceName : IAlteration<string>
{
	readonly string               _separator;
	readonly ITranslationStrategy _strategy;

	public UniqueResourceName(string separator, ITranslationStrategy strategy)
	{
		_separator = separator;
		_strategy  = strategy;
	}

	public string Get(string parameter) => parameter.Replace(_separator, " ").ToSlug(_separator, _strategy);
}
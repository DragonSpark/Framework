using DragonSpark.Model.Selection.Alterations;
using Sluggy;
using Sluggy.Strategies;

namespace DragonSpark.Application
{
	public sealed class DefaultUniqueResourceName : UniqueResourceName
	{
		public static DefaultUniqueResourceName Default { get; } = new DefaultUniqueResourceName();

		DefaultUniqueResourceName() : base(Sluggy.Sluggy.DefaultSeparator, Sluggy.Sluggy.DefaultTranslationStrategy) {}
	}

	public class UniqueResourceName : IAlteration<string>
	{
		readonly string               _separator;
		readonly ITranslationStrategy _strategy;

		public UniqueResourceName(string separator, ITranslationStrategy strategy)
		{
			_separator = separator;
			_strategy  = strategy;
		}

		public string Get(string parameter) => parameter.ToSlug(_separator, _strategy);
	}
}
using DragonSpark.Model.Selection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Diagnostics;

sealed class ComposeRules : ISelect<IConfiguration, IEnumerable<LoggerFilterRule>>
{
	public static ComposeRules Default { get; } = new();

	ComposeRules() : this(LoggingLevelPath.Default, StringComparison.OrdinalIgnoreCase) {}

	readonly string           _section;
	readonly StringComparison _comparison;

	public ComposeRules(string section, StringComparison comparison)
	{
		_section    = section;
		_comparison = comparison;
	}

	public IEnumerable<LoggerFilterRule> Get(IConfiguration parameter)
	{
		var section = parameter.GetSection(_section);

		var levels = section.Get<Dictionary<string, LogLevel>>() ?? new();

		var result = new List<LoggerFilterRule>();

		foreach (var (category, level) in levels)
		{
			result.Add(new(null, string.Equals(category, "Default", _comparison) ? null : category, level, null));
		}

		return result;
	}
}
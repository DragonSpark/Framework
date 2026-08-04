using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog.Events;

namespace DragonSpark.Diagnostics;

sealed class LevelParser : IParser<LogEventLevel>
{
	readonly Array<LoggerFilterRule>          _rules;
	readonly ISelect<LogLevel, LogEventLevel> _level;
	readonly StringComparison                 _comparison;

	public LevelParser(IConfiguration configuration) : this(ComposeRules.Default.Get(configuration)) {}

	public LevelParser(IEnumerable<LoggerFilterRule> rules)
		: this(rules.OrderByDescending(r => r.CategoryName?.Length ?? 0).Result(), LogEventLevels.Default,
		       StringComparison.OrdinalIgnoreCase) {}

	public LevelParser(Array<LoggerFilterRule> rules, ISelect<LogLevel, LogEventLevel> level,
	                   StringComparison comparison)
	{
		_rules      = rules;
		_level      = level;
		_comparison = comparison;
	}

	public LogEventLevel Get(string parameter)
	{
		foreach (var rule in _rules)
		{
			if ((rule.CategoryName.IsNullOrEmpty() || parameter.StartsWith(rule.CategoryName, _comparison)) &&
			    rule.LogLevel.HasValue)
			{
				return _level.Get(rule.LogLevel.Value);
			}
		}

		return LogEventLevel.Information;
	}
}
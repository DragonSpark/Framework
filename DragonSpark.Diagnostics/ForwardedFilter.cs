using DragonSpark.Text;
using Microsoft.Extensions.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace DragonSpark.Diagnostics;

sealed class ForwardedFilter : ILogEventFilter
{
	readonly IParser<LogEventLevel> _level;

	public ForwardedFilter(IConfiguration configuration) : this(new LevelParser(configuration)) {}

	public ForwardedFilter(IParser<LogEventLevel> level) => _level = level;

	public bool IsEnabled(LogEvent logEvent)
	{
		var category = logEvent.Properties.TryGetValue("SourceContext", out var property)
		               && property is ScalarValue { Value: string source }
			               ? source
			               : string.Empty;

		var level = _level.Get(category);
		return logEvent.Level >= level;
	}
}
using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using Serilog.Core;
using Serilog.Events;

namespace DragonSpark.Diagnostics;

sealed class PolicyFilter : ILogEventFilter
{
	readonly Array<string> _sources;
	readonly ICondition    _policy;

	public PolicyFilter(params string[] sources) : this(sources, IsPolicy.Default) {}

	public PolicyFilter(Array<string> sources, ICondition policy)
	{
		_sources = sources;
		_policy  = policy;
	}

	public bool IsEnabled(LogEvent logEvent)
		=> logEvent is not { Level: LogEventLevel.Error } ||
		   logEvent.Properties.TryGetValue("SourceContext", out var source)
		   && _sources.Open().Contains(source.ToString())
		   && _policy.Get();
}
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Sentry;

sealed class Process : ISelect<SentryEvent, SentryEvent?>
{
	public static Process Default { get; } = new();

	Process() : this(ShouldProcess.Default) {}

	readonly ICondition<SentryEvent> _process;

	public Process(ICondition<SentryEvent> process) => _process = process;

	public SentryEvent? Get(SentryEvent parameter) => _process.Get(parameter) ? parameter : null;
}
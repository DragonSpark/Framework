using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Sentry;

sealed class BeforeSend : ISelect<SentryEvent, SentryEvent?>
{
	public static BeforeSend Default { get; } = new();

	BeforeSend() : this(HasException.Default) {}

	readonly ICondition<SentryEvent> _valid;

	public BeforeSend(ICondition<SentryEvent> valid) => _valid = valid;

	public SentryEvent? Get(SentryEvent parameter) => _valid.Get(parameter) ? parameter : null;
}
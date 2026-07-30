using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences.Collections;
using Sentry.Protocol;

namespace DragonSpark.Sentry;

sealed class ProcessedExceptions : ICondition<SentryEvent>
{
	public static ProcessedExceptions Default { get; } = new();

	ProcessedExceptions() : this(typeof(OperationCanceledException), typeof(TaskCanceledException)) {}

	readonly Func<SentryException, bool> _report;

	public ProcessedExceptions(params Type[] ignore) : this(ignore.Select(x => x.FullName.Verify()).ToArray()) {}

	public ProcessedExceptions(params string[] ignore)
		: this(new Contains(ignore).Then().Inverse().Accept<SentryException>(x => x.Type)) {}

	public ProcessedExceptions(Func<SentryException, bool> report) => _report = report;

	public bool Get(SentryEvent parameter)
		=> parameter.SentryExceptions is not {} exceptions || exceptions.All(_report);
}
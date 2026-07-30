using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Sentry;

sealed class ShouldProcess : AllCondition<SentryEvent>
{
	public static ShouldProcess Default { get; } = new();

	ShouldProcess() : base(ProcessedException.Default, ProcessedExceptions.Default) {}
}
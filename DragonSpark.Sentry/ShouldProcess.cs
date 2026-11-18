using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Sentry;

sealed class ShouldProcess : ICondition<SentryEvent>
{
	readonly Func<Exception, bool> _process;

	public ShouldProcess(Func<Exception, bool> process) => _process = process;

	public bool Get(SentryEvent parameter) => parameter.Exception is not {} exception || _process(exception);
}
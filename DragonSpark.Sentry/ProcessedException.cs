using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Sentry;

sealed class ProcessedException : ICondition<SentryEvent>
{
	public static ProcessedException Default { get; } = new();

	ProcessedException() : this(Application.AspNet.Diagnostics.ShouldProcess.Default) {}

	readonly ICondition<Exception> _process;

	public ProcessedException(ICondition<Exception> process) => _process = process;

	public bool Get(SentryEvent parameter) => parameter.Exception is not {} exception || _process.Get(exception);
}
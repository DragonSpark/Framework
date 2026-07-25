using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.AspNet.Diagnostics;

public sealed class AggregateAwareIgnoreException : ICondition<Exception>
{
	public static AggregateAwareIgnoreException Default { get; } = new();

	AggregateAwareIgnoreException() : this(InnerExceptionAwareIgnoreException.Default.Get) {}

	readonly Func<Exception, bool> _previous;

	public AggregateAwareIgnoreException(Func<Exception, bool> previous) => _previous = previous;

	public bool Get(Exception parameter)
		=> parameter is AggregateException aggregate
			   ? aggregate.Flatten().InnerExceptions.Any(_previous)
			   : _previous(parameter);
}
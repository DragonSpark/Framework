using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Diagnostics;

public class AggregateAwareCondition : ICondition<Exception>
{
	readonly Func<Exception, bool> _previous;

	protected AggregateAwareCondition(Func<Exception, bool> previous) => _previous = previous;

	public bool Get(Exception parameter)
		=> parameter is AggregateException aggregate
			   ? aggregate.Flatten().InnerExceptions.Any(_previous)
			   : _previous(parameter);
}
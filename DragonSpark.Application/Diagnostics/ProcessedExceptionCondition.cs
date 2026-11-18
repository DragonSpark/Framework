using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Diagnostics;

public class ProcessedExceptionCondition : AggregateAwareCondition
{
	public ProcessedExceptionCondition(ICondition<Exception> previous) : this(previous.Get) {}

	public ProcessedExceptionCondition(Func<Exception, bool> previous)
		: base(new InnerExceptionAwareCondition(previous).Get) {}
}
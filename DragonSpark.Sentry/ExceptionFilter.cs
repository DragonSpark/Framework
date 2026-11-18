using DragonSpark.Model.Selection.Conditions;
using Sentry.Extensibility;

namespace DragonSpark.Sentry;

public class ExceptionFilter : IExceptionFilter
{
	readonly Func<Exception, bool> _previous;

	protected ExceptionFilter(ICondition<Exception> previous) : this(previous.Get) {}

	protected ExceptionFilter(Func<Exception, bool> previous) => _previous = previous;

	public bool Filter(Exception ex) => _previous(ex);
}
using DragonSpark.Application.Diagnostics;
using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Selection.Conditions;
using Radzen;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Diagnostics;

sealed class SpecificationAwareExceptionNotification : IExceptionNotification
{
	readonly ICondition<Exception>  _condition;
	readonly IExceptionNotification _previous;
	readonly Alter<Exception>       _select;

	public SpecificationAwareExceptionNotification(IExceptionNotification previous)
		: this(Start.A.Condition<Exception>()
		            .By.Calling(x => x is TaskCanceledException || x.InnerException is TaskCanceledException)
		            .Inverse()
		            .Out(),
		       previous, Aggregation.Default.Get) {}

	public SpecificationAwareExceptionNotification(ICondition<Exception> condition, IExceptionNotification previous,
	                                               Alter<Exception> select)
	{
		_condition = condition;
		_previous  = previous;
		_select    = select;
	}

	public NotificationMessage? Get(Exception parameter)
	{
		var exception = _select(parameter);
		var result    = _condition.Get(exception) ? _previous.Get(exception) : null;
		return result;
	}
}
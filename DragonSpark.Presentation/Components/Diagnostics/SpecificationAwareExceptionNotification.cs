using DragonSpark.Application.Diagnostics;
using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Selection.Conditions;
using Radzen;
using System;

namespace DragonSpark.Presentation.Components.Diagnostics;

sealed class SpecificationAwareExceptionNotification : IExceptionNotification
{
	readonly ICondition<Exception>  _condition;
	readonly IExceptionNotification _previous;
	readonly Alter<Exception>       _select;

	public SpecificationAwareExceptionNotification(IExceptionNotification previous)
		: this(previous, Application.AspNet.Diagnostics.AggregateAwareIgnoreException.Default) {}

	public SpecificationAwareExceptionNotification(IExceptionNotification previous, ICondition<Exception> ignore)
		: this(ignore.Then().Inverse().Out(), previous, Flatten.Default.Get) {}

	public SpecificationAwareExceptionNotification(ICondition<Exception> condition, IExceptionNotification previous,
	                                               Alter<Exception> select)
	{
		_condition = condition;
		_previous  = previous;
		_select    = select;
	}

	public NotificationMessage? Get(Exception parameter)
		=> _condition.Get(parameter) ? _previous.Get(_select(parameter)) : null;
}
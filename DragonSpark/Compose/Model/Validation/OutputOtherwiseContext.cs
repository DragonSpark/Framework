using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Compose.Model.Validation;

public class OutputOtherwiseContext<TIn, TOut>
{
	readonly Func<TOut, bool>   _condition;
	readonly ISelect<TIn, TOut> _subject;

	public OutputOtherwiseContext(ISelect<TIn, TOut> subject, Func<TOut, bool> condition)
	{
		_subject   = subject;
		_condition = condition;
	}

	public Selector<TIn, TOut> UseDefault() => Use(Start.A.Selection<TIn>().By.Default<TOut>());

	public Selector<TIn, TOut> Use(ISelect<TIn, TOut> instead) => Use(instead.Get);

	public Selector<TIn, TOut> Use(Func<TIn, TOut> instead)
		=> new ValidatedResult<TIn, TOut>(_condition, _subject.Get, instead).Then();

	public Selector<TIn, TOut> Use(ICommand<TIn> instead) => Use(instead.Then()
	                                                                    .ToConfiguration()
	                                                                    .Default<TOut>());
}
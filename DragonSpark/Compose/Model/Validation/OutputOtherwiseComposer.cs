using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Compose.Model.Validation;

public class OutputOtherwiseComposer<TIn, TOut>
{
	readonly Func<TOut, bool>   _condition;
	readonly ISelect<TIn, TOut> _subject;

	public OutputOtherwiseComposer(ISelect<TIn, TOut> subject, Func<TOut, bool> condition)
	{
		_subject   = subject;
		_condition = condition;
	}

	public Composer<TIn, TOut> UseDefault() => Use(Start.A.Selection<TIn>().By.Default<TOut>());

	public Composer<TIn, TOut> Use(ISelect<TIn, TOut> instead) => Use(instead.Get);

	public Composer<TIn, TOut> Use(Func<TIn, TOut> instead)
		=> new ValidatedResult<TIn, TOut>(_condition, _subject.Get, instead).Then();

	public Composer<TIn, TOut> Use(ICommand<TIn> instead) => Use(instead.Then()
	                                                                    .ToConfiguration()
	                                                                    .Default<TOut>());
}
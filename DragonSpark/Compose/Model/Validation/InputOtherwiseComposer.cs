using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using System;

namespace DragonSpark.Compose.Model.Validation;

public class InputOtherwiseComposer<TIn, TOut>
{
	readonly Func<TIn, bool>    _condition;
	readonly ISelect<TIn, TOut> _subject;

	public InputOtherwiseComposer(ISelect<TIn, TOut> subject, Func<TIn, bool> condition)
	{
		_subject   = subject;
		_condition = condition;
	}

	public ConditionalComposer<TIn, TOut> UseDefault() => Use(Start.A.Selection<TIn>().By.Default<TOut>());

	public ConditionalComposer<TIn, TOut> Use(ISelect<TIn, TOut> instead) => Use(instead.Get);

	public ConditionalComposer<TIn, TOut> Use(Func<TIn, TOut> instead)
		=> new Conditional<TIn, TOut>(_condition, _subject.Get, instead).Then();

	public ConditionalComposer<TIn, TOut> Use(ICommand<TIn> instead) => Use(instead.Then()
	                                                                               .ToConfiguration()
	                                                                               .Default<TOut>());
}
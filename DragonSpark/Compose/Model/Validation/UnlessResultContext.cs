using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using System;

namespace DragonSpark.Compose.Model.Validation;

public sealed class UnlessResultContext<TIn, TOut>
{
	readonly Func<TIn, bool>    _condition;
	readonly ISelect<TIn, TOut> _subject;

	public UnlessResultContext(ISelect<TIn, TOut> subject, Func<TIn, bool> condition)
	{
		_subject   = subject;
		_condition = condition;
	}

	public ConditionalSelector<TIn, TOut> ThenUse(ISelect<TIn, TOut> instead) => ThenUse(instead.Get);

	public ConditionalSelector<TIn, TOut> ThenUse(Func<TIn, TOut> instead)
		=> new Conditional<TIn, TOut>(_condition, instead, _subject.Get).Then();
}
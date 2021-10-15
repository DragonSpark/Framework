using DragonSpark.Compose;
using System;

namespace DragonSpark.Model.Selection.Conditions;

public class SelectedConditional<TFrom, TTo, TOut> : IConditional<TFrom, TOut>
{
	readonly Func<TFrom, TTo>        _select;
	readonly IConditional<TTo, TOut> _source;

	public SelectedConditional(IConditional<TTo, TOut> source, Func<TFrom, TTo> select)
		: this(new Condition<TFrom>(select.Start().Select(source.Condition)), source, select) {}

	public SelectedConditional(ICondition<TFrom> condition, IConditional<TTo, TOut> source, Func<TFrom, TTo> select)
	{
		Condition = condition;
		_select   = select;
		_source   = source;
	}

	public ICondition<TFrom> Condition { get; }

	public TOut Get(TFrom parameter) => _source.Get(_select(parameter));
}
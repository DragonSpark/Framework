using DragonSpark.Compose.Model.Commands;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Selection.Stores;
using System;

namespace DragonSpark.Compose.Extents.Selections;

public sealed class Into<TIn, TOut>
{
	public static Into<TIn, TOut> Default { get; } = new Into<TIn, TOut>();

	Into() {}

	public ITable<TIn, TOut> Table() => Table(_ => default!);


#pragma warning disable 8714
	public ITable<TIn, TOut> Table(Func<TIn, TOut> select) => Tables<TIn, TOut>.Default.Get(select);
#pragma warning restore 8714

	public ICondition<TIn> Condition(Func<TIn, bool> condition) => new Condition<TIn>(condition);

	public IAction<TIn> Action(System.Action<TIn> body) => new Model.Commands.Action<TIn>(body);
}
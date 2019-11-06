using System;
using DragonSpark.Model.Selection.Adapters;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Compose.Selections
{
	public sealed class Into<TIn, TOut>
	{
		public static Into<TIn, TOut> Default { get; } = new Into<TIn, TOut>();

		Into() {}

		public ITable<TIn, TOut> Table() => Table(x => default);

		public ITable<TIn, TOut> Table(Func<TIn, TOut> select) => Tables<TIn, TOut>.Default.Get(select);

		public ICondition<TIn> Condition(Func<TIn, bool> condition)
			=> new Model.Selection.Conditions.Condition<TIn>(condition);

		public IAction<TIn> Action(System.Action<TIn> body) => new Model.Selection.Adapters.Action<TIn>(body);
	}
}
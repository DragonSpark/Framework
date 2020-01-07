using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using System;
using System.Runtime.CompilerServices;

namespace DragonSpark.Model.Selection.Stores
{
	public sealed class ReferenceValueTables<TIn, TOut> : ISelect<ConditionalWeakTable<TIn, TOut>, ITable<TIn, TOut>>
		where TIn : class
		where TOut : class
	{
		public static ReferenceValueTables<TIn, TOut> Default { get; } = new ReferenceValueTables<TIn, TOut>();

		ReferenceValueTables() : this(Default<TIn, TOut>.Instance.ToDelegate()) {}

		ReferenceValueTables(ConditionalWeakTable<TIn, TOut>.CreateValueCallback callback) => _callback = callback;

		readonly ConditionalWeakTable<TIn, TOut>.CreateValueCallback _callback;

		public ReferenceValueTables(Func<TIn, TOut> callback)
			: this(new ConditionalWeakTable<TIn, TOut>.CreateValueCallback(callback)) {}

		public ITable<TIn, TOut> Get(ConditionalWeakTable<TIn, TOut> parameter) => new Table(parameter, _callback);

		sealed class Table : ITable<TIn, TOut>
		{
			readonly ConditionalWeakTable<TIn, TOut>.CreateValueCallback _callback;
			readonly ConditionalWeakTable<TIn, TOut>                     _table;

			public Table(ConditionalWeakTable<TIn, TOut> table,
			             ConditionalWeakTable<TIn, TOut>.CreateValueCallback callback)
				: this(new ConditionalWeakTableContainsAdapter<TIn, TOut>(table), table, callback) {}

			public Table(ICondition<TIn> condition,
			             ConditionalWeakTable<TIn, TOut> table,
			             ConditionalWeakTable<TIn, TOut>.CreateValueCallback callback)
			{
				Condition = condition;
				_table    = table;
				_callback = callback;
			}

			public ICondition<TIn> Condition { get; }

			public TOut Get(TIn parameter) => _table.GetValue(parameter, _callback);

			public void Execute(Pair<TIn, TOut> parameter)
			{
				_table.Remove(parameter.Key);
				_table.Add(parameter.Key, parameter.Value);
			}

			public bool Remove(TIn key) => _table.Remove(key);
		}
	}
}
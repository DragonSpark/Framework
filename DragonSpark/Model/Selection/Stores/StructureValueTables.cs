using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Runtime.Activation;
using System;
using System.Runtime.CompilerServices;

namespace DragonSpark.Model.Selection.Stores
{
	public sealed class StructureValueTables<TIn, TOut> :
		ISelect<ConditionalWeakTable<TIn, Tuple<TOut>>, ITable<TIn, TOut>>,
		IActivateUsing<Func<TIn, TOut>>
		where TIn : class
		where TOut : struct
	{
		public static StructureValueTables<TIn, TOut> Default { get; } = new StructureValueTables<TIn, TOut>();

		StructureValueTables() : this(Default<TIn, TOut>.Instance.ToDelegate()) {}

		readonly ConditionalWeakTable<TIn, Tuple<TOut>>.CreateValueCallback _callback;

		public StructureValueTables(Func<TIn, TOut> source)
			: this(new ConditionalWeakTable<TIn, Tuple<TOut>>.CreateValueCallback(new TupleSelector(source).Get)) {}

		public StructureValueTables(ConditionalWeakTable<TIn, Tuple<TOut>>.CreateValueCallback callback)
			=> _callback = callback;

		public ITable<TIn, TOut> Get(ConditionalWeakTable<TIn, Tuple<TOut>> parameter)
			=> new Table(parameter, _callback);

		sealed class Table : ITable<TIn, TOut>
		{
			readonly ConditionalWeakTable<TIn, Tuple<TOut>>.CreateValueCallback _callback;
			readonly ConditionalWeakTable<TIn, Tuple<TOut>>                     _table;

			public Table(ConditionalWeakTable<TIn, Tuple<TOut>> table,
			             ConditionalWeakTable<TIn, Tuple<TOut>>.CreateValueCallback callback)
				: this(new ConditionalWeakTableContainsAdapter<TIn, Tuple<TOut>>(table), table, callback) {}

			public Table(ICondition<TIn> condition,
			             ConditionalWeakTable<TIn, Tuple<TOut>> table,
			             ConditionalWeakTable<TIn, Tuple<TOut>>.CreateValueCallback callback)
			{
				Condition = condition;
				_table    = table;
				_callback = callback;
			}

			public ICondition<TIn> Condition { get; }

			public TOut Get(TIn parameter) => _table.GetValue(parameter, _callback).Item1;

			public void Execute(Pair<TIn, TOut> parameter)
			{
				_table.Remove(parameter.Key);
				_table.Add(parameter.Key, new Tuple<TOut>(parameter.Value));
			}

			public bool Remove(TIn key) => _table.Remove(key);
		}

		sealed class TupleSelector : ISelect<TIn, Tuple<TOut>>
		{
			readonly Func<TIn, TOut> _source;

			public TupleSelector(Func<TIn, TOut> source) => _source = source;

			public Tuple<TOut> Get(TIn parameter) => Tuple.Create(_source(parameter));
		}
	}
}
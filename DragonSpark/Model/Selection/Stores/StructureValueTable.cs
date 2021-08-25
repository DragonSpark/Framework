using DragonSpark.Model.Selection.Conditions;
using System;
using System.Runtime.CompilerServices;

namespace DragonSpark.Model.Selection.Stores
{
	public class StructureValueTable<TIn, TOut> : ITable<TIn, TOut> where TIn : class
	{
		readonly ConditionalWeakTable<TIn, Tuple<TOut>>.CreateValueCallback _callback;
		readonly ConditionalWeakTable<TIn, Tuple<TOut>>                     _table;

		public StructureValueTable(Func<TIn, TOut> source)
			: this(new ConditionalWeakTable<TIn, Tuple<TOut>>(), source) {}

		public StructureValueTable(ConditionalWeakTable<TIn, Tuple<TOut>> table)
			: this(table, new Func<TIn, TOut>(_ => default!)) {}

		public StructureValueTable(ConditionalWeakTable<TIn, Tuple<TOut>> table, Func<TIn, TOut> source)
			: this(table,
			       new ConditionalWeakTable<TIn, Tuple<TOut>>.CreateValueCallback(new TupleSelector(source).Get)) {}

		public StructureValueTable(ConditionalWeakTable<TIn, Tuple<TOut>> table,
		                           ConditionalWeakTable<TIn, Tuple<TOut>>.CreateValueCallback callback)
			: this(new ConditionalWeakTableContainsAdapter<TIn, Tuple<TOut>>(table), table, callback) {}

		public StructureValueTable(ICondition<TIn> condition,
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

		sealed class TupleSelector : ISelect<TIn, Tuple<TOut>>
		{
			readonly Func<TIn, TOut> _source;

			public TupleSelector(Func<TIn, TOut> source) => _source = source;

			public Tuple<TOut> Get(TIn parameter) => Tuple.Create(_source(parameter));
		}
	}
}
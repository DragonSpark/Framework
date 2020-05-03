using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Runtime.Activation;
using System;
using System.Collections.Concurrent;

namespace DragonSpark.Model.Selection.Stores
{
	public sealed class ConcurrentTables<TIn, TOut> : ISelect<ConcurrentDictionary<TIn, TOut>, ITable<TIn, TOut>>,
	                                                  IActivateUsing<Func<TIn, TOut>>
		where TIn : notnull
	{
		public static ConcurrentTables<TIn, TOut> Default { get; } = new ConcurrentTables<TIn, TOut>();

		ConcurrentTables() : this(Default<TIn, TOut>.Instance.Get) {}

		readonly Func<TIn, TOut> _source;

		public ConcurrentTables(Func<TIn, TOut> source) => _source = source;

		public ITable<TIn, TOut> Get(ConcurrentDictionary<TIn, TOut> parameter) => new Table(parameter, _source);

		sealed class Table : ITable<TIn, TOut>
		{
			readonly Func<TIn, TOut>                 _select;
			readonly ConcurrentDictionary<TIn, TOut> _table;

			public Table(ConcurrentDictionary<TIn, TOut> table, Func<TIn, TOut> select)
				: this(new Condition<TIn>(table.ContainsKey), table, select) {}

			public Table(ICondition<TIn> condition, ConcurrentDictionary<TIn, TOut> table, Func<TIn, TOut> select)
			{
				Condition = condition;
				_table    = table;
				_select   = select;
			}

			public ICondition<TIn> Condition { get; }

			public TOut Get(TIn parameter) => _table.GetOrAdd(parameter, _select);

			public void Execute(Pair<TIn, TOut> parameter)
			{
				_table[parameter.Key] = parameter.Value;
			}

			public bool Remove(TIn key) => _table.TryRemove(key, out _);
		}
	}
}
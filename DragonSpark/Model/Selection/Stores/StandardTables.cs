using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Runtime.Activation;
using System;
using System.Collections.Generic;

namespace DragonSpark.Model.Selection.Stores
{
	public class StandardTables<TIn, TOut> : ISelect<IDictionary<TIn, TOut>, ITable<TIn, TOut>>,
	                                         IActivateUsing<Func<TIn, TOut>>
		where TIn : notnull
	{
		public static StandardTables<TIn, TOut> Default { get; } = new StandardTables<TIn, TOut>();

		StandardTables() : this(Default<TIn, TOut>.Instance.Get) {}

		readonly Func<TIn, TOut> _source;

		public StandardTables(Func<TIn, TOut> source) => _source = source;

		public ITable<TIn, TOut> Get(IDictionary<TIn, TOut> parameter) => new Table(parameter, _source);

		sealed class Table : ITable<TIn, TOut>
		{
			readonly Func<TIn, TOut>        _select;
			readonly IDictionary<TIn, TOut> _table;

			public Table(IDictionary<TIn, TOut> table, Func<TIn, TOut> select)
				: this(new Condition<TIn>(table.ContainsKey), table, select) {}

			public Table(ICondition<TIn> condition, IDictionary<TIn, TOut> table, Func<TIn, TOut> select)
			{
				Condition = condition;
				_table    = table;
				_select   = select;
			}

			public ICondition<TIn> Condition { get; }

			public TOut Get(TIn parameter)
			{
				if (_table.TryGetValue(parameter, out var existing))
				{
					return existing;
				}

				var result = _select(parameter);
				_table[parameter] = result;
				return result;
			}

			public void Execute(Pair<TIn, TOut> parameter)
			{
				_table[parameter.Key] = parameter.Value;
			}

			public bool Remove(TIn key) => _table.Remove(key);
		}
	}
}
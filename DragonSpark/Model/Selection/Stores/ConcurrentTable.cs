using DragonSpark.Model.Selection.Conditions;
using System;
using System.Collections.Concurrent;

namespace DragonSpark.Model.Selection.Stores;

public class ConcurrentTable<TIn, TOut> : ITable<TIn, TOut> where TIn : notnull
{
	readonly Func<TIn, TOut>                 _select;
	readonly ConcurrentDictionary<TIn, TOut> _table;

	public ConcurrentTable(Func<TIn, TOut> select) : this(new ConcurrentDictionary<TIn, TOut>(), @select) {}

	public ConcurrentTable(ConcurrentDictionary<TIn, TOut> table)
		: this(table, _ => default!) {}

	public ConcurrentTable(ConcurrentDictionary<TIn, TOut> table, Func<TIn, TOut> select)
		: this(new Condition<TIn>(table.ContainsKey), table, select) {}

	public ConcurrentTable(ICondition<TIn> condition, ConcurrentDictionary<TIn, TOut> table, Func<TIn, TOut> select)
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
using DragonSpark.Model.Selection.Conditions;
using System;
using System.Runtime.CompilerServices;

namespace DragonSpark.Model.Selection.Stores;

public class ReferenceValueTable<TIn, TOut> : ITable<TIn, TOut> where TIn : class where TOut : class?
{
	readonly ConditionalWeakTable<TIn, TOut>.CreateValueCallback _callback;
	readonly ConditionalWeakTable<TIn, TOut>                     _table;

	public ReferenceValueTable() : this(new ConditionalWeakTable<TIn, TOut>()) {}

	public ReferenceValueTable(ConditionalWeakTable<TIn, TOut> table)
		: this(table, new ConditionalWeakTable<TIn, TOut>.CreateValueCallback(_ => default!)) {}

	public ReferenceValueTable(Func<TIn, TOut> factory) : this(new ConditionalWeakTable<TIn, TOut>(), factory) {}

	public ReferenceValueTable(ConditionalWeakTable<TIn, TOut> table, Func<TIn, TOut> factory)
		: this(table, new ConditionalWeakTable<TIn, TOut>.CreateValueCallback(factory)) {}

	public ReferenceValueTable(ConditionalWeakTable<TIn, TOut> table,
	                           ConditionalWeakTable<TIn, TOut>.CreateValueCallback callback)
		: this(new ConditionalWeakTableContainsAdapter<TIn, TOut>(table), table, callback) {}

	public ReferenceValueTable(ICondition<TIn> condition,
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
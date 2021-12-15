using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Conditions;
using System;
using System.Collections.Generic;

namespace DragonSpark.Model.Selection.Stores;

public class StandardTable<TIn, TOut> : ITable<TIn, TOut>, ICommand where TIn : notnull
{
	readonly Func<TIn, TOut>        _select;
	readonly IDictionary<TIn, TOut> _table;

	public StandardTable(IDictionary<TIn, TOut> table) : this(table, _ => default!) {}

	public StandardTable(Func<TIn, TOut> select) : this(new Dictionary<TIn, TOut>(), @select) {}

	public StandardTable(IDictionary<TIn, TOut> table, Func<TIn, TOut> select)
		: this(new Condition<TIn>(table.ContainsKey), table, select) {}

	public StandardTable(ICondition<TIn> condition, IDictionary<TIn, TOut> table, Func<TIn, TOut> select)
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

	public void Execute(None parameter)
	{
		_table.Clear();
	}
}
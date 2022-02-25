using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;
using System;

namespace DragonSpark.Model.Selection.Stores;

public class ReferenceVariable<TIn, TOut> : ITable<TIn, TOut?> where TIn : class
{
	readonly ITable<TIn, IMutable<TOut?>> _store;

	public ReferenceVariable() : this(_ => default!) {}

	public ReferenceVariable(Func<TIn, TOut> create)
		: this(new ReferenceValueTable<TIn, IMutable<TOut?>>(@in => new Variable<TOut>(create(@in)))) {}

	public ReferenceVariable(ITable<TIn, IMutable<TOut?>> store) : this(store.Condition, store) {}

	public ReferenceVariable(ICondition<TIn> condition, ITable<TIn, IMutable<TOut?>> store)
	{
		Condition = condition;
		_store    = store;
	}

	public ICondition<TIn> Condition { get; }

	public TOut? Get(TIn parameter) => _store.Get(parameter).Get();

	public void Execute(Pair<TIn, TOut?> parameter)
	{
		var (key, value) = parameter;
		_store.Get(key).Execute(value);
	}

	public bool Remove(TIn key) => _store.Remove(key);
}
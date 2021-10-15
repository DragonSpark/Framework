using System.Collections.Generic;

namespace DragonSpark.Model.Selection.Stores;

public class Table<TIn, TOut> : Lookup<TIn, TOut>, ITable<TIn, TOut>
	where TIn : notnull
{
	readonly IDictionary<TIn, TOut> _store;

	public Table() : this(new Dictionary<TIn, TOut>()) {}

	public Table(IDictionary<TIn, TOut> store) : base(store) => _store = store;

	public bool Remove(TIn key) => _store.Remove(key);

	public void Execute(Pair<TIn, TOut> parameter) => _store[parameter.Key] = parameter.Value;
}
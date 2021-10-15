using DragonSpark.Model.Sequences;
using System.Collections.Generic;

namespace DragonSpark.Model.Selection.Stores;

public class TableValues<TIn, TOut> : DecoratedTable<TIn, TOut>, ITableValues<TIn, TOut> where TIn : notnull
{
	public TableValues(IDictionary<TIn, TOut> store)
		: this(new Table<TIn, TOut>(store), new DeferredArray<TOut>(store.Values)) {}

	public TableValues(ITable<TIn, TOut> source, IArray<TOut> values) : base(source) => Values = values;

	public IArray<TOut> Values { get; }
}
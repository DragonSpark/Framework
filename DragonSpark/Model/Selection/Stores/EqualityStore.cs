using System;
using System.Collections.Generic;

namespace DragonSpark.Model.Selection.Stores;

public class EqualityStore<TIn, TOut> : Select<TIn, TOut> where TIn : notnull
{
	protected EqualityStore(ISelect<TIn, TOut> source, IDictionary<TIn, TOut> store)
		: this(source.Get, store) {}

	protected EqualityStore(Func<TIn, TOut> source, IDictionary<TIn, TOut> store)
		: base(new StandardTable<TIn, TOut>(store, source)) {}
}
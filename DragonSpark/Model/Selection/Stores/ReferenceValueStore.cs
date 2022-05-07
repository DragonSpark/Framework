using System;

namespace DragonSpark.Model.Selection.Stores;

public class ReferenceValueStore<TIn, TOut> : Select<TIn, TOut> where TIn : class where TOut : class?
{
	protected ReferenceValueStore(ISelect<TIn, TOut> select) : this(select.Get) {}

	protected ReferenceValueStore(Func<TIn, TOut> source) : base(new ReferenceValueTable<TIn, TOut>(source)) {}
}
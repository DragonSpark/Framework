using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public class ReferenceStoring<TIn, TOut> : Storing<TIn, TOut> where TIn : class where TOut : class?
{
	protected ReferenceStoring(ISelect<TIn, ValueTask<TOut>> source) : this(source.Get) {}

	protected ReferenceStoring(Func<TIn, ValueTask<TOut>> source) : this(new ReferenceValueTable<TIn, TOut>(), source) {}

	protected ReferenceStoring(ITable<TIn, TOut> store, Func<TIn, ValueTask<TOut>> source) : base(store, source) {}
}
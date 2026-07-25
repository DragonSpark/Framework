using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Model.Operations.Selection.Stop;

public class ReferenceStoring<TIn, TOut> : Storing<TIn, TOut> where TIn : class where TOut : class?
{
	public ReferenceStoring(ISelect<Stop<TIn>, ValueTask<TOut>> previous) : this(previous.Get) {}

	protected ReferenceStoring(Func<Stop<TIn>, ValueTask<TOut>> source)
		: this(new ReferenceValueTable<TIn, TOut>(), source) {}

	protected ReferenceStoring(ITable<TIn, TOut> store, Func<Stop<TIn>, ValueTask<TOut>> source)
		: base(store, source) {}
}
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime.Activation;

namespace DragonSpark.Runtime.Invocation
{
	sealed class Striped<TIn, TOut> : Select<TIn, TOut>, IActivateUsing<ISelect<TIn, TOut>>
	{
		public Striped(ISelect<TIn, TOut> select) : base(Start.An.Extent<Deferred<TIn, TOut>>()
		                                                      .From(select)
		                                                      .ToDelegate()
		                                                      .To(Start.An.Extent<Stripe<TIn, TOut>>())
		                                                      .Unless(select)) {}
	}
}
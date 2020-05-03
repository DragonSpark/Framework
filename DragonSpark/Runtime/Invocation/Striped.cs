using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime.Activation;

namespace DragonSpark.Runtime.Invocation
{
	sealed class Striped<TIn, TOut> : Select<TIn, TOut>, IActivateUsing<ISelect<TIn, TOut>> where TIn : notnull
	{
		public Striped(ISelect<TIn, TOut> select) : base(Start.An.Extent<Deferred<TIn, TOut>>()
		                                                      .From(select)
		                                                      .ToDelegate()
		                                                      .To(Start.An.Extent<Stripe<TIn, TOut>>())
		                                                      .Then()
		                                                      .Unless.Using(select)
		                                                      .ResultsInAssigned()) {}
	}
}
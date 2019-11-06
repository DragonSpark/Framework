using DragonSpark.Model.Selection;
using DragonSpark.Reflection;
using DragonSpark.Runtime.Activation;

namespace DragonSpark.Runtime.Invocation
{
	sealed class Striped<TIn, TOut> : Select<TIn, TOut>, IActivateUsing<ISelect<TIn, TOut>>
	{
		public Striped(ISelect<TIn, TOut> select) : base(select.To(I.A<Deferred<TIn, TOut>>())
		                                                       .ToDelegate()
		                                                       .To(I.A<Stripe<TIn, TOut>>())
		                                                       .Unless(select)) {}
	}
}
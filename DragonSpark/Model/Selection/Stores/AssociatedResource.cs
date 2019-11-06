using System;
using DragonSpark.Compose;

namespace DragonSpark.Model.Selection.Stores
{
	public class AssociatedResource<TIn, TOut> : DecoratedTable<TIn, TOut>
	{
		public AssociatedResource() : this(Start.A.Selection<TIn>().AndOf<TOut>().By.Activation().Get) {}

		public AssociatedResource(Func<TIn, TOut> resource) : base(Tables<TIn, TOut>.Default.Get(resource)) {}
	}
}
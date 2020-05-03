using DragonSpark.Compose;
using System;

namespace DragonSpark.Model.Selection.Stores
{
	public class AssociatedResource<TIn, TOut> : DecoratedTable<TIn, TOut> where TIn : notnull
	{
		public AssociatedResource() : this(Start.A.Selection<TIn>().AndOf<TOut>().By.Activation()) {}

		public AssociatedResource(Func<TIn, TOut> resource) : base(Tables<TIn, TOut>.Default.Get(resource)) {}
	}
}
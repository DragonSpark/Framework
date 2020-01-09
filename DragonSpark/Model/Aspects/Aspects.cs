using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime.Environment;
using System;

namespace DragonSpark.Model.Aspects
{
	public sealed class Aspects<TIn, TOut> : Select<ISelect<TIn, TOut>, IAspect<TIn, TOut>>
	{
		public Aspects() : this(new AspectRegistry()) {}

		public Aspects(IRegistry<IRegistration> registry)
			: this(new AspectLocator<TIn, TOut>(new AspectRegistrations<TIn, TOut>(registry))) {}

		public Aspects(ISelect<Type, IAspect<TIn, TOut>> locator)
			: base(Start.A.Selection<ISelect<TIn, TOut>>()
			            .By.Type.Select(locator.Then()
			                                   .Stores()
			                                   .New())
			      ) {}
	}
}
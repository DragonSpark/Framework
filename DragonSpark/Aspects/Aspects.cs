using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime.Environment;

namespace DragonSpark.Aspects
{
	public sealed class Aspects<TIn, TOut> : Select<ISelect<TIn, TOut>, IAspect<TIn, TOut>>
	{
		public Aspects() : this(new AspectRegistry()) {}

		public Aspects(IRegistry<IRegistration> registry)
			: base(Start.A.Selection<ISelect<TIn, TOut>>()
			            .By.Type.Then()
			            .Select(Start
			                    .An
			                    .Instance(new AspectLocator<TIn, TOut>(new AspectRegistrations<TIn, TOut>(registry)))
			                    .Stores()
			                    .New())
			      ) {}
	}
}
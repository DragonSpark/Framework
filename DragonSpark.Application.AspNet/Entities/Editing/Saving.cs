using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.AspNet.Entities.Editing;

public class Saving<TIn, TOut> : StopAwareConfiguringResult<TIn, TOut> where TOut : class
{
	protected Saving(IStopAware<TIn, TOut> @new, Save<TOut> add) : base(@new, add) {}

	protected Saving(IStopAware<TIn, TOut> select, IStopAware<TOut> operation) : base(select, operation) {}

	public Saving(Await<Stop<TIn>, TOut> select, Await<Stop<TOut>> configure) : base(select, configure) {}
}
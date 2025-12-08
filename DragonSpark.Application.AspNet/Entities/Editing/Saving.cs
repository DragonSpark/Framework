using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.AspNet.Entities.Editing;

public class Saving<TIn, TOut> : StopAwareConfiguringResult<TIn, TOut> where TOut : class
{
	protected Saving(IStopAware<TIn, TOut> compose, Save<TOut> add) : base(compose, add) {}

	protected Saving(IStopAware<TIn, TOut> select, IStopAware<TOut> operation) : base(select, operation) {}

	public Saving(Await<Stop<TIn>, TOut> select, Await<Stop<TOut>> configure) : base(select, configure) {}
}
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Entities.Editing;

public class Saving<TIn, TOut> : ConfiguringResult<TIn, TOut> where TOut : class
{
	protected Saving(ISelecting<TIn, TOut> @new, Save<TOut> add) : base(@new, add) {}

	protected Saving(ISelecting<TIn, TOut> select, IOperation<TOut> operation) : base(select, operation) {}

	protected Saving(Await<TIn, TOut> @new, Await<TOut> add) : base(@new, add) {}
}
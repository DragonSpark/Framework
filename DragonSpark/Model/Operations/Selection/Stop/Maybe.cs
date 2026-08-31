using DragonSpark.Compose;

namespace DragonSpark.Model.Operations.Selection.Stop;

public class Maybe<TIn, TOut> : Selection.Maybe<Stop<TIn>, TOut?>, IStopAware<TIn, TOut?>
{
	protected Maybe(ISelecting<Stop<TIn>, TOut?> first, ISelecting<Stop<TIn>, TOut?> second)
		: this(first.Off, second.Off) {}

	protected Maybe(Await<Stop<TIn>, TOut?> first, Await<Stop<TIn>, TOut?> second) : base(first, second) {}
}
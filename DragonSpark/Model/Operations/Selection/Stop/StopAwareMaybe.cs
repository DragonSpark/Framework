using DragonSpark.Compose;

namespace DragonSpark.Model.Operations.Selection.Stop;

public class StopAwareMaybe<TIn, TOut> : Maybe<Stop<TIn>, TOut?>, IStopAware<TIn, TOut?>
{
	public StopAwareMaybe(ISelecting<Stop<TIn>, TOut?> first, ISelecting<Stop<TIn>, TOut?> second)
		: this(first.Off, second.Off) {}

	public StopAwareMaybe(Await<Stop<TIn>, TOut?> first, Await<Stop<TIn>, TOut?> second) : base(first, second) {}
}
namespace DragonSpark.Model.Operations.Selection.Stop;

public interface IStopAdaptor<TIn, TOut> : IStopAware<TIn, TOut>
{
	ISelecting<TIn, TOut> Alternate { get; }
}
namespace DragonSpark.Model.Operations.Stop;

public class StopAdaptor<T> : StopAware<T>, IStopAdaptor<T>
{
	protected StopAdaptor(IStopAware<T> stop) : this(stop, new StopAwareOperationAdapter<T>(stop)) {}

	protected StopAdaptor(IStopAware<T> stop, IOperation<T> selecting)
		: base(stop) => Alternate = selecting;

	public IOperation<T> Alternate { get; }
}
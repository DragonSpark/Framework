namespace DragonSpark.Model.Operations.Stop;

public interface IStopAdaptor<T> : IStopAware<T>
{
	IOperation<T> Alternate { get; }
}
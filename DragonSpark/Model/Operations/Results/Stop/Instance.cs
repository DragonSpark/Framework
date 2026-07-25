namespace DragonSpark.Model.Operations.Results.Stop;

public class Instance<T> : DragonSpark.Model.Operations.Results.Instance<T>, IStopAware<T>
{
    protected Instance(T instance) : base(instance) {}

    public ValueTask<T> Get(CancellationToken parameter) => Get();
}
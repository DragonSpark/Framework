using System;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Operations.Results.Stop;

public class StopAware<T> : Selecting<CancellationToken, T>, IStopAware<T>
{
	public StopAware(ISelect<CancellationToken, ValueTask<T>> select) : base(select) {}

	public StopAware(Func<CancellationToken, ValueTask<T>> select) : base(select) {}
}

// TODO

public class Instance<T> : DragonSpark.Model.Operations.Results.Instance<T>, IStopAware<T>
{
    protected Instance(T instance) : base(instance) {}

    public ValueTask<T> Get(CancellationToken parameter) => Get();
}
using System;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Operations.Stop;

public class StopAware : Select<CancellationToken, ValueTask>, IStopAware
{
    public StopAware(ISelect<CancellationToken, ValueTask> select) : base(select) {}

    public StopAware(Func<CancellationToken, ValueTask> select) : base(select) {}
}

public class StopAware<T> : Operation<Stop<T>>, IStopAware<T>
{
    protected StopAware(ISelect<Stop<T>, ValueTask> select) : base(select) {}

    public StopAware(Func<Stop<T>, ValueTask> select) : base(select) {}
}
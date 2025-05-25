using DragonSpark.Model.Selection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Allocated;

public interface IAllocatedStopAware<T, TOut> : ISelect<Stop<T>, Task<TOut>>;

public interface IAllocatedStopAware<T> : ISelect<Stop<T>, Task>;
public interface IAllocatedStopAware : ISelect<CancellationToken, Task>;

// TODO
public class AllocatedStopAware<T> : Select<Stop<T>, Task>, IAllocatedStopAware<T>
{
	public AllocatedStopAware(ISelect<Stop<T>, Task> select) : base(select) {}

	public AllocatedStopAware(Func<Stop<T>, Task> select) : base(select) {}
}
using DragonSpark.Model.Selection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Stop;

public interface IStopAware<T> : IOperation<Stop<T>>;

public interface IStopAware : IOperation<CancellationToken>;

public class StopAware : Select<CancellationToken, ValueTask>, IStopAware
{
	public StopAware(ISelect<CancellationToken, ValueTask> select) : base(select) {}

	public StopAware(Func<CancellationToken, ValueTask> select) : base(select) {}
}
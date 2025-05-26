using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Selection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Results.Stop;

public class StopAware<T> : Selecting<CancellationToken, T>, IStopAware<T>
{
	public StopAware(ISelect<CancellationToken, ValueTask<T>> select) : base(select) {}

	public StopAware(Func<CancellationToken, ValueTask<T>> select) : base(select) {}
}
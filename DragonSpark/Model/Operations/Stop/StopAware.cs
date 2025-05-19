using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Stop;

public class StopAware<T> : Operation<Stop<T>>, IStopAware<T>
{
	public StopAware(ISelect<Stop<T>, ValueTask> select) : base(select) {}

	public StopAware(Func<Stop<T>, ValueTask> select) : base(select) {}
}
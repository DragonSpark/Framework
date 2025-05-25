using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Allocated;

public class AllocatedStopAware<T> : Select<Stop<T>, Task>, IAllocatedStopAware<T>
{
	public AllocatedStopAware(ISelect<Stop<T>, Task> select) : base(select) {}

	public AllocatedStopAware(Func<Stop<T>, Task> select) : base(select) {}
}
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Allocated.Stop;

public class Allocated<T> : Select<Stop<T>, Task>, IAllocated<T>
{
	public Allocated(ISelect<Stop<T>, Task> select) : base(select) {}

	protected Allocated(Func<Stop<T>, Task> select) : base(select) {}
}
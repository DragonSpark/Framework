using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Allocated;

public class AllocatedOperation<T> : Select<T, Task>, IAllocated<T>
{
	public AllocatedOperation(ISelect<T, Task> @select) : base(@select) {}

	public AllocatedOperation(Func<T, Task> @select) : base(@select) {}
}
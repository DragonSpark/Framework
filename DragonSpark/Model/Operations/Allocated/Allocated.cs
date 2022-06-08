using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Allocated;

public class Allocated : Result<Task>, IAllocated
{
	public Allocated(IResult<Task> result) : base(result) {}

	public Allocated(Func<Task> source) : base(source) {}
}

public class Allocated<T> : Select<T, Task>, IAllocated<T>
{
	public Allocated(ISelect<T, Task> @select) : base(@select) {}

	public Allocated(Func<T, Task> @select) : base(@select) {}
}
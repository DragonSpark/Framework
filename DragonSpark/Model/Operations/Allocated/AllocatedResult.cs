using DragonSpark.Model.Results;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Allocated;

public class AllocatedResult<T> : Result<Task<T>>, IAllocatedResult<T>
{
	public AllocatedResult(IResult<Task<T>> result) : base(result) {}

	public AllocatedResult(Func<Task<T>> source) : base(source) {}
}
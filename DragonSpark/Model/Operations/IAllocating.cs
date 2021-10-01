using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations
{
	public interface IAllocatedResult<T> : IResult<Task<T>> {}

	public class AllocatedResult<T> : Result<Task<T>>, IAllocatedResult<T>
	{
		public AllocatedResult(IResult<Task<T>> result) : base(result) {}

		public AllocatedResult(Func<Task<T>> source) : base(source) {}
	}

	public interface IAllocating<in T, TOut> : ISelect<T, Task<TOut>> {}

	public class Allocating<T, TOut> : Select<T, Task<TOut>>, IAllocating<T, TOut>
	{
		public Allocating(ISelect<T, Task<TOut>> @select) : base(@select) {}

		public Allocating(Func<T, Task<TOut>> @select) : base(@select) {}
	}
}
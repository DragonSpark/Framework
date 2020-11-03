using DragonSpark.Model.Results;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations
{
	public class OperationResult<T> : Result<ValueTask<T>>, IResulting<T>
	{
		public OperationResult(IResult<ValueTask<T>> result) : base(result) {}

		public OperationResult(Func<ValueTask<T>> source) : base(source) {}
	}
}
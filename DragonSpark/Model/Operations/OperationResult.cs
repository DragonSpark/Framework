using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations
{
	public class OperationResult<TIn, TOut> : Select<TIn, ValueTask<TOut>>, IOperationResult<TIn, TOut>
	{
		public OperationResult(ISelect<TIn, ValueTask<TOut>> select) : this(select.Get) {}

		public OperationResult(Func<TIn, ValueTask<TOut>> select) : base(select) {}
	}

	public class OperationResult<T> : Result<ValueTask<T>>, IOperationResult<T>
	{
		public OperationResult(IResult<ValueTask<T>> result) : base(result) {}

		public OperationResult(Func<ValueTask<T>> source) : base(source) {}
	}
}
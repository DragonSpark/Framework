using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Operations
{
	public class OperationResult<TIn, TOut> : Select<TIn, ValueTask<TOut>>, IOperationResult<TIn, TOut>
	{
		public OperationResult(ISelect<TIn, ValueTask<TOut>> select) : this(select.Get) {}

		public OperationResult(Func<TIn, ValueTask<TOut>> select) : base(select) {}
	}

	public class OperationResult<T> : Instance<ValueTask<T>>, IOperationResult<T>
	{
		public OperationResult(Task<T> instance) : this(new ValueTask<T>(instance)) {}

		public OperationResult(T instance) : this(new ValueTask<T>(instance)) {}

		public OperationResult(ValueTask<T> instance) : base(instance) {}
	}
}
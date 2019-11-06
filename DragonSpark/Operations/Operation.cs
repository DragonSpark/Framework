using System;
using System.Threading.Tasks;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;

namespace DragonSpark.Operations
{
	public class Operation<TIn, TOut> : Select<TIn, ValueTask<TOut>>, IOperation<TIn, TOut>
	{
		public Operation(Func<TIn, ValueTask<TOut>> select) : base(select) {}
	}

	class Operation<T> : Instance<ValueTask<T>>, IOperation<T>
	{
		public Operation(Task<T> instance) : this(new ValueTask<T>(instance)) {}

		public Operation(T instance) : this(new ValueTask<T>(instance)) {}

		public Operation(ValueTask<T> instance) : base(instance) {}
	}
}
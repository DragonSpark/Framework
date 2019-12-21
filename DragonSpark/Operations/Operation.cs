using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Operations
{
	public class Operation<T> : Select<T, ValueTask>, IOperation<T>
	{
		public Operation(Func<T, ValueTask> select) : base(select) {}
	}

	public class Operation : Instance<ValueTask>, IOperation
	{
		public Operation(Task instance) : this(new ValueTask(instance)) {}

		public Operation(ValueTask instance) : base(instance) {}
	}
}
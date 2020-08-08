using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations
{
	public class Operation<T> : Select<T, ValueTask>, IOperation<T>
	{
		public Operation(Func<T, ValueTask> select) : base(select) {}
	}

	
}
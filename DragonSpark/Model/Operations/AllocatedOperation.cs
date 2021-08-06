using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations
{
	public class AllocatedOperation<T> : Select<T, Task>
	{
		public AllocatedOperation(ISelect<T, Task> @select) : base(@select) {}

		public AllocatedOperation(Func<T, Task> @select) : base(@select) {}
	}
}

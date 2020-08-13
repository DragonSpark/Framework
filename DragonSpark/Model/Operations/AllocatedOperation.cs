using DragonSpark.Model.Selection;
using JetBrains.Annotations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations
{
	public class AllocatedOperation<T> : Select<T, Task>
	{
		public AllocatedOperation([NotNull] ISelect<T, Task> @select) : base(@select) {}

		public AllocatedOperation([NotNull] Func<T, Task> @select) : base(@select) {}
	}
}

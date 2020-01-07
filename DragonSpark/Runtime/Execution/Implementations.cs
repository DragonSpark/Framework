using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using System;

namespace DragonSpark.Runtime.Execution
{
	static class Implementations
	{
		public static IAssign<object, IDisposable> Resources { get; } = AssociatedResources.Default.ToAssignment();
	}
}
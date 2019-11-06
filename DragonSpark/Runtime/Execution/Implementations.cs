using System;
using DragonSpark.Model.Commands;

namespace DragonSpark.Runtime.Execution
{
	static class Implementations
	{
		public static IAssign<object, IDisposable> Resources { get; } = AssociatedResources.Default.ToAssignment();
	}
}
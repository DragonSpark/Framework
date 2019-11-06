using System;

namespace DragonSpark.Application.Hosting.xUnit
{
	[AttributeUsage(AttributeTargets.Method)]
	public class TestPriorityAttribute : Attribute
	{
		public TestPriorityAttribute(int priority) => Priority = priority;

		public int Priority { get; }
	}
}
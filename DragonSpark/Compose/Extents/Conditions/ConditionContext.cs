using DragonSpark.Compose.Model.Selection;
using System;

namespace DragonSpark.Compose.Extents.Conditions
{
	public interface IConditionContext
	{
		ConditionExtent Of { get; }
	}

	public sealed class ConditionContext : IConditionContext
	{
		public static ConditionContext Default { get; } = new ConditionContext();

		ConditionContext() {}

		public ConditionExtent Of => ConditionExtent.Default;
	}

	public sealed class ConditionContext<T>
	{
		public static ConditionContext<T> Instance { get; } = new ConditionContext<T>();

		ConditionContext() {}

		public ConditionSelector<T> Always => Is.Always<T>();

		public ConditionSelector<T> Never => Is.Never<T>();

		public ConditionSelector<T> Assigned => Is.Assigned<T>();

		public ConditionSelector<T> Calling(Func<T, bool> condition)
			=> new DragonSpark.Model.Selection.Conditions.Condition<T>(condition).Then();
	}
}
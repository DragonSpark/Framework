using DragonSpark.Compose.Model.Selection;
using System;

namespace DragonSpark.Compose.Extents.Conditions;

public sealed class ConditionContext : IConditionContext
{
	public static ConditionContext Default { get; } = new();

	ConditionContext() {}

	public ConditionExtent Of => ConditionExtent.Default;
}

public sealed class ConditionContext<T>
{
	public static ConditionContext<T> Instance { get; } = new();

	ConditionContext() {}

	public ConditionComposer<T> Always => Is.Always<T>();

	public ConditionComposer<T> Never => Is.Never<T>();

	public ConditionComposer<T> Assigned => Is.Assigned<T>();

	public ConditionComposer<T> Calling(Func<T, bool> condition)
		=> new DragonSpark.Model.Selection.Conditions.Condition<T>(condition).Then();
}
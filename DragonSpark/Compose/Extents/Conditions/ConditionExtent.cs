using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;
using System;
using System.Reflection;

namespace DragonSpark.Compose.Extents.Conditions;

public sealed class ConditionExtent
{
	public static ConditionExtent Default { get; } = new ConditionExtent();

	ConditionExtent() {}

	public SystemExtents System => SystemExtents.Instance;
	public ConditionExtent<object> Any => DefaultConditionExtent<object>.Default;
	public ConditionExtent<None> None => DefaultConditionExtent<None>.Default;

	public ConditionExtent<T> Type<T>() => DefaultConditionExtent<T>.Default;

	public sealed class SystemExtents
	{
		public static SystemExtents Instance { get; } = new SystemExtents();

		SystemExtents() {}

		public ConditionExtent<Type> Type => DefaultConditionExtent<Type>.Default;

		public ConditionExtent<TypeInfo> Metadata => DefaultConditionExtent<TypeInfo>.Default;
	}
}

public class ConditionExtent<T>
{
	protected ConditionExtent() {}

	public Selections As => Selections.Instance;

	public ConditionContext<T> By => ConditionContext<T>.Instance;

	public class Selections
	{
		public static Selections Instance { get; } = new Selections();

		Selections() {}

		public SequenceConditionExtent<T> Sequence => SequenceConditionExtent<T>.Default;
		public ConditionExtent<Func<T>> Delegate => DefaultConditionExtent<Func<T>>.Default;
		public ConditionExtent<ICondition<T>> Condition => DefaultConditionExtent<ICondition<T>>.Default;
		public ConditionExtent<IResult<T>> Result => DefaultConditionExtent<IResult<T>>.Default;
		public ConditionExtent<ICommand<T>> Command => DefaultConditionExtent<ICommand<T>>.Default;
	}
}
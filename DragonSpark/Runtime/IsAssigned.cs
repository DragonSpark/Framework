using DragonSpark.Compose;
using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using System;

namespace DragonSpark.Runtime;

sealed class IsAssigned : InverseCondition<object>
{
	public static IsAssigned Default { get; } = new();

	IsAssigned() : base(IsNullReference.Default) {}
}

sealed class IsAssigned<T> : Select<T, bool>, ICondition<T>
{
	public static IsAssigned<T> Default { get; } = new();

	IsAssigned() : base(IsAssignedConditions<T>.Default.Get(typeof(T))) {}
}

public class IsAssigned<TIn, TOut> : Model.Selection.Conditions.Condition<TIn> where TOut : class
{
	protected IsAssigned(Func<TIn, TOut> select) : this(select.Start()) {}

	protected IsAssigned(Composer<TIn, TOut> source) : base(source.Select<bool>(Is.Assigned())) {}
}
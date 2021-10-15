using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Runtime.Activation;
using System;

namespace DragonSpark.Reflection.Types;

sealed class IsAssignableFrom<T> : Condition<Type>
{
	public static IsAssignableFrom<T> Default { get; } = new IsAssignableFrom<T>();

	IsAssignableFrom() : base(new IsAssignableFrom(A.Metadata<T>())) {}
}

public sealed class IsAssignableFrom : Condition<Type>, IActivateUsing<Type>
{
	public IsAssignableFrom(Type type) : base(type.IsAssignableFrom) {}
}
using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Reflection;
using DragonSpark.Reflection.Types;
using DragonSpark.Runtime;
using System;
using System.Reflection;

namespace DragonSpark.Compose;

public static class Is
{
	public static ConditionComposer<ICustomAttributeProvider> DecoratedWith<T>() where T : Attribute
		=> IsDecoratedWith<T>.Default.Then();

	public static ConditionComposer<T> Always<T>()
		=> DragonSpark.Model.Selection.Conditions.Always<T>.Default.Then();

	public static ConditionComposer<object> Always() => Always<object>();

	public static ConditionComposer<T> Never<T>() => DragonSpark.Model.Selection.Conditions.Never<T>.Default.Then();

	public static ConditionComposer<object> Never() => Always<object>();

	public static ConditionComposer<T> EqualTo<T>(T source) => new Equals<T>(source).Then();

	public static ConditionComposer<object> Of<T>() => IsOf<T>.Default.Then();

	public static ConditionComposer<T> Assigned<T>() => IsAssigned<T>.Default.Then();

	public static ConditionComposer<object> Assigned() => IsAssigned.Default.Then();

	public static ConditionComposer<Type> AssignableFrom<T>() => IsAssignableFrom<T>.Default.Then();
}
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
	public static ConditionSelector<ICustomAttributeProvider> DecoratedWith<T>() where T : Attribute
		=> IsDecoratedWith<T>.Default.Then();

	public static ConditionSelector<T> Always<T>()
		=> DragonSpark.Model.Selection.Conditions.Always<T>.Default.Then();

	public static ConditionSelector<object> Always() => Always<object>();

	public static ConditionSelector<T> Never<T>() => DragonSpark.Model.Selection.Conditions.Never<T>.Default.Then();

	public static ConditionSelector<object> Never() => Always<object>();

	public static ConditionSelector<T> EqualTo<T>(T source) => new Equals<T>(source).Then();

	public static ConditionSelector<object> Of<T>() => IsOf<T>.Default.Then();

	public static ConditionSelector<T> Assigned<T>() => IsAssigned<T>.Default.Then();

	public static ConditionSelector<object> Assigned() => IsAssigned.Default.Then();

	public static ConditionSelector<Type> AssignableFrom<T>() => IsAssignableFrom<T>.Default.Then();
}
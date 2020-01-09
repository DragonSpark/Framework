using DragonSpark.Compose.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using DragonSpark.Reflection;
using DragonSpark.Reflection.Types;
using DragonSpark.Runtime;
using System;
using System.Reflection;

namespace DragonSpark.Compose
{
	public static class Is
	{
		public static ICondition<ICustomAttributeProvider> DecoratedWith<T>() where T : Attribute
			=> IsDecoratedWith<T>.Default;

		public static ConditionSelector<T> Always<T>() => DragonSpark.Model.Selection.Conditions.Always<T>.Default.Then();

		public static ConditionSelector<object> Always() => Always<object>();

		public static ConditionSelector<T> Never<T>() => DragonSpark.Model.Selection.Conditions.Never<T>.Default.Then();

		public static ConditionSelector<object> Never() => Always<object>();

		public static ConditionSelector<T> EqualTo<T>(T source) => new Equals<T>(source).Then();

		public static ConditionSelector<object> Of<T>() => IsOf<T>.Default.Then();

		public static ConditionSelector<T> Assigned<T>() => IsAssigned<T>.Default.Then();

		public static ConditionSelector<object> Assigned() => IsAssigned.Default.Then();

		public static ConditionSelector<Type> AssignableFrom<T>() => IsAssignableFrom<T>.Default.Then();
	}

	public static class A
	{
		public static T Default<T>() => DragonSpark.Model.Results.Default<T>.Instance.Get();

		public static T Of<T>() => Start.An.Instance<T>();

		public static ICommand<T> Command<T>(ICommand<T> instance) => instance;

		public static ICondition<T> Condition<T>(ICondition<T> instance) => instance;

		public static ISelect<TIn, TOut> Selection<TIn, TOut>(ISelect<TIn, TOut> instance) => instance;

		public static IResult<T> Result<T>(IResult<T> instance) => instance;

		public static IResult<ISelect<TIn, TOut>> SelectionResult<TIn, TOut>(IResult<ISelect<TIn, TOut>> instance)
			=> instance;

		public static IResult<IConditional<TIn, TOut>> ConditionalResult<TIn, TOut>(IResult<IConditional<TIn, TOut>> @this) => @this;

		public static IAlteration<T> Self<T>() => DragonSpark.Model.Selection.Self<T>.Default;

		public static Type Type<T>() => Reflection.Types.Type<T>.Instance;

		public static TypeInfo Metadata<T>() => Reflection.Types.Type<T>.Metadata;
	}

	public static class An
	{
		public static Array<T> Array<T>(params T[] elements) => elements;
	}
}
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

		public static ICondition<T> Always<T>() => Model.Selection.Conditions.Always<T>.Default;

		public static ICondition<object> Always() => Always<object>();

		public static ICondition<T> Never<T>() => Model.Selection.Conditions.Never<T>.Default;

		public static ICondition<object> Never() => Always<object>();

		public static ICondition<T> EqualTo<T>(T source) => new Equals<T>(source);

		public static ICondition<object> Of<T>() => IsOf<T>.Default;

		public static ICondition<T> Assigned<T>() => IsAssigned<T>.Default;

		public static ICondition<object> Assigned() => IsAssigned.Default;

		public static ICondition<Type> AssignableFrom<T>() => IsAssignableFrom<T>.Default;
	}

	public static class A
	{
		public static T Default<T>() => Model.Results.Default<T>.Instance.Get();

		public static T Of<T>() => Start.An.Instance<T>();

		public static ICommand<T> Command<T>(ICommand<T> instance) => instance;

		public static ISelect<TIn, TOut> Selection<TIn, TOut>(ISelect<TIn, TOut> instance) => instance;

		public static IResult<T> Result<T>(IResult<T> instance) => instance;

		/*public static T This<T>(T instance) => instance;*/

		public static IAlteration<T> Self<T>() => Model.Selection.Self<T>.Default;

		public static Type Type<T>() => Reflection.Types.Type<T>.Instance;

		public static TypeInfo Metadata<T>() => Reflection.Types.Type<T>.Metadata;
	}

	public static class An
	{
		public static Array<T> Array<T>(params T[] elements) => elements;
	}
}
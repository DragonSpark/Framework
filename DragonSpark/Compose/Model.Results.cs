using DragonSpark.Compose.Model;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using System;

namespace DragonSpark.Compose
{
	// ReSharper disable once MismatchedFileName
	public static partial class ExtensionMethods
	{
		// TODO:

		public static IResult<T> Unless<T>(this IResult<T> @this, IResult<T> assigned)
			=> @this.Unless(Is.Assigned<T>(), assigned);

		public static IResult<T> Unless<T>(this IResult<T> @this, ICondition<T> condition, IResult<T> then)
			=> @this.Unless(condition.ToDelegate(), then);

		public static IResult<T> Unless<T>(this IResult<T> @this, Model.Condition<T> condition, IResult<T> then)
			=> new ValidatedResult<T>(condition, then, @this);

		public static IResult<T> Unless<T>(this IResult<T> @this, ICondition condition, IResult<T> then)
			=> @this.Unless(new Condition(condition.Then()), then);

		public static IResult<T> Unless<T>(this IResult<T> @this, Condition condition, IResult<T> then)
			=> new Validated<T>(condition, then, @this);

		/**/

		// TODO: Move to Query

		public static IResult<Array<TTo>> Select<TFrom, TTo>(this IResult<Array<TFrom>> @this, Func<TFrom, TTo> select)
			=> @this.Query()
			        .Select(select)
			        .Return()
			        .Then()
			        .Bind()
			        .Get();

		/**/

		public static Func<T> ToDelegate<T>(this IResult<T> @this) => @this.Get;

		public static Func<T> ToDelegateReference<T>(this IResult<T> @this)
			=> DragonSpark.Model.Results.Delegates<T>.Default.Get(@this);
	}
}
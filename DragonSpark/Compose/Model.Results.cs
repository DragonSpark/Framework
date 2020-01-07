﻿using DragonSpark.Compose.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
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

		public static IResult<T> Unless<T>(this IResult<T> @this, Model.Condition<T> condition,
		                                   IResult<T> then)
			=> new ValidatedResult<T>(condition, then, @this);

		public static IResult<T> Unless<T>(this IResult<T> @this, ICondition condition, IResult<T> then)
			=> @this.Unless(condition.ToResult().ToDelegate(), then);

		public static IResult<T> Unless<T>(this IResult<T> @this, Condition condition, IResult<T> then)
			=> new Validated<T>(condition, then, @this);

		/**/

		// TODO: Audit.

		public static TOut Get<TIn, TOut>(this IResult<ISelect<TIn, TOut>> @this, TIn parameter)
			=> @this.Get().Get(parameter);

		/**/

		// TODO: Move to Selectors

		public static ISelect<TIn, TOut> Assume<TIn, TOut>(this IResult<Func<TIn, TOut>> @this)
			=> new Assume<TIn, TOut>(@this.Get);

		public static ISelect<TIn, TOut> Assume<TIn, TOut>(this IResult<ISelect<TIn, TOut>> @this)
			=> new Select<TIn, TOut>(@this.Get);

		public static ICommand<T> Assume<T>(this IResult<ICommand<T>> @this)
			=> new DelegatedInstanceCommand<T>(@this.Get);

		public static IResult<Array<TTo>> Select<TFrom, TTo>(this IResult<Array<TFrom>> @this, Func<TFrom, TTo> select)
			=> @this.Query()
			        .Select(select)
			        .Return()
			        .ToResult();

		/**/

		public static Func<T> ToDelegate<T>(this IResult<T> @this) => @this.Get;

		public static Func<T> ToDelegateReference<T>(this IResult<T> @this)
			=> DragonSpark.Model.Results.Delegates<T>.Default.Get(@this);

		/*public static ISelect<T> ToSelect<T>(this IResult<T> @this) => new Model.Result<T>(@this.Get);*/
	}
}
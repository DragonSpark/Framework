﻿using DragonSpark.Compose.Model;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using System;

namespace DragonSpark.Compose
{
	// ReSharper disable once MismatchedFileName
	public static partial class ExtensionMethods
	{
		public static ResultContext<T> Start<T>(this Func<T> @this)
			=> Compose.Start.A.Result.Of.Type<T>().By.Calling(@this);

		public static Func<T> ToDelegate<T>(this IResult<T> @this) => @this.Get;

		public static TOut Get<TIn, TOut>(this IResult<ISelect<TIn, TOut>> @this, TIn parameter)
			=> @this.Get().Get(parameter);

		public static IResult<Array<TTo>> Select<TFrom, TTo>(this IResult<Array<TFrom>> @this, Func<TFrom, TTo> select)
			=> @this.Query()
			        .Select(select)
			        .Get()
			        .Then()
			        .Bind()
			        .Get();
	}
}
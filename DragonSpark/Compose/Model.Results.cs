﻿using DragonSpark.Compose.Model.Commands;
using DragonSpark.Compose.Model.Results;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Sequences;
using NetFabric.Hyperlinq;
using System;

namespace DragonSpark.Compose;

// ReSharper disable once MismatchedFileName
public static partial class ExtensionMethods
{
	public static ResultComposer<T> Start<T>(this T @this) => Compose.Start.A.Result.Of.Type<T>().By.Using(@this);

	public static ResultComposer<T> Start<T>(this Func<T> @this)
		=> Compose.Start.A.Result.Of.Type<T>().By.Calling(@this);

	public static CommandComposer<T> Assume<T>(this Func<ICommand<T>> @this) => Compose.Start.A.Command(@this).Assume();

	public static Func<T> ToDelegate<T>(this IResult<T> @this) => @this.Get;

	public static IResult<Array<TTo>> Select<TFrom, TTo>(this IResult<Array<TFrom>> @this,
	                                                     Func<TFrom, TTo> select)
		=> @this.Then()
		        .Select(x => x.Open().AsValueEnumerable().Select(select).ToArray().Result())
		        .Get();

	public static Lazy<T> Defer<T>(this IResult<T> @this) => new(@this.Get);

	public static Lazy<T> Defer<T>(this ResultComposer<T> @this) => @this.Get().Defer();
}
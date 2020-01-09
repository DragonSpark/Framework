using DragonSpark.Compose.Model;
using DragonSpark.Model.Results;
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

		public static ValidatedResultContext<T> Unless<T>(this IResult<T> @this, IResult<T> other)
			=> @this.Then().Unless(other);

		// TODO: Move to Query

		public static IResult<Array<TTo>> Select<TFrom, TTo>(this IResult<Array<TFrom>> @this, Func<TFrom, TTo> select)
			=> @this.Query()
			        .Select(select)
			        .Return()
			        .Then()
			        .Bind()
			        .Get();
	}
}
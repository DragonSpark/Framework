﻿using DragonSpark.Compose.Model;
using DragonSpark.Model;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using System;

namespace DragonSpark.Compose
{
	// ReSharper disable once MismatchedFileName
	public static partial class ExtensionMethods
	{
		public static Selector<TIn, TOut> Start<TIn, TOut>(this Func<TIn, TOut> @this)
			=> Compose.Start.A.Selection<TIn>().By.Calling(@this);

		/**/

		public static ISelect<TIn, TTo> Select<TIn, TFrom, TTo>(this ISelect<TIn, TFrom> @this,
		                                                        ISelect<TFrom, TTo> select) => @this.Select(select.Get);

		public static ISelect<TIn, TTo> Select<TIn, TFrom, TTo>(this ISelect<TIn, TFrom> @this, Func<TFrom, TTo> select)
			=> new Selection<TIn, TFrom, TTo>(@this.Get, select);

		/**/

		public static TOut Get<TIn, TOut, TOther>(this ISelect<TIn, TOut> @this, TIn parameter, TOther _)
			=> @this.Get(parameter);

		public static TOut Get<TIn, TOut, TOther>(this ISelect<(TIn, TOther), TOut> @this, TIn parameter, TOther other)
			=> @this.Get((parameter, other));

		public static TOut Get<TIn, TOut, TOther>(this ISelect<TIn, TOut> @this, (TIn, TOther) parameter)
			=> @this.Get(parameter.Item1);

		public static TOut Get<TIn, TOut, TOther>(this ISelect<TOther, TOut> @this, (TIn, TOther) parameter)
			=> @this.Get(parameter.Item2);

		public static TOut Get<TIn, TOut, TOther>(this ISelect<TIn, TOut> @this, TOther _, TIn parameter)
			=> @this.Get(parameter);

		public static T Get<T>(this ISelect<uint, T> @this, int parameter) => @this.Get((uint)parameter);

		public static TOut Get<TItem, TOut>(this ISelect<Array<TItem>, TOut> @this, TItem parameter)
			=> @this.Get(DragonSpark.Model.Sequences.Query.Yield<TItem>.Default.Get(parameter));

		public static TOut Get<TItem, TOut>(this ISelect<Array<TItem>, Func<TOut>> @this, params TItem[] parameters)
			=> @this.Get(parameters)();

		public static TOut Get<TItem, TOut>(this ISelect<Array<TItem>, TOut> @this, params TItem[] parameters)
			=> @this.Get(parameters);

		public static T Get<T>(this ISelect<None, T> @this) => @this.Get(None.Default);

		/**/

		public static Selector<TIn?, TOut> AccountStructure<TIn, TOut>(this Selector<TIn, TOut> @this) 
			where TIn : struct => new(new AccountedStructure<TIn, TOut>(@this.Get()));

		public static Selector<TIn?, TOut> Accounting<TIn, TOut>(this Selector<TIn, TOut> @this) 
			 => new(new Accounted<TIn, TOut>(@this.Get()));

		public static Selector<_, T> Verified<_, T>(this Selector<_, T?> @this) where T : class
			=> new Verified<_, T>(@this.Get()).Then();
		/**/

		public static Func<TIn, TOut> ToDelegate<TIn, TOut>(this ISelect<TIn, TOut> @this) => @this.Get;

		public static Func<TIn, TOut> ToDelegateReference<TIn, TOut>(this ISelect<TIn, TOut> @this)
			=> Delegates<TIn, TOut>.Default.Get(@this);
	}
}
using DragonSpark.Compose;
using DragonSpark.Compose.Extents;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Adapters;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using DragonSpark.Runtime;
using System;

namespace DragonSpark
{
	// ReSharper disable once MismatchedFileName
	public static partial class ExtensionMethods
	{
		public static IResult<T> Start<T>(this T @this) => Compose.Start.A.Result(@this);

		public static IResult<T> Start<T>(this Func<T> @this) => @this.Target as IResult<T> ??
		                                                         new Model.Results.Result<T>(@this);

		public static IResult<T> Singleton<T>(this IResult<T> @this) => new DeferredSingleton<T>(@this.Get);

		public static IMutable<T> Variable<T>(this IResult<T> @this) => new Variable<T>(@this.Get());

		public static IResult<T> Unless<T>(this IResult<T> @this, IResult<T> assigned)
			=> @this.Unless(IsAssigned<T>.Default, assigned);

		public static IResult<T> Unless<T>(this IResult<T> @this, ICondition<T> condition, IResult<T> then)
			=> @this.Unless(condition.ToDelegate(), then);

		public static IResult<T> Unless<T>(this IResult<T> @this, Model.Selection.Adapters.Condition<T> condition,
		                                   IResult<T> then)
			=> new ValidatedResult<T>(condition, then, @this);

		public static IResult<T> Unless<T>(this IResult<T> @this, ICondition condition, IResult<T> then)
			=> @this.Unless(condition.ToResult().ToDelegate(), then);

		public static IResult<T> Unless<T>(this IResult<T> @this, Condition condition, IResult<T> then)
			=> new Validated<T>(condition, then, @this);

		public static TOut Get<TIn, TOut>(this IResult<ISelect<TIn, TOut>> @this, TIn parameter)
			=> @this.Get().Get(parameter);

		public static IResult<T> Assume<T>(this IResult<IResult<T>> @this)
			=> new Assume<T>(@this.Then().Delegate().Selector());

		public static IResult<T> Assume<T>(this IResult<Func<T>> @this) => new Assume<T>(@this.Get);

		public static ISelect<TIn, TOut> Assume<TIn, TOut>(this IResult<Func<TIn, TOut>> @this)
			=> new Assume<TIn, TOut>(@this.Get);

		public static ISelect<TIn, TOut> Assume<TIn, TOut>(this IResult<ISelect<TIn, TOut>> @this)
			=> new Select<TIn, TOut>(@this.Get);

		public static ICommand<T> Assume<T>(this IResult<ICommand<T>> @this)
			=> new DelegatedInstanceCommand<T>(@this.Get);

		public static IResult<TOut> Select<TIn, TOut>(this IResult<TIn> @this, ISelect<TIn, TOut> select)
			=> @this.Select(select.Get);

		public static IResult<TOut> Select<TIn, TOut>(this IResult<TIn> @this, Func<TIn, TOut> select)
			=> new DelegatedSelection<TIn, TOut>(select, @this.Get);

		public static IResult<Array<TTo>> Select<TFrom, TTo>(this IResult<Array<TFrom>> @this, Func<TFrom, TTo> select)
			=> @this.Query()
			        .Select(select)
			        .Get()
			        .Out();

		public static Func<T> ToDelegate<T>(this IResult<T> @this) => @this.Get;

		public static Func<T> ToDelegateReference<T>(this IResult<T> @this)
			=> Model.Results.Delegates<T>.Default.Get(@this);

		public static ISelect<TIn, TOut> ToSelect<TIn, TOut>(this IResult<TOut> @this, Extent<TIn> _)
			=> Compose.Start.A.Selection<TIn>().By.Returning(@this);

		public static ISelect<T> ToSelect<T>(this IResult<T> @this)
			=> new Model.Selection.Adapters.Result<T>(@this.Get);
	}
}
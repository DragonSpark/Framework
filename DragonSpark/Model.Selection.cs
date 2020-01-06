using DragonSpark.Compose;
using DragonSpark.Compose.Extents;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Adapters;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using DragonSpark.Reflection.Types;
using DragonSpark.Runtime;
using DragonSpark.Runtime.Objects;
using System;
using Action = DragonSpark.Model.Selection.Adapters.Action;

namespace DragonSpark
{
	// ReSharper disable once MismatchedFileName
	public static partial class ExtensionMethods
	{
		public static ISelect<TIn, TOut> Start<TIn, TOut>(this TOut @this, Extent<TIn> _)
			=> Compose.Start.A.Selection<TIn>().By.Returning(@this);

		public static ISelect<TIn, TOut> Start<TIn, TOut>(this Func<TIn, TOut> @this)
			=> @this.Target as ISelect<TIn, TOut> ?? new Select<TIn, TOut>(@this);


		public static TOut Get<TIn, TOut, TOther>(this ISelect<TIn, TOut> @this, TIn parameter, TOther _)
			=> @this.Get(parameter);

		public static T Get<T>(this ISelect<uint, T> @this, int parameter) => @this.Get((uint)parameter);

		public static TOut Get<TItem, TOut>(this ISelect<Array<TItem>, TOut> @this, TItem parameter)
			=> @this.Get(Model.Sequences.Query.Yield<TItem>.Default.Get(parameter));

		public static TOut Get<TItem, TOut>(this ISelect<Array<TItem>, Func<TOut>> @this, params TItem[] parameters)
			=> @this.Get(parameters)();

		public static TOut Get<TItem, TOut>(this ISelect<Array<TItem>, TOut> @this, params TItem[] parameters)
			=> @this.Get(parameters);

		public static T Get<T>(this ISelect<None, T> @this) => @this.Get(None.Default);

		public static IResult<T> Out<T>(this ISelect<None, T> @this) => @this.In(None.Default);

		public static ISelect<TIn, TOut> Select<TIn, TOut>(this ISelect<TIn, TOut> @this,
		                                                   ISelect<Decoration<TIn, TOut>, TOut> other)
			=> new Decorator<TIn, TOut>(other, @this);

		public static IConditional<TIn, TTo> Select<TIn, TFrom, TTo>(this ISelect<TIn, TFrom> @this,
		                                                             IConditional<TFrom, TTo> select)
			=> new Conditional<TIn, TTo>(@this.Select(select.Condition).Get, @this.Select(select.Get).Get);

		public static ISelect<TIn, TTo> Select<TIn, TFrom, TTo>(this ISelect<TIn, TFrom> @this,
		                                                        ISelect<TFrom, TTo> select) => @this.Select(select.Get);

		public static ISelect<TIn, TTo> Select<TIn, TFrom, TTo>(this ISelect<TIn, TFrom> @this, Func<TFrom, TTo> select)
			=> new Selection<TIn, TFrom, TTo>(@this.Get, select);

		public static IResult<TOut> In<TIn, TOut>(this ISelect<TIn, TOut> @this, TIn parameter)
			=> new FixedSelection<TIn, TOut>(@this, parameter);

		public static IResult<TOut> In<TIn, TOut>(this ISelect<TIn, TOut> @this, Func<TIn> parameter)
			=> new DelegatedSelection<TIn, TOut>(@this.Get, parameter);

		public static IResult<TOut> In<TIn, TOut>(this ISelect<TIn, TOut> @this, IResult<TIn> parameter)
			=> new DelegatedSelection<TIn, TOut>(@this, parameter);

		public static ISelect<TIn, TOut> Assigned<TIn, TOut>(this ISelect<TIn, TOut> @this)
			=> @this.If(Is.Assigned<TIn>());

		public static ISelect<TIn, TOut> If<TIn, TOut>(this ISelect<TIn, TOut> @this, ISelect<TIn, bool> @true)
			=> Compose.Start.A.Selection<TIn>().By.Default<TOut>().Unless(@true, @this);

		public static ISelect<TIn, TOut> Unless<TIn, TOut>(this ISelect<TIn, TOut> @this, ISelect<TIn, TOut> assigned)
			=> @this.Unless(assigned.ToDelegate());

		public static ISelect<TIn, TOut> Unless<TIn, TOut>(this ISelect<TIn, TOut> @this, Selection<TIn, TOut> assigned)
			=> new ValidatedResult<TIn, TOut>(Is.Assigned<TOut>(), assigned, @this);

		public static IConditional<TIn, TOut> Unless<TIn, TOut>(this ISelect<TIn, TOut> @this,
		                                                        IConditional<TIn, TOut> then)
			=> @this.Unless(then.Condition, then);

		public static ISelect<TIn, TOut> UnlessIsOf<TIn, TOut, T>(this ISelect<TIn, TOut> @this, ISelect<T, TOut> then)
			=> @this.Unless(IsOf<TIn, T>.Default, CastOrThrow<TIn, T>.Default.Select(then));

		public static ISelect<TIn, TOut> Unless<TIn, TOut>(this ISelect<TIn, TOut> @this, ICondition<TIn> condition,
		                                                   IResult<TOut> then)
			=> @this.Unless(condition, then.Then().Accept<TIn>().Return());

		public static IConditional<TIn, TOut> Unless<TIn, TOut>(this ISelect<TIn, TOut> @this,
		                                                        ISelect<TIn, bool> unless,
		                                                        ISelect<TIn, TOut> then)
			=> new Conditional<TIn, TOut>(unless.Get, then.Get, @this.Get);

		public static Func<TIn, TOut> ToDelegate<TIn, TOut>(this ISelect<TIn, TOut> @this) => @this.Get;

		public static Func<TIn, TOut> ToDelegateReference<TIn, TOut>(this ISelect<TIn, TOut> @this)
			=> Delegates<TIn, TOut>.Default.Get(@this);

		public static ReferenceContext<TIn, TOut> Stores<TIn, TOut>(this ISelect<TIn, TOut> @this) where TIn : class
			=> new ReferenceContext<TIn, TOut>(@this);

		public static ICondition<T> ToCondition<T>(this ISelect<T, bool> @this)
			=> @this as ICondition<T> ?? new Model.Selection.Conditions.Condition<T>(@this.Get);

		public static IConditional<TIn, TOut> ToConditional<TIn, TOut>(this ISelect<TIn, TOut> @this,
		                                                               ICondition<TIn> condition)
			=> new Conditional<TIn, TOut>(condition, @this.Get);

		public static ICommand<T> ToCommand<T>(this ISelect<T, None> @this) => new InvokeParameterCommand<T>(@this.Get);

		public static ICommand ToAction(this ISelect<None, None> @this)
			=> new Action(@this.ToCommand().Execute);

		public static IResult<T> ToResult<T>(this ISelect<None, T> @this) => @this as IResult<T> ??
		                                                                     new Model.Results.Result<T>(@this.Get);
	}
}
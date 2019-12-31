using DragonSpark.Compose.Results;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Runtime;
using DragonSpark.Runtime.Activation;
using System;
using Action = DragonSpark.Model.Selection.Adapters.Action;

namespace DragonSpark.Compose
{
	public static class Extensions
	{
		public static Extents.Extent<T> Extent<T>(this VowelContext _) => Extents.Extent<T>.Default;

		public static Extents.Instance<T> Activation<T>(this VowelContext _) => Extents.Instance<T>.Implementation;

		public static T Instance<T>(this VowelContext _) => Extents.Instance<T>.Implementation.Activate();

		public static T Instance<T>(this VowelContext _, T instance) => instance;

		public static Extent<T> Of<T>(this Context @this) => @this.Of.Type<T>();

		public static Extent<T> Result<T>(this ModelContext @this) => @this.Result.Of.Type<T>();

		public static IResult<T> Result<T>(this ModelContext @this, T instance) => @this.Result<T>().By.Using(instance);

		public static IResult<T> Result<T>(this ModelContext @this, Func<T> result)
			=> @this.Result<T>().By.Calling(result);

		public static Conditions.Extent<T> Of<T>(this Conditions.Context @this) => @this.Of.Type<T>();

		public static Conditions.Extent<T> Condition<T>(this ModelContext @this) => @this.Condition.Of.Type<T>();

		public static ICondition<T> Condition<T>(this ModelContext _, ICondition<T> result) => result;

		public static Commands.Extent<T> Of<T>(this Commands.Context @this) => @this.Of.Type<T>();

		public static Commands.Extent<T> Command<T>(this ModelContext @this) => @this.Command.Of.Type<T>();

		public static Action Calling(this Commands.Extent<None> _, System.Action body) => new Action(body);

		public static Selections.Extent<T> Of<T>(this Selections.Context @this) => @this.Of.Type<T>();

		public static Selections.Extent<T> Selection<T>(this ModelContext @this) => @this.Selection.Of.Type<T>();

		public static New<T> New<T>(this ModelContext _) => Runtime.Activation.New<T>.Default;

		public static ISelect<TIn, TOut> Selection<TIn, TOut>(this ModelContext _, Func<TIn, TOut> select)
			=> new Select<TIn, TOut>(select);

		public static ISelect<TIn, TOut> Selection<TIn, TOut>(this ModelContext _, ISelect<TIn, TOut> select) => select;
	}
}
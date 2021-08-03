using DragonSpark.Compose.Extents;
using DragonSpark.Compose.Extents.Commands;
using DragonSpark.Compose.Extents.Conditions;
using DragonSpark.Compose.Extents.Results;
using DragonSpark.Compose.Extents.Selections;
using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Runtime.Activation;
using System;
using CommandContext = DragonSpark.Compose.Extents.Commands.CommandContext;

namespace DragonSpark.Compose
{
	// ReSharper disable once MismatchedFileName
	public static partial class ExtensionMethods
	{
		public static Extent<T> Extent<T>(this VowelContext _) => Extents.Extent<T>.Default;

		public static Instance<T> Activation<T>(this VowelContext _) => Extents.Instance<T>.Implementation;

		public static T Instance<T>(this VowelContext _) => Extents.Instance<T>.Implementation.Activate();

		public static T Instance<T>(this VowelContext _, T instance) => instance;

		public static ResultExtent<T> Of<T>(this ResultContext @this) => @this.Of.Type<T>();

		public static ResultExtent<T> Result<T>(this ModelContext @this) => @this.Result.Of.Type<T>();

		public static Model.Results.ResultContext<T> Result<T>(this ModelContext @this, T instance)
			=> @this.Result<T>().By.Using(instance);

		public static Model.Results.ResultContext<T> Result<T>(this ModelContext @this, Func<T> result)
			=> @this.Result<T>().By.Calling(result);

		public static ConditionExtent<T> Of<T>(this ConditionContext @this) => @this.Of.Type<T>();

		public static ConditionExtent<T> Condition<T>(this ModelContext @this) => @this.Condition.Of.Type<T>();

		public static ConditionSelector<T> Condition<T>(this ModelContext _, Func<T, bool> condition)
			=> Compose.Start.A.Condition<T>().By.Calling(condition);

		public static ICondition<T> Condition<T>(this ModelContext _, ICondition<T> result) => result;

		public static CommandExtent<T> Of<T>(this CommandContext @this) => @this.Of.Type<T>();

		public static CommandExtent<T> Command<T>(this ModelContext @this) => @this.Command.Of.Type<T>();

		public static Model.Commands.CommandContext<T> Command<T>(this ModelContext @this, Action<T> action)
			=> @this.Command.Of.Type<T>().By.Calling(action);

		public static Model.Commands.CommandContext<(T1, T2)> Command<T1, T2>(this ModelContext @this, Action<T1, T2> action)
			=> @this.Command.Of.Type<(T1, T2)>().By.Calling(action.Invoke);

		public static Model.Commands.CommandContext Command(this ModelContext _, Action action)
			=> new(new Command(action));

		public static SelectionExtent<T> Of<T>(this SelectionContext @this) => @this.Of.Type<T>();

		public static SelectionExtent<T> Selection<T>(this ModelContext @this)
			=> @this.Selection.Of.Type<T>();

		public static New<T> New<T>(this ModelContext _) => Runtime.Activation.New<T>.Default;

		public static ISelect<TIn, TOut> Selection<TIn, TOut>(this ModelContext _, Func<TIn, TOut> select)
			=> new Select<TIn, TOut>(select);

		public static ISelect<TIn, TOut> Selection<TIn, TOut>(this ModelContext _, ISelect<TIn, TOut> select) => select;

		public static GuardModelContext<T> Guard<T>(this ModelContext _) where T : Exception
			=> GuardModelContext<T>.Default;
	}
}
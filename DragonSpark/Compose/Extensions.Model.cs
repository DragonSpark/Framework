using DragonSpark.Compose.Extents.Results;
using DragonSpark.Compose.Model;
using DragonSpark.Model;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Runtime.Activation;
using System;
using Action = DragonSpark.Compose.Model.Action;

namespace DragonSpark.Compose
{
	// ReSharper disable once MismatchedFileName
	public static partial class Extensions
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

		public static Extents.Conditions.ConditionExtent<T> Of<T>(this Extents.Conditions.ConditionContext @this) => @this.Of.Type<T>();

		public static Extents.Conditions.ConditionExtent<T> Condition<T>(this ModelContext @this) => @this.Condition.Of.Type<T>();

		public static ConditionSelector<T> Condition<T>(this ModelContext _, Func<T, bool> condition)
			=> Start.A.Condition<T>().By.Calling(condition);

		public static ICondition<T> Condition<T>(this ModelContext _, ICondition<T> result) => result;

		public static Extents.Commands.CommandExtent<T> Of<T>(this Extents.Commands.CommandContext @this) => @this.Of.Type<T>();

		public static Extents.Commands.CommandExtent<T> Command<T>(this ModelContext @this) => @this.Command.Of.Type<T>();

		public static Action Calling(this Extents.Commands.CommandExtent<None> _, System.Action body) => new Action(body);

		public static Extents.Selections.Extent<T> Of<T>(this Extents.Selections.Context @this) => @this.Of.Type<T>();

		public static Extents.Selections.Extent<T> Selection<T>(this ModelContext @this) => @this.Selection.Of.Type<T>();

		public static New<T> New<T>(this ModelContext _) => Runtime.Activation.New<T>.Default;

		public static ISelect<TIn, TOut> Selection<TIn, TOut>(this ModelContext _, Func<TIn, TOut> select)
			=> new Select<TIn, TOut>(select);

		public static ISelect<TIn, TOut> Selection<TIn, TOut>(this ModelContext _, ISelect<TIn, TOut> select) => select;

		public static GuardModelContext<T> Guard<T>(this ModelContext _) where T : Exception
			=> GuardModelContext<T>.Default;
	}

	public sealed class GuardModelContext<TException> where TException : Exception
	{
		public static GuardModelContext<TException> Default { get; } = new GuardModelContext<TException>();

		GuardModelContext() {}

		public GuardThrowContext<T, TException> Displaying<T>(ISelect<T, string> message)
			=> new GuardThrowContext<T, TException>(message);
	}

	public sealed class GuardThrowContext<T, TException> where TException : Exception
	{
		readonly ISelect<T, string> _message;

		public GuardThrowContext(ISelect<T, string> message) => _message = message;

		public CommandContext<T> WhenUnassigned() => When(Is.Assigned<T>().Inverse().Out());

		public CommandContext<T> When(ICondition<T> condition) => new Guard<T, TException>(condition, _message).Then();
	}
}
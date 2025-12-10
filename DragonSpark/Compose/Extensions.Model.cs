using DragonSpark.Compose.Extents;
using DragonSpark.Compose.Extents.Commands;
using DragonSpark.Compose.Extents.Conditions;
using DragonSpark.Compose.Extents.Results;
using DragonSpark.Compose.Extents.Selections;
using DragonSpark.Compose.Model.Commands;
using DragonSpark.Compose.Model.Operations;
using DragonSpark.Compose.Model.Operations.Allocated;
using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Operations.Results;
using DragonSpark.Model.Operations.Results.Stop;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Runtime.Activation;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Action = System.Action;
using CommandComposer = DragonSpark.Compose.Extents.Commands.CommandComposer;
using ValueTask = System.Threading.Tasks.ValueTask;

namespace DragonSpark.Compose;

// ReSharper disable once MismatchedFileName
public static partial class ExtensionMethods
{
	public static Extent<T> Extent<T>(this VowelContext _) => Extents.Extent<T>.Default;

	public static Extents.Instance<T> Activation<T>(this VowelContext _) => Extents.Instance<T>.Implementation;

	public static T Instance<T>(this VowelContext _) => Extents.Instance<T>.Implementation.Activate();

	public static OperationComposer<T> Operation<T>(this VowelContext _, DragonSpark.Model.Operations.Await<T> start)
		=> new(new Awaiting<T>(start));

	public static OperationComposer<T> Operation<T>(this VowelContext _, Func<T, ValueTask> start)
		=> new(new Operation<T>(start));

	public static TaskComposer<T> Allocated<T>(this VowelContext _, Func<T, Task> start)
		=> (start.Target as IAllocated<T> ?? new Allocated<T>(start)).Then();

	public static T Instance<T>(this VowelContext _, T instance) => instance;

	public static ResultExtent<T> Of<T>(this ResultComposer @this) => @this.Of.Type<T>();

	public static ResultExtent<T> Result<T>(this ModelContext @this) => @this.Result.Of.Type<T>();

	public static Model.Results.ResultComposer<T> Result<T>(this ModelContext @this, T instance)
		=> @this.Result<T>().By.Using(instance);

	public static Model.Results.ResultComposer<T> Result<T>(this ModelContext @this, Func<T> result)
		=> @this.Result<T>().By.Calling(result);

	public static ConditionExtent<T> Of<T>(this ConditionContext @this) => @this.Of.Type<T>();

	public static ConditionExtent<T> Condition<T>(this ModelContext @this) => @this.Condition.Of.Type<T>();

	public static ConditionComposer<T> Condition<T>(this ModelContext _, Func<T, bool> condition)
		=> Compose.Start.A.Condition<T>().By.Calling(condition);

	public static ICondition<T> Condition<T>(this ModelContext _, ICondition<T> result) => result;

	public static CommandExtent<T> Of<T>(this CommandComposer @this) => @this.Of.Type<T>();

	public static CommandExtent<T> Command<T>(this ModelContext @this) => @this.Command.Of.Type<T>();

	public static Model.Commands.CommandComposer<T> Command<T>(this ModelContext @this, System.Action<T> action)
		=> @this.Command.Of.Type<T>().By.Calling(action);

	public static Model.Commands.CommandComposer<(T1, T2)> Command<T1, T2>(this ModelContext @this,
	                                                                       Action<T1, T2> action)
		=> @this.Command.Of.Type<(T1, T2)>().By.Calling(action.Invoke);

	public static Model.Commands.CommandComposer Command(this ModelContext _, Action action)
		=> new(new Command(action));

	public static CommandResultComposer<T> Command<T>(this ModelContext _, Func<ICommand<T>> action)
		=> new(action.Start().Get());

	public static CommandResultComposer Command(this ModelContext _, Func<ICommand> action)
		=> new(action.Start().Get());

	public static SelectionExtent<T> Of<T>(this SelectionContext @this) => @this.Of.Type<T>();

	public static SelectionExtent<T> Selection<T>(this ModelContext @this)
		=> @this.Selection.Of.Type<T>();

	public static New<T> New<T>(this ModelContext _) => Runtime.Activation.New<T>.Default;

	public static ISelect<TIn, TOut> Selection<TIn, TOut>(this ModelContext _, Func<TIn, TOut> select)
		=> new Select<TIn, TOut>(select);

	public static ISelect<TIn, TOut> Selection<TIn, TOut>(this ModelContext _, ISelect<TIn, TOut> select) => select;

	public static GuardModelContext<T> Guard<T>(this ModelContext _) where T : Exception
		=> GuardModelContext<T>.Default;

	[MustDisposeResource]
	public static Switching Scoped(this ISwitch @this)
	{
		@this.Up();
		return new(@this);
	}

	public static bool Down(this IMutable<bool> @this)
	{
		var result = @this.Get();
		if (result)
		{
			@this.Execute(false);
		}

		return result;
	}

	public static Switch Switched(this Switch @this)
	{
		@this.Execute(!@this);
		return @this;
	}

	public static bool Up(this IMutable<bool> @this)
	{
		var result = !@this.Get();
		if (result)
		{
			@this.Execute(true);
		}

		return result;
	}

	public static bool Assign<T>(this IMutable<T> @this, T parameter)
		=> @this.Assign(parameter, EqualityComparer<T>.Default);

	public static bool Assign<T>(this IMutable<T> @this, T parameter, IEqualityComparer<T> comparer)
	{
		var result = !comparer.Equals(@this.Get(), parameter);
		if (result)
		{
			@this.Execute(parameter);
		}

		return result;
	}

	public static IStopAware<T> AsToken<T>(this IResulting<T> @this) => new StopAwareAdapter<T>(@this);
}
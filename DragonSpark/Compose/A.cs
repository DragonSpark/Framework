using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Selection.Conditions;
using System;
using System.Reflection;

namespace DragonSpark.Compose;

public static class A
{
	public static T Default<T>() => DragonSpark.Model.Results.Default<T>.Instance.Get();

	public static T Of<T>() => Start.An.Instance<T>();

	public static ICommand<T> Command<T>(ICommand<T> instance) => instance;

	public static ICondition<T> Condition<T>(ICondition<T> instance) => instance;

	public static ISelect<TIn, TOut> Selection<TIn, TOut>(ISelect<TIn, TOut> instance) => instance;

	public static IResult<T> Result<T>(IResult<T> instance) => instance;

	public static IResult<ISelect<TIn, TOut>> SelectionResult<TIn, TOut>(IResult<ISelect<TIn, TOut>> instance)
		=> instance;

	public static IResult<ICommand> CommandResult(IResult<ICommand> @this) => @this;
	public static IResult<ICommand<T>> CommandResult<T>(IResult<ICommand<T>> @this) => @this;

	public static IResult<IConditional<TIn, TOut>> ConditionalResult<TIn, TOut>(
		IResult<IConditional<TIn, TOut>> @this) => @this;

	public static IAlteration<T> Self<T>() => DragonSpark.Model.Selection.Self<T>.Default;

	public static Type Type<T>() => Reflection.Types.Type<T>.Instance;

	public static TypeInfo Metadata<T>() => Reflection.Types.Type<T>.Metadata;
}
using DragonSpark.Compose.Extents.Selections;
using DragonSpark.Compose.Model;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using ConditionSelector = DragonSpark.Compose.Model.ConditionSelector;

namespace DragonSpark.Compose
{
	// ReSharper disable once MismatchedFileName
	public static partial class ExtensionMethods
	{
		/*
		 * https://youtu.be/oqwzuiSy9y0
		 */

		public static ResultContext<T> Then<T>(this IResult<T> @this) => new ResultContext<T>(@this);

		public static NestedResultContext<T> Then<T>(this IResult<IResult<T>> @this)
			=> new NestedResultContext<T>(@this);

		public static ResultDelegateContext<T> Then<T>(this IResult<Func<T>> @this)
			=> new ResultDelegateContext<T>(@this);

		public static SelectionResultContext<T, bool> Then<T>(this IResult<ICondition<T>> @this)
			=> new SelectionResultContext<T, bool>(@this);

		public static SelectionResultContext<TIn, TOut> Then<TIn, TOut>(this IResult<ISelect<TIn, TOut>> @this)
			=> new SelectionResultContext<TIn, TOut>(@this);

		public static SelectionDelegateResultContext<TIn, TOut> Then<TIn, TOut>(this IResult<Func<TIn, TOut>> @this)
			=> new SelectionDelegateResultContext<TIn, TOut>(@this);

		public static CommandResultContext<T> Then<T>(this IResult<ICommand<T>> @this)
			=> new CommandResultContext<T>(@this);

		/**/

		public static Selector<T> Then<T>(this ISelect<None, T> @this) => new Selector<T>(@this);

		public static ConditionSelector Then(this ISelect<None, bool> @this) => new ConditionSelector(@this);

		public static Selector<TIn, TOut> Then<TIn, TOut>(this ISelect<TIn, TOut> @this)
			=> new Selector<TIn, TOut>(@this);

		public static ConditionSelector<T> Then<T>(this ISelect<T, bool> @this) => new ConditionSelector<T>(@this);

		public static TypeSelector<T> Then<T>(this ISelect<T, Type> @this) => new TypeSelector<T>(@this);

		public static MetadataSelector<T> Then<T>(this ISelect<T, TypeInfo> @this) => new MetadataSelector<T>(@this);

		public static OperationContext<T> Then<T>(this ISelect<T, ValueTask> @this) => new OperationContext<T>(@this);

		public static ConditionSelector<_, T> Then<_, T>(this ISelect<_, ICondition<T>> @this)
			=> new ConditionSelector<_, T>(@this);

		public static OperationSelector<_, T> Then<_, T>(this ISelect<_, ValueTask<T>> @this)
			=> new OperationSelector<_, T>(@this);

		public static TaskSelector<_, T> Then<_, T>(this ISelect<_, Task<T>> @this) => new TaskSelector<_, T>(@this);


/**/

		public static CommandSelector Then(this ICommand @this) => new CommandSelector(@this);

		public static CommandSelector<T> Then<T>(this ICommand<T> @this) => new CommandSelector<T>(@this);

		public static CommandInstanceSelector<_, T> Then<_, T>(this ISelect<_, ICommand<T>> @this)
			=> new CommandInstanceSelector<_, T>(@this);

		public static ResultDelegateSelector<_, T> Then<_, T>(this ISelect<_, Func<T>> @this)
			=> new ResultDelegateSelector<_, T>(@this);

		public static ResultSelectionSelector<_, T> Then<_, T>(this ISelect<_, IResult<T>> @this)
			=> new ResultSelectionSelector<_, T>(@this);

		public static SelectionSelector<_, TIn, TOut> Then<_, TIn, TOut>(this ISelect<_, ISelect<TIn, TOut>> @this)
			=> new SelectionSelector<_, TIn, TOut>(@this);

		public static ExpressionSelector<T> Then<T>(this ISelect<T, Expression> @this)
			=> new ExpressionSelector<T>(@this);

		public static CollectionSelector<_, T> Then<_, T>(this ISelect<_, ICollection<T>> @this)
			=> new CollectionSelector<_, T>(@this);

		public static OpenArraySelector<_, T> Then<_, T>(this ISelect<_, T[]> @this)
			=> new OpenArraySelector<_, T>(@this);

		/**/

		public static ICommand Out<T>(this CommandSelector<T> @this, T parameter) => @this.Bind(parameter).Command;

		public static IAlteration<T> Out<T>(this CommandSelector<T> @this) => @this.ToConfiguration().Out();

		public static ICondition<T> Out<T>(this Selector<T, bool> @this)
			=> @this.Get()
			        .To(x => x as ICondition<T> ?? new DragonSpark.Model.Selection.Conditions.Condition<T>(x.Get));

		public static IConditional<TIn, TOut> Out<TIn, TOut>(this Selector<TIn, TOut> @this, ICondition<TIn> condition)
			=> new Conditional<TIn, TOut>(condition, @this.Get().Get);

		public static IAlteration<T> Out<T>(this Selector<T, T> @this) => Alterations<T>.Default.Get(@this.Get().Get);

		public static ISelect<_, Array<T>> Out<_, T>(this OpenArraySelector<_, T> @this) => @this.Get().Result();

		public static IOperation<T> Out<T>(this Selector<T, ValueTask> @this)
			=> @this.Get().To(x => x as IOperation<T> ?? new Operation<T>(x.Get));

		/**/

		public static ICommand<T> ToCommand<T>(this ISelect<T, None> @this) => new InvokeParameterCommand<T>(@this.Get);

		public static ICommand ToAction(this ISelect<None, None> @this)
			=> new Model.Action(@this.ToCommand().Execute);

		public static ResultContext<T> Bind<T>(this Selector<None, T> @this) => @this.Bind(None.Default);

		/*public static IResult<T> ToResult<T>(this ISelect<None, T> @this)
			=> @this as IResult<T> ??
			   new DragonSpark.Model.Results.Result<T>(@this.Get);*/

		public static T Return<T>(this ResultContext<T> @this) => @this.Get().Get();

		public static ISelect<_, T> Return<_, T>(this Selector<_, T> @this) => @this.Get();

		public static ISelect<_, Array<T>> Return<_, T>(this Query<_, T> @this) => @this.Get();
	}
}
using DragonSpark.Compose.Model;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Model.Sequences;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

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

		public static NestedResultContext<T> Then<T>(this ResultContext<IResult<T>> @this)
			=> @this.Get().Then();

		public static ResultDelegateContext<T> Then<T>(this IResult<Func<T>> @this)
			=> new ResultDelegateContext<T>(@this);

		public static ResultDelegateContext<T> Then<T>(this ResultContext<Func<T>> @this)
			=> @this.Get().Then();

		public static SelectionResultContext<T, bool> Then<T>(this IResult<ICondition<T>> @this)
			=> new SelectionResultContext<T, bool>(@this);

		public static SelectionResultContext<T, bool> Then<T>(this ResultContext<ICondition<T>> @this)
			=> @this.Get().Then();

		public static SelectionResultContext<TIn, TOut> Then<TIn, TOut>(this IResult<ISelect<TIn, TOut>> @this)
			=> new SelectionResultContext<TIn, TOut>(@this);

		public static SelectionResultContext<TIn, TOut> Then<TIn, TOut>(this ResultContext<ISelect<TIn, TOut>> @this)
			=> @this.Get().Then();

		public static SelectionDelegateResultContext<TIn, TOut> Then<TIn, TOut>(this IResult<Func<TIn, TOut>> @this)
			=> new SelectionDelegateResultContext<TIn, TOut>(@this);

		public static SelectionDelegateResultContext<TIn, TOut> Then<TIn, TOut>(
			this ResultContext<Func<TIn, TOut>> @this)
			=> @this.Get().Then();

		public static CommandResultContext<T> Then<T>(this IResult<ICommand<T>> @this)
			=> new CommandResultContext<T>(@this);

		public static CommandResultContext<T> Then<T>(this ResultContext<ICommand<T>> @this)
			=> @this.Get().Then();

		/**/

		public static Selector<T> Then<T>(this ISelect<None, T> @this) => new Selector<T>(@this);

		public static NestedConditionSelector Then(this ISelect<None, bool> @this)
			=> new NestedConditionSelector(@this);

		public static NestedConditionSelector Then(this Selector<None, bool> @this)
			=> @this.Get().Then();

		public static Selector<TIn, TOut> Then<TIn, TOut>(this ISelect<TIn, TOut> @this)
			=> new Selector<TIn, TOut>(@this);

		public static ConditionalSelector<TIn, TOut> Then<TIn, TOut>(this IConditional<TIn, TOut> @this)
			=> new ConditionalSelector<TIn, TOut>(@this);

		public static TableSelector<TIn, TOut> Then<TIn, TOut>(this ITable<TIn, TOut> @this)
			=> new TableSelector<TIn, TOut>(@this);

		public static ConditionSelector<T> Then<T>(this ISelect<T, bool> @this) => new ConditionSelector<T>(@this);

		public static ConditionSelector<T> Then<T>(this Selector<T, bool> @this) => @this.Get().Then();

		public static TypeSelector<T> Then<T>(this ISelect<T, Type> @this) => new TypeSelector<T>(@this);

		public static TypeSelector<T> Then<T>(this Selector<T, Type> @this) => @this.Get().Then();

		public static MetadataSelector<T> Then<T>(this ISelect<T, TypeInfo> @this) => new MetadataSelector<T>(@this);

		public static MetadataSelector<T> Then<T>(this Selector<T, TypeInfo> @this) => @this.Get().Then();

		public static NestedConditionSelector<_, T> Then<_, T>(this ISelect<_, ICondition<T>> @this)
			=> new NestedConditionSelector<_, T>(@this);

		public static NestedConditionSelector<_, T> Then<_, T>(this Selector<_, ICondition<T>> @this)
			=> @this.Get().Then();

		public static OperationContext<T> Then<T>(this ISelect<T, ValueTask> @this) => new OperationContext<T>(@this);

		public static OperationContext<T> Then<T>(this Selector<T, ValueTask> @this) => @this.Get().Then();

		public static OperationResultSelector<T> Then<T>(this IResult<ValueTask<T>> @this)
			=> new OperationResultSelector<T>(@this);

		public static OperationResultSelector<T> Then<T>(this ResultContext<ValueTask<T>> @this) => @this.Get().Then();

		public static OperationResultSelector<_, T> Then<_, T>(this ISelect<_, ValueTask<T>> @this)
			=> new OperationResultSelector<_, T>(@this);

		public static OperationResultSelector<_, T> Then<_, T>(this Selector<_, ValueTask<T>> @this)
			=> @this.Get().Then();

		public static TaskSelector<_, T> Then<_, T>(this ISelect<_, Task<T>> @this) => new TaskSelector<_, T>(@this);

		public static TaskSelector<_, T> Then<_, T>(this Selector<_, Task<T>> @this) => @this.Get().Then();

/**/

		public static CommandContext Then(this ICommand @this) => new CommandContext(@this);

		public static CommandContext<T> Then<T>(this ICommand<T> @this) => new CommandContext<T>(@this);

		public static CommandInstanceSelector<_, T> Then<_, T>(this ISelect<_, ICommand<T>> @this)
			=> new CommandInstanceSelector<_, T>(@this);

		public static CommandInstanceSelector<_, T> Then<_, T>(this Selector<_, ICommand<T>> @this)
			=> @this.Get().Then();

		public static ResultDelegateSelector<_, T> Then<_, T>(this ISelect<_, Func<T>> @this)
			=> new ResultDelegateSelector<_, T>(@this);

		public static ResultDelegateSelector<_, T> Then<_, T>(this Selector<_, Func<T>> @this)
			=> @this.Get().Then();

		public static ResultSelectionSelector<_, T> Then<_, T>(this ISelect<_, IResult<T>> @this)
			=> new ResultSelectionSelector<_, T>(@this);

		public static ResultSelectionSelector<_, T> Then<_, T>(this Selector<_, IResult<T>> @this)
			=> @this.Get().Then();

		public static SelectionSelector<_, TIn, TOut> Then<_, TIn, TOut>(this ISelect<_, ISelect<TIn, TOut>> @this)
			=> new SelectionSelector<_, TIn, TOut>(@this);

		public static SelectionSelector<_, TIn, TOut> Then<_, TIn, TOut>(this Selector<_, ISelect<TIn, TOut>> @this)
			=> @this.Get().Then();

		public static ExpressionSelector<T> Then<T>(this ISelect<T, Expression> @this)
			=> new ExpressionSelector<T>(@this);

		public static ExpressionSelector<T> Then<T>(this Selector<T, Expression> @this)
			=> @this.Get().Then();

		public static SequenceSelector<_, T> Then<_, T>(this ISelect<_, IEnumerable<T>> @this)
			=> new SequenceSelector<_, T>(@this);

		public static SequenceSelector<_, T> Then<_, T>(this Selector<_, IEnumerable<T>> @this)
			=> @this.Get().Then();

		public static CollectionSelector<_, T> Then<_, T>(this ISelect<_, ICollection<T>> @this)
			=> new CollectionSelector<_, T>(@this);

		public static CollectionSelector<_, T> Then<_, T>(this Selector<_, ICollection<T>> @this)
			=> @this.Get().Then();

		public static OpenArraySelector<_, T> Then<_, T>(this ISelect<_, T[]> @this)
			=> new OpenArraySelector<_, T>(@this);

		public static OpenArraySelector<_, T> Then<_, T>(this Selector<_, T[]> @this)
			=> @this.Get().Then();

		public static ArraySelector<_, T> Then<_, T>(this ISelect<_, Array<T>> @this)
			=> new ArraySelector<_, T>(@this);

		public static ArraySelector<_, T> Then<_, T>(this Selector<_, Array<T>> @this)
			=> @this.Get().Then();

		/**/

		public static ICommand Out<T>(this CommandContext<T> @this, T parameter) => @this.Bind(parameter).Command;

		public static IAlteration<T> Out<T>(this CommandContext<T> @this) => @this.ToConfiguration().Out();

		public static ICondition<T> Out<T>(this Selector<T, bool> @this)
			=> @this.Get()
			        .To(x => x as ICondition<T> ?? new DragonSpark.Model.Selection.Conditions.Condition<T>(x.Get));

		public static IConditional<TIn, TOut> Out<TIn, TOut>(this Selector<TIn, TOut> @this, ICondition<TIn> condition)
			=> new Conditional<TIn, TOut>(condition, @this.Get().Get);

		public static IAlteration<T> Out<T>(this Selector<T, T> @this) => Alterations<T>.Default.Get(@this);

		public static IOperation<T> Out<T>(this Selector<T, ValueTask> @this)
			=> @this.Get().To(x => x as IOperation<T> ?? new Operation<T>(x.Get));

		/**/

		public static ICommand<T> ToCommand<T>(this Selector<T, None> @this)
			=> new InvokeParameterCommand<T>(@this);

		public static ResultContext<T> Bind<T>(this Selector<None, T> @this) => @this.Bind(None.Default);

		public static T Return<T>(this ResultContext<T> @this) => @this.Get().Get();

		public static ISelect<_, T> Return<_, T>(this Selector<_, T> @this) => @this.Get();
	}
}
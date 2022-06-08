using DragonSpark.Compose.Model.Commands;
using DragonSpark.Compose.Model.Operations;
using DragonSpark.Compose.Model.Operations.Allocated;
using DragonSpark.Compose.Model.Results;
using DragonSpark.Compose.Model.Selection;
using DragonSpark.Compose.Model.Sequences;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Allocated;
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

namespace DragonSpark.Compose;

// ReSharper disable once MismatchedFileName
public static partial class ExtensionMethods
{
	public static ResultContext<T> Then<T>(this IResult<T> @this) => new(@this);

	public static NestedResultContext<T> Then<T>(this IResult<IResult<T>> @this)
		=> new(@this);

	public static NestedResultContext<T> Then<T>(this ResultContext<IResult<T>> @this)
		=> @this.Get().Then();

	public static ResultDelegateContext<T> Then<T>(this IResult<Func<T>> @this)
		=> new(@this);

	public static ResultDelegateContext<T> Then<T>(this ResultContext<Func<T>> @this)
		=> @this.Get().Then();

	public static SelectionResultContext<T, bool> Then<T>(this IResult<ICondition<T>> @this)
		=> new(@this);

	public static SelectionResultContext<T, bool> Then<T>(this ResultContext<ICondition<T>> @this)
		=> @this.Get().Then();

	public static SelectionResultContext<TIn, TOut> Then<TIn, TOut>(this IResult<ISelect<TIn, TOut>> @this)
		=> new(@this);

	public static SelectionResultContext<TIn, TOut> Then<TIn, TOut>(this ResultContext<ISelect<TIn, TOut>> @this)
		=> @this.Get().Then();

	public static SelectionDelegateResultContext<TIn, TOut> Then<TIn, TOut>(this IResult<Func<TIn, TOut>> @this)
		=> new(@this);

	public static SelectionDelegateResultContext<TIn, TOut> Then<TIn, TOut>(
		this ResultContext<Func<TIn, TOut>> @this)
		=> @this.Get().Then();

	public static CommandResultContext Then(this IResult<ICommand> @this) => new(@this);

	public static CommandResultContext<T> Then<T>(this IResult<ICommand<T>> @this) => new(@this);

	public static CommandResultContext<T> Then<T>(this ResultContext<ICommand<T>> @this) => new(@this.Get());



	/**/

	public static Selector<T> Then<T>(this ISelect<None, T> @this) => new(@this);

	public static NestedConditionSelector Then(this ISelect<None, bool> @this)
		=> new(@this);

	public static NestedConditionSelector Then(this Selector<None, bool> @this)
		=> @this.Get().Then();

	public static Selector<TIn, TOut> Then<TIn, TOut>(this ISelect<TIn, TOut> @this)
		=> new(@this);

	public static ConditionalSelector<TIn, TOut> Then<TIn, TOut>(this IConditional<TIn, TOut> @this)
		=> new(@this);

	public static TableSelector<TIn, TOut> Then<TIn, TOut>(this ITable<TIn, TOut> @this)
		=> new(@this);

	public static ConditionSelector<T> Then<T>(this ISelect<T, bool> @this) => new(@this);

	public static ConditionSelector<T> Then<T>(this Selector<T, bool> @this) => @this.Get().Then();

	public static TypeSelector<T> Then<T>(this ISelect<T, Type> @this) => new(@this);

	public static TypeSelector<T> Then<T>(this Selector<T, Type> @this) => @this.Get().Then();

	public static MetadataSelector<T> Then<T>(this ISelect<T, TypeInfo> @this) => new(@this);

	public static MetadataSelector<T> Then<T>(this Selector<T, TypeInfo> @this) => @this.Get().Then();

	public static NestedConditionSelector<_, T> Then<_, T>(this ISelect<_, ICondition<T>> @this)
		=> new(@this);

	public static NestedConditionSelector<_, T> Then<_, T>(this Selector<_, ICondition<T>> @this)
		=> @this.Get().Then();


	public static OperationContext<T> Then<T>(this IOperation<T> @this) => new(@this);
	public static OperationContext<T> Then<T>(this ISelect<T, ValueTask> @this) => new(@this);

	public static OperationContext<T> Then<T>(this Await<T> @this)
		=> new(@this.Target as ISelect<T, ValueTask> ?? new Awaiting<T>(@this));

	public static OperationContext<T> Then<T>(this Selector<T, ValueTask> @this) => @this.Get().Then();

	public static OperationResultSelector<T> Then<T>(this IResult<ValueTask<T>> @this)
		=> new(@this);

	public static OperationResultSelector<T> Then<T>(this ResultContext<ValueTask<T>> @this) => @this.Get().Then();

	public static OperationSelector Then(this IResult<ValueTask> @this) => new(@this);

	public static OperationSelector Then(this ResultContext<ValueTask> @this) => @this.Get().Then();

	public static AllocatedOperationSelector Then(this IResult<Task> @this) => new(@this);

	public static AllocatedOperationSelector Then(this ResultContext<Task> @this) => @this.Get().Then();

	public static OperationResultSelector<_, T> Then<_, T>(this ISelecting<_, T> @this) => new(@this);

	public static OperationResultSelector<_, T> Then<_, T>(this ISelect<_, ValueTask<T>> @this) => new(@this);

	public static OperationResultSelector<_, T> Then<_, T>(this Selector<_, ValueTask<T>> @this) => @this.Get().Then();

	public static TaskSelector<_, T> Then<_, T>(this ISelect<_, Task<T>> @this) => new(@this);

	public static TaskSelector<_, T> Then<_, T>(this Selector<_, Task<T>> @this) => @this.Get().Then();

	public static TaskSelector<_> Then<_>(this ISelect<_, Task> @this) => new(@this);

	public static TaskSelector<_> Then<_>(this Selector<_, Task> @this) => @this.Get().Then();

/**/

	public static CommandContext Then(this ICommand @this) => new(@this);

	public static CommandContext<T> Then<T>(this ICommand<T> @this) => new(@this);

	public static CommandInstanceSelector<_, T> Then<_, T>(this ISelect<_, ICommand<T>> @this) => new(@this);

	public static CommandInstanceSelector<_, T> Then<_, T>(this Selector<_, ICommand<T>> @this) => @this.Get().Then();

	public static ResultDelegateSelector<_, T> Then<_, T>(this ISelect<_, Func<T>> @this) => new(@this);

	public static ResultDelegateSelector<_, T> Then<_, T>(this Selector<_, Func<T>> @this) => @this.Get().Then();

	public static ResultSelectionSelector<_, T> Then<_, T>(this ISelect<_, IResult<T>> @this) => new(@this);

	public static ResultSelectionSelector<_, T> Then<_, T>(this Selector<_, IResult<T>> @this) => @this.Get().Then();

	public static SelectionSelector<_, TIn, TOut> Then<_, TIn, TOut>(this ISelect<_, ISelect<TIn, TOut>> @this)
		=> new(@this);

	public static SelectionSelector<_, TIn, TOut> Then<_, TIn, TOut>(this Selector<_, ISelect<TIn, TOut>> @this)
		=> @this.Get().Then();

	public static ExpressionSelector<T> Then<T>(this ISelect<T, Expression> @this) => new(@this);

	public static ExpressionSelector<T> Then<T>(this Selector<T, Expression> @this) => @this.Get().Then();

	public static SequenceSelector<_, T> Then<_, T>(this ISelect<_, IEnumerable<T>> @this) => new(@this);

	public static SequenceSelector<_, T> Then<_, T>(this Selector<_, IEnumerable<T>> @this) => @this.Get().Then();

	public static CollectionSelector<_, T> Then<_, T>(this ISelect<_, ICollection<T>> @this) => new(@this);

	public static CollectionSelector<_, T> Then<_, T>(this Selector<_, ICollection<T>> @this) => @this.Get().Then();

	public static OpenArraySelector<_, T> Then<_, T>(this ISelect<_, T[]> @this) => new(@this);

	public static OpenArraySelector<_, T> Then<_, T>(this Selector<_, T[]> @this) => @this.Get().Then();

	public static ArraySelector<_, T> Then<_, T>(this ISelect<_, Array<T>> @this) => new(@this);

	public static ArraySelector<_, T> Then<_, T>(this Selector<_, Array<T>> @this) => @this.Get().Then();

	/**/

	public static ICommand Out(this CommandContext<None> @this)
		=> @this.Get().To(x => x as ICommand ?? new Command(x.Execute));

	public static IAlteration<T> Out<T>(this CommandContext<T> @this) => @this.ToConfiguration().Out();

	public static ICondition<T> Out<T>(this Selector<T, bool> @this)
		=> @this.Get()
		        .To(x => x as ICondition<T> ?? new DragonSpark.Model.Selection.Conditions.Condition<T>(x.Get));

	public static ICondition Out(this ResultContext<bool> @this) => @this.Get().To(x => new Condition(x.Get));

	public static IConditional<TIn, TOut> Out<TIn, TOut>(this Selector<TIn, TOut> @this, ICondition<TIn> condition)
		=> new Conditional<TIn, TOut>(condition, @this.Get().Get);

	public static IAlteration<T> Out<T>(this Selector<T, T> @this) => References<T>.Default.Get(@this);

	public static IOperation Out(this OperationSelector @this)
		=> @this.Get().To(x => x as IOperation ?? new Operation(x.Get));

	public static IOperation<T> Out<T>(this Selector<T, ValueTask> @this)
		=> @this.Get().To(x => x as IOperation<T> ?? new Operation<T>(x.Get));
	public static IDepending<T> Out<T>(this Selector<T, ValueTask<bool>> @this)
		=> @this.Get().To(x => x as IDepending<T> ?? new Depending<T>(x.Get));

	public static ISelecting<TIn, TOut> Out<TIn, TOut>(this Selector<TIn, ValueTask<TOut>> @this)
		=> @this.Get().To(x => x as ISelecting<TIn, TOut> ?? new Selecting<TIn, TOut>(x.Get));


	public static IAllocating<TIn, TOut> Out<TIn, TOut>(this Selector<TIn, Task<TOut>> @this) => @this.Get().Out();
	public static IAllocating<TIn, TOut> Out<TIn, TOut>(this ISelect<TIn, Task<TOut>> @this)
		=> new Allocating<TIn, TOut>(@this);

	public static IAllocated<T> Out<T>(this Selector<T, Task> @this) => @this.Get().Out();
	public static IAllocated<T> Out<T>(this ISelect<T, Task> @this) => new Allocated<T>(@this);


	public static IAllocatedResult<T> Out<T>(this ResultContext<Task<T>> @this) => @this.Get().Out();
	public static IAllocatedResult<T> Out<T>(this IResult<Task<T>> @this) => new AllocatedResult<T>(@this);

	public static IResulting<T> Out<T>(this ResultContext<ValueTask<T>> @this) => @this.Get().Out();
	public static IResulting<T> Out<T>(this IResult<ValueTask<T>> @this) => new Resulting<T>(@this);
	
	public static IDepending Out(this ResultContext<ValueTask<bool>> @this) => @this.Get().Out();
	public static IDepending Out(this IResult<ValueTask<bool>> @this) => new Depending(@this);

	/**/

	public static ICommand<T> ToCommand<T>(this Selector<T, None> @this) => new InvokeParameterCommand<T>(@this);

	public static ResultContext<T> Bind<T>(this Selector<None, T> @this) => @this.Bind(None.Default);

	public static T Instance<T>(this ResultContext<T> @this) => @this.Get().Get();

	public static T Instance<T>(this IResult<IResult<T>> @this) => @this.Get().Get();

	public static ISelect<_, T> Return<_, T>(this Selector<_, T> @this) => @this.Get();
}
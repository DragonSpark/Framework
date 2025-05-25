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
using DragonSpark.Model.Operations.Results;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Operations.Selection.Conditions;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Operations.Stop;
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
	public static ResultComposer<T> Then<T>(this IResult<T> @this) => new(@this);

	public static NestedResultComposer<T> Then<T>(this IResult<IResult<T>> @this)
		=> new(@this);

	public static NestedResultComposer<T> Then<T>(this ResultComposer<IResult<T>> @this)
		=> @this.Get().Then();

	public static ResultDelegateComposer<T> Then<T>(this IResult<Func<T>> @this)
		=> new(@this);

	public static ResultDelegateComposer<T> Then<T>(this ResultComposer<Func<T>> @this)
		=> @this.Get().Then();

	public static SelectionResultComposer<T, bool> Then<T>(this IResult<ICondition<T>> @this)
		=> new(@this);

	public static SelectionResultComposer<T, bool> Then<T>(this ResultComposer<ICondition<T>> @this)
		=> @this.Get().Then();

	public static SelectionResultComposer<TIn, TOut> Then<TIn, TOut>(this IResult<ISelect<TIn, TOut>> @this)
		=> new(@this);

	public static SelectionResultComposer<TIn, TOut> Then<TIn, TOut>(this ResultComposer<ISelect<TIn, TOut>> @this)
		=> @this.Get().Then();

	public static SelectionDelegateResultComposer<TIn, TOut> Then<TIn, TOut>(this IResult<Func<TIn, TOut>> @this)
		=> new(@this);

	public static SelectionDelegateResultComposer<TIn, TOut> Then<TIn, TOut>(
		this ResultComposer<Func<TIn, TOut>> @this)
		=> @this.Get().Then();

	public static CommandResultComposer Then(this IResult<ICommand> @this) => new(@this);

	public static CommandResultComposer<T> Then<T>(this IResult<ICommand<T>> @this) => new(@this);

	public static CommandResultComposer<T> Then<T>(this ResultComposer<ICommand<T>> @this) => new(@this.Get());



	/**/

	public static Composer<T> Then<T>(this ISelect<None, T> @this) => new(@this);

	public static NestedConditionComposer Then(this ISelect<None, bool> @this)
		=> new(@this);

	public static NestedConditionComposer Then(this Composer<None, bool> @this)
		=> @this.Get().Then();

	public static Composer<TIn, TOut> Then<TIn, TOut>(this ISelect<TIn, TOut> @this)
		=> new(@this);

	public static ConditionalComposer<TIn, TOut> Then<TIn, TOut>(this IConditional<TIn, TOut> @this)
		=> new(@this);

	public static TableComposer<TIn, TOut> Then<TIn, TOut>(this ITable<TIn, TOut> @this)
		=> new(@this);

	public static ConditionComposer<T> Then<T>(this ISelect<T, bool> @this) => new(@this);

	public static ConditionComposer<T> Then<T>(this Composer<T, bool> @this) => @this.Get().Then();

	public static TypeComposer<T> Then<T>(this ISelect<T, Type> @this) => new(@this);

	public static TypeComposer<T> Then<T>(this Composer<T, Type> @this) => @this.Get().Then();

	public static MetadataComposer<T> Then<T>(this ISelect<T, TypeInfo> @this) => new(@this);

	public static MetadataComposer<T> Then<T>(this Composer<T, TypeInfo> @this) => @this.Get().Then();

	public static NestedConditionComposer<_, T> Then<_, T>(this ISelect<_, ICondition<T>> @this)
		=> new(@this);

	public static NestedConditionComposer<_, T> Then<_, T>(this Composer<_, ICondition<T>> @this)
		=> @this.Get().Then();


	public static OperationComposer<T> Then<T>(this IOperation<T> @this) => new(@this);
	public static OperationComposer<T> Then<T>(this ISelect<T, ValueTask> @this) => new(@this);

	public static OperationComposer<T> Then<T>(this DragonSpark.Model.Operations.Await<T> @this)
		=> new(@this.Target as ISelect<T, ValueTask> ?? new Awaiting<T>(@this));

	public static OperationComposer<T> Then<T>(this Composer<T, ValueTask> @this) => @this.Get().Then();

	public static OperationResultComposer<T> Then<T>(this IResult<ValueTask<T>> @this)
		=> new(@this);

	public static OperationResultComposer<T> Then<T>(this ResultComposer<ValueTask<T>> @this) => @this.Get().Then();

	public static OperationComposer Then(this IResult<ValueTask> @this) => new(@this);

	public static OperationComposer Then(this ResultComposer<ValueTask> @this) => @this.Get().Then();

	public static AllocatedOperationComposer Then(this IResult<Task> @this) => new(@this);

	public static AllocatedOperationComposer Then(this ResultComposer<Task> @this) => @this.Get().Then();

	public static OperationResultComposer<_, T> Then<_, T>(this ISelecting<_, T> @this) => new(@this);

	public static OperationResultComposer<_, T> Then<_, T>(this ISelect<_, ValueTask<T>> @this) => new(@this);

	public static OperationResultComposer<_, T> Then<_, T>(this Composer<_, ValueTask<T>> @this) => @this.Get().Then();

	public static TaskComposer<_, T> Then<_, T>(this ISelect<_, Task<T>> @this) => new(@this);

	public static TaskComposer<_, T> Then<_, T>(this Composer<_, Task<T>> @this) => @this.Get().Then();

	public static TaskComposer<_> Then<_>(this ISelect<_, Task> @this) => new(@this);

	public static TaskComposer<_> Then<_>(this Composer<_, Task> @this) => @this.Get().Then();

/**/

	public static CommandComposer Then(this ICommand @this) => new(@this);

	public static CommandComposer<T> Then<T>(this ICommand<T> @this) => new(@this);

	public static CommandInstanceComposer<_, T> Then<_, T>(this ISelect<_, ICommand<T>> @this) => new(@this);

	public static CommandInstanceComposer<_, T> Then<_, T>(this Composer<_, ICommand<T>> @this) => @this.Get().Then();

	public static ResultDelegateComposer<_, T> Then<_, T>(this ISelect<_, Func<T>> @this) => new(@this);

	public static ResultDelegateComposer<_, T> Then<_, T>(this Composer<_, Func<T>> @this) => @this.Get().Then();

	public static ResultSelectionComposer<_, T> Then<_, T>(this ISelect<_, IResult<T>> @this) => new(@this);

	public static ResultSelectionComposer<_, T> Then<_, T>(this Composer<_, IResult<T>> @this) => @this.Get().Then();

	public static SelectionComposer<_, TIn, TOut> Then<_, TIn, TOut>(this ISelect<_, ISelect<TIn, TOut>> @this)
		=> new(@this);

	public static SelectionComposer<_, TIn, TOut> Then<_, TIn, TOut>(this Composer<_, ISelect<TIn, TOut>> @this)
		=> @this.Get().Then();

	public static ExpressionComposer<T> Then<T>(this ISelect<T, Expression> @this) => new(@this);

	public static ExpressionComposer<T> Then<T>(this Composer<T, Expression> @this) => @this.Get().Then();

	public static SequenceComposer<_, T> Then<_, T>(this ISelect<_, IEnumerable<T>> @this) => new(@this);

	public static SequenceComposer<_, T> Then<_, T>(this Composer<_, IEnumerable<T>> @this) => @this.Get().Then();

	public static CollectionComposer<_, T> Then<_, T>(this ISelect<_, ICollection<T>> @this) => new(@this);

	public static CollectionComposer<_, T> Then<_, T>(this Composer<_, ICollection<T>> @this) => @this.Get().Then();

	public static OpenArrayComposer<_, T> Then<_, T>(this ISelect<_, T[]> @this) => new(@this);

	public static OpenArrayComposer<_, T> Then<_, T>(this Composer<_, T[]> @this) => @this.Get().Then();

	public static ArrayComposer<_, T> Then<_, T>(this ISelect<_, Array<T>> @this) => new(@this);

	public static ArrayComposer<_, T> Then<_, T>(this Composer<_, Array<T>> @this) => @this.Get().Then();

	/**/

	public static ICommand Out(this CommandComposer<None> @this)
		=> @this.Get().To(x => x as ICommand ?? new Command(x.Execute));

	public static IAlteration<T> Out<T>(this CommandComposer<T> @this) => @this.ToConfiguration().Out();

	public static ICondition<T> Out<T>(this Composer<T, bool> @this)
		=> @this.Get().To(x => x as ICondition<T> ?? new Condition<T>(x.Get));

	public static ICondition Out(this ConditionComposer<None> @this) => @this.Get().To(x => new Condition(x.Get));
	public static ICondition Out(this ResultComposer<bool> @this) => @this.Get().To(x => new Condition(x.Get));

	public static IConditional<TIn, TOut> Out<TIn, TOut>(this Composer<TIn, TOut> @this, ICondition<TIn> condition)
		=> new Conditional<TIn, TOut>(condition, @this.Get().Get);

	public static IAlteration<T> Out<T>(this Composer<T, T> @this) => References<T>.Default.Get(@this);

	public static IOperation Out(this OperationComposer @this)
		=> @this.Get().To(x => x as IOperation ?? new Operation(x.Get));
	public static IOperation<T> Out<T>(this Composer<T, ValueTask> @this)
		=> @this.Get().To(x => x as IOperation<T> ?? new Operation<T>(x.Get));

	public static IDepending<T> Out<T>(this Composer<T, ValueTask<bool>> @this)
		=> @this.Get().To(x => x as IDepending<T> ?? new Depending<T>(x.Get));

	public static IDependingWithStop<T> Out<T>(this Composer<Stop<T>, ValueTask<bool>> @this) => @this.Get().Out();
	public static IDependingWithStop<T> Out<T>(this ISelect<Stop<T>, ValueTask<bool>> @this)
		=> @this.To(x => x as IDependingWithStop<T> ?? new DependingWithStop<T>(x.Get));

	public static ISelecting<TIn, TOut> Out<TIn, TOut>(this Composer<TIn, ValueTask<TOut>> @this)
		=> @this.Get().To(x => x as ISelecting<TIn, TOut> ?? new Selecting<TIn, TOut>(x.Get));
	public static IStopAware<TIn, TOut> Out<TIn, TOut>(this Composer<Stop<TIn>, ValueTask<TOut>> @this)
		=> @this.Get().To(x => x as IStopAware<TIn, TOut> ?? new StopAware<TIn, TOut>(x.Get));

	public static IStopAware<T> Out<T>(this Composer<Stop<T>, ValueTask> @this) => @this.Get().Out();
	public static IStopAware<T> Out<T>(this ISelect<Stop<T>, ValueTask> @this)
		=> @this.To(x => x as IStopAware<T> ?? new StopAware<T>(x.Get));

	public static IContinuing<TIn, TOut> Out<TIn, TOut>(this Composer<Stop<TIn>, ValueTask<Stop<TOut>>> @this)
		=> @this.Get().To(x => x as IContinuing<TIn, TOut> ?? new Continuing<TIn, TOut>(x.Get));


	public static IAllocating<TIn, TOut> Out<TIn, TOut>(this Composer<TIn, Task<TOut>> @this) => @this.Get().Out();
	public static IAllocating<TIn, TOut> Out<TIn, TOut>(this ISelect<TIn, Task<TOut>> @this)
		=> new Allocating<TIn, TOut>(@this);

	public static IAllocated<T> Out<T>(this Composer<T, Task> @this) => @this.Get().Out();
	public static IAllocated<T> Out<T>(this ISelect<T, Task> @this) => new Allocated<T>(@this);


	public static IAllocatedResult<T> Out<T>(this ResultComposer<Task<T>> @this) => @this.Get().Out();
	public static IAllocatedResult<T> Out<T>(this IResult<Task<T>> @this) => new AllocatedResult<T>(@this);

	public static IOperation Out(this ResultComposer<ValueTask> @this) => @this.Get().Out();
	public static IOperation Out(this IResult<ValueTask> @this) => new Operation(@this.Get);

	public static IResulting<T> Out<T>(this ResultComposer<ValueTask<T>> @this) => @this.Get().Out();
	public static IResulting<T> Out<T>(this IResult<ValueTask<T>> @this) => new Resulting<T>(@this);

	public static IDepending Out(this ResultComposer<ValueTask<bool>> @this) => @this.Get().Out();
	public static IDepending Out(this IResult<ValueTask<bool>> @this) => new Depending(@this);

	/**/

	public static ICommand<T> ToCommand<T>(this Composer<T, None> @this) => new InvokeParameterCommand<T>(@this);

	public static ResultComposer<T> Bind<T>(this Composer<None, T> @this) => @this.Bind(None.Default);

	public static T Instance<T>(this ResultComposer<T> @this) => @this.Get().Get();

	public static T Instance<T>(this IResult<IResult<T>> @this) => @this.Get().Get();

	public static ISelect<_, T> Return<_, T>(this Composer<_, T> @this) => @this.Get();
}
using DragonSpark.Compose.Selections;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Adapters;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using DragonSpark.Runtime;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Action = System.Action;

namespace DragonSpark
{
	// ReSharper disable once MismatchedFileName
	public static partial class ExtensionMethods
	{
		/*
		 * https://youtu.be/oqwzuiSy9y0
		 */

		public static OperationContext<T> Then<T>(this ISelect<T, ValueTask> @this) => new OperationContext<T>(@this);

		public static ConditionSelector<_, T> Then<_, T>(this ISelect<_, ICondition<T>> @this)
			=> new ConditionSelector<_, T>(@this);

		public static OperationSelector<_, T> Then<_, T>(this ISelect<_, ValueTask<T>> @this)
			=> new OperationSelector<_, T>(@this);

		public static TaskSelector<_, T> Then<_, T>(this ISelect<_, Task<T>> @this) => new TaskSelector<_, T>(@this);

		public static Selector<TIn, TOut> Then<TIn, TOut>(this ISelect<TIn, TOut> @this)
			=> new Selector<TIn, TOut>(@this);

		public static ConditionSelector<T> Then<T>(this ISelect<T, bool> @this) => new ConditionSelector<T>(@this);

		public static TypeSelector<T> Then<T>(this ISelect<T, Type> @this) => new TypeSelector<T>(@this);

		public static MetadataSelector<T> Then<T>(this ISelect<T, TypeInfo> @this) => new MetadataSelector<T>(@this);

		public static ResultSelector<T> Then<T>(this IResult<T> @this) => new ResultSelector<T>(@this.ToSelect());

		public static CommandSelector Then(this ICommand @this) => new CommandSelector(@this);

		public static CommandSelector<T> Then<T>(this ICommand<T> @this) => new CommandSelector<T>(@this);

		public static ResultInstanceSelector<None, T> Then<T>(this IResult<IResult<T>> @this)
			=> new ResultInstanceSelector<None, T>(@this.ToSelect());

		public static CommandInstanceSelector<_, T> Then<_, T>(this ISelect<_, ICommand<T>> @this)
			=> new CommandInstanceSelector<_, T>(@this);

		public static ResultDelegateSelector<_, T> Then<_, T>(this ISelect<_, Func<T>> @this)
			=> new ResultDelegateSelector<_, T>(@this);

		public static ResultInstanceSelector<_, T> Then<_, T>(this ISelect<_, IResult<T>> @this)
			=> new ResultInstanceSelector<_, T>(@this);

		public static SelectionSelector<None, TIn, TOut> Then<TIn, TOut>(this IResult<ISelect<TIn, TOut>> @this)
			=> @this.ToSelect().Then();

		public static SelectionSelector<_, TIn, TOut> Then<_, TIn, TOut>(this ISelect<_, ISelect<TIn, TOut>> @this)
			=> new SelectionSelector<_, TIn, TOut>(@this);

		public static ExpressionSelector<T> Then<T>(this ISelect<T, Expression> @this)
			=> new ExpressionSelector<T>(@this);

		public static CollectionSelector<_, T> Then<_, T>(this ISelect<_, ICollection<T>> @this)
			=> new CollectionSelector<_, T>(@this);

		public static OpenArraySelector<_, T> Then<_, T>(this ISelect<_, T[]> @this)
			=> new OpenArraySelector<_, T>(@this);

		public static MessageSelector Then(this ISelect<Type, string> @this) => new MessageSelector(@this);

		/**/

		public static Action Selector(this CommandSelector @this) => @this;

		public static Action Selector(this CommandSelector<None> @this) => @this.Get().Execute;

		public static System.Action<T> Selector<T>(this CommandSelector<T> @this) => @this;

		public static Func<T> Selector<T>(this Selector<None, T> @this) => @this.Get().ToResult().Get;

		public static Func<Array<T>> Selector<T>(this Query<None, T> @this) => @this.Get().ToResult().Get;

		public static Func<_, Array<T>> Selector<_, T>(this Query<_, T> @this) => @this.Get().Get;

		/**/

		public static ICommand Out<T>(this CommandSelector<T> @this, T parameter) => @this.Bind(parameter).Command;

		public static IAlteration<T> Out<T>(this CommandSelector<T> @this) => @this.ToConfiguration().Out();

		public static ICondition<T> Out<T>(this Selector<T, bool> @this)
			=> @this.Get().To(x => x as ICondition<T> ?? new Model.Selection.Conditions.Condition<T>(x.Get));

		public static IConditional<TIn, TOut> Out<TIn, TOut>(this Selector<TIn, TOut> @this, ICondition<TIn> condition)
			=> new Conditional<TIn, TOut>(condition, @this.Get);

		public static IAlteration<T> Out<T>(this Selector<T, T> @this) => Alterations<T>.Default.Get(@this.Get().Get);

		public static ISelect<_, Array<T>> Out<_, T>(this OpenArraySelector<_, T> @this) => @this.Get().Result();

		public static IOperation<T> Out<T>(this Selector<T, ValueTask> @this)
			=> @this.Get().To(x => x as IOperation<T> ?? new Operation<T>(x.Get));

		/**/

		public static ICommand<T> ToCommand<T>(this ISelect<T, None> @this) => new InvokeParameterCommand<T>(@this.Get);

		public static ICommand ToAction(this ISelect<None, None> @this)
			=> new Model.Selection.Adapters.Action(@this.ToCommand().Execute);

		public static IResult<T> ToResult<T>(this Selector<None, T> @this) => @this.Get().In(None.Default);

		public static IResult<T> ToResult<T>(this ISelect<None, T> @this) => @this as IResult<T> ??
		                                                                     new Model.Results.Result<T>(@this.Get);

		public static ISelect<_, T> Return<_, T>(this Selector<_, T> @this) => @this.Get();

		public static ISelect<_, Array<T>> Return<_, T>(this Query<_, T> @this) => @this.Get();
	}
}
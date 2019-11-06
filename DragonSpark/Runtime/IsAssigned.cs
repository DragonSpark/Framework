using System;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Runtime
{
	sealed class IsAssigned : InverseCondition<object>
	{
		public static IsAssigned Default { get; } = new IsAssigned();

		IsAssigned() : base(IsNullReference.Default) {}
	}

	sealed class IsAssigned<T> : Select<T, bool>, ICondition<T>
	{
		public static IsAssigned<T> Default { get; } = new IsAssigned<T>();

		IsAssigned() : base(IsAssignedConditions<T>.Default.Get(typeof(T))) {}
	}

	public class IsAssigned<TIn, TOut> : Condition<TIn> where TOut : class
	{
		protected IsAssigned(Func<TIn, TOut> select) : this(select.Start()) {}

		protected IsAssigned(ISelect<TIn, TOut> source) : base(source.Select(IsAssigned.Default)) {}
	}
}
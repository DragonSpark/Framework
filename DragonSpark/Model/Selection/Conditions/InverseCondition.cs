using DragonSpark.Runtime.Activation;
using System;

namespace DragonSpark.Model.Selection.Conditions
{
	public class InverseCondition<T> : ICondition<T>, IActivateUsing<ICondition<T>>
	{
		readonly Func<T, bool> _condition;

		public InverseCondition(ICondition<T> condition) : this(condition.Get) {}

		public InverseCondition(Func<T, bool> inner) => _condition = inner;

		public bool Get(T parameter) => !_condition(parameter);
	}
}
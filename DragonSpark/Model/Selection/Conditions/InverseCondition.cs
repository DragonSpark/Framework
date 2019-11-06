using DragonSpark.Runtime.Activation;

namespace DragonSpark.Model.Selection.Conditions
{
	public class InverseCondition<T> : ICondition<T>, IActivateUsing<ICondition<T>>
	{
		readonly ICondition<T> _condition;

		public InverseCondition(ICondition<T> inner) => _condition = inner;

		public bool Get(T parameter) => !_condition.Get(parameter);
	}
}
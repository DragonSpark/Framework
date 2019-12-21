using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Model.Selection.Conditions
{
	sealed class InverseConditions<T> : ReferenceValueStore<ICondition<T>, InverseCondition<T>>
	{
		public static InverseConditions<T> Default { get; } = new InverseConditions<T>();

		InverseConditions() : base(x => new InverseCondition<T>(x)) {}
	}
}
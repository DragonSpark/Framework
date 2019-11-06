using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Model.Selection.Conditions
{
	sealed class InverseSpecifications<T> : ReferenceValueStore<ICondition<T>, InverseCondition<T>>
	{
		public static InverseSpecifications<T> Default { get; } = new InverseSpecifications<T>();

		InverseSpecifications() : base(x => new InverseCondition<T>(x)) {}
	}
}
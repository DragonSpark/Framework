using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Runtime
{
	sealed class IsNullReference : Condition<object>
	{
		public static IsNullReference Default { get; } = new IsNullReference();

		IsNullReference() : base(EqualsNullReference.Default.Get) {}
	}
}
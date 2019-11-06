using DragonSpark.Runtime.Invocation;

namespace DragonSpark.Runtime
{
	sealed class EqualsNullReference : Invocation0<object, object, bool>
	{
		public static EqualsNullReference Default { get; } = new EqualsNullReference();

		EqualsNullReference() : base(ReferenceEquals, null) {}
	}
}
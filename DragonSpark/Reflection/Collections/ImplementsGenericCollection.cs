using DragonSpark.Reflection.Types;

namespace DragonSpark.Reflection.Collections;

public sealed class ImplementsGenericCollection : ImplementsGenericType
{
	public static ImplementsGenericCollection Default { get; } = new();

	ImplementsGenericCollection() : base(typeof(ICollection<>)) {}
}
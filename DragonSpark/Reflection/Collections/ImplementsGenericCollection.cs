using DragonSpark.Reflection.Types;
using System.Collections.Generic;

namespace DragonSpark.Reflection.Collections;

public sealed class ImplementsGenericCollection : ImplementsGenericType
{
	public static ImplementsGenericCollection Default { get; } = new ImplementsGenericCollection();

	ImplementsGenericCollection() : base(typeof(ICollection<>)) {}
}
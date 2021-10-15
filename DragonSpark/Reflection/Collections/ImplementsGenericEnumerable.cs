using DragonSpark.Reflection.Types;
using System.Collections.Generic;

namespace DragonSpark.Reflection.Collections;

public sealed class ImplementsGenericEnumerable : ImplementsGenericType
{
	public static ImplementsGenericEnumerable Default { get; } = new ImplementsGenericEnumerable();

	ImplementsGenericEnumerable() : base(typeof(IEnumerable<>)) {}
}
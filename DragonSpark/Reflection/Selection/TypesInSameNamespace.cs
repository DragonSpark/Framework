using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using System;
using System.Collections.Generic;

namespace DragonSpark.Reflection.Selection;

public sealed class TypesInSameNamespace : Instances<Type>
{
	public TypesInSameNamespace(Type referenceType, IEnumerable<Type> candidates)
		: base(candidates.Introduce(referenceType.Namespace, x => x.Item1.Namespace == x.Item2)) {}
}
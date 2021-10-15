using System;

namespace DragonSpark.Compose.Extents.Generics;

public sealed class GenericContext
{
	public GenericContext(Type definition) : this(new GenericExtent(definition)) {}

	public GenericContext(GenericExtent extent) => Of = extent;

	public GenericExtent Of { get; }
}
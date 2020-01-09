using System;

namespace DragonSpark.Compose.Extents.Generics
{
	public sealed class GenericContext
	{
		public GenericContext(Type definition) : this(new Extent(definition)) {}

		public GenericContext(Extent extent) => Of = extent;

		public Extent Of { get; }
	}
}
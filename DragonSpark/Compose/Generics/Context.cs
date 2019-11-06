using System;

namespace DragonSpark.Compose.Generics
{
	public sealed class Context
	{
		public Context(Type definition) : this(new Extent(definition)) {}

		public Context(Extent extent) => Of = extent;

		public Extent Of { get; }
	}
}
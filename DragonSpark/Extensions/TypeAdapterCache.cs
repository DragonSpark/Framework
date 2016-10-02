using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.TypeSystem;
using System;

namespace DragonSpark.Extensions
{
	public sealed class TypeAdapterCache : Cache<Type, TypeAdapter>
	{
		public static TypeAdapterCache Default { get; } = new TypeAdapterCache();

		TypeAdapterCache() : base( t => new TypeAdapter( t ) ) {}
	}
}
using DragonSpark.Sources;
using DragonSpark.TypeSystem;
using System;
using System.Collections.Immutable;

namespace DragonSpark.Application
{
	public sealed class ApplicationTypes : DelegatedSource<ImmutableArray<Type>>
	{
		public static ISource<ImmutableArray<Type>> Default { get; } = new ApplicationTypes();
		ApplicationTypes() : base( () => ApplicationParts.Default.Get()?.Types ?? Items<Type>.Immutable ) {}
	}
}
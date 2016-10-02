using System;
using System.Collections.Immutable;
using DragonSpark.Sources;

namespace DragonSpark.Application
{
	public sealed class ApplicationTypes : DelegatedSource<ImmutableArray<Type>>
	{
		public static ISource<ImmutableArray<Type>> Default { get; } = new ApplicationTypes();
		ApplicationTypes() : base( () => ApplicationParts.Default.Get().Types ) {}
	}
}
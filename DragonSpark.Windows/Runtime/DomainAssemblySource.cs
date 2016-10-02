using DragonSpark.Sources.Parameterized.Caching;
using System;
using System.Collections.Immutable;
using System.Reflection;

namespace DragonSpark.Windows.Runtime
{
	public sealed class DomainAssemblySource : FactoryCache<AppDomain, ImmutableArray<Assembly>>
	{
		public static DomainAssemblySource Default { get; } = new DomainAssemblySource();
		DomainAssemblySource() {}

		protected override ImmutableArray<Assembly> Create( AppDomain parameter ) => new AssemblySource( parameter ).Get();
	}
}
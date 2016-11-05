using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.Sources.Scopes;
using JetBrains.Annotations;
using System;
using System.IO;
using System.Reflection;

namespace DragonSpark.Windows.Runtime
{
	public sealed class DomainAssemblies : CacheWithImplementedFactoryBase<AppDomain, Assembly>
	{
		public static DomainAssemblies Default { get; } = new DomainAssemblies();
		DomainAssemblies() : this( AssemblyLoader.Implementation.Get ) {}

		readonly Func<string, Assembly> loader;

		[UsedImplicitly]
		public DomainAssemblies( Func<string, Assembly> loader )
		{
			this.loader = loader;
		}

		protected override Assembly Create( AppDomain parameter )
		{
			try
			{
				return loader( parameter.FriendlyName );
			}
			catch ( FileNotFoundException )
			{
				var result = Assembly.GetEntryAssembly();
				return result;
			}
		}

		public sealed class AssemblyLoader : ParameterizedSingletonScope<string, Assembly>
		{
			public static AssemblyLoader Implementation { get; } = new AssemblyLoader();
			AssemblyLoader() : base( Assembly.Load ) {}
		}
	}
}
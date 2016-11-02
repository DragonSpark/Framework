using DragonSpark.Sources.Parameterized.Caching;
using System;
using System.IO;
using System.Reflection;

namespace DragonSpark.Windows.Runtime
{
	public sealed class DomainAssemblies : FactoryCache<AppDomain, Assembly>
	{
		public static DomainAssemblies Default { get; } = new DomainAssemblies();
		DomainAssemblies() {}

		protected override Assembly Create( AppDomain parameter )
		{
			try
			{
				return Assembly.Load( parameter.FriendlyName );
			}
			catch ( FileNotFoundException )
			{
				var result = Assembly.GetEntryAssembly();
				return result;
			}
		}
	}
}
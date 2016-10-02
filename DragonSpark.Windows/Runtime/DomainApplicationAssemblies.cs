using System;
using System.IO;
using System.Reflection;
using DragonSpark.Sources.Parameterized.Caching;

namespace DragonSpark.Windows.Runtime
{
	public sealed class DomainApplicationAssemblies : FactoryCache<AppDomain, Assembly>
	{
		public static DomainApplicationAssemblies Default { get; } = new DomainApplicationAssemblies();
		DomainApplicationAssemblies() {}

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
using System;

namespace DragonSpark.Windows.Runtime
{
	public static class Defaults
	{
		public static AppDomain Domain { get; } = AppDomainSource.Default.Get( typeof(Sources.Parameterized.Defaults).AssemblyQualifiedName );
	}
}
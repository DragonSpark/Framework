using System.Diagnostics.CodeAnalysis;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;

namespace DragonSpark.Configuration
{
	[SuppressMessage( "ReSharper", "PossibleInfiniteInheritance" )]
	public class ConfigurationSource<T> : SuppliedAndExportedItems<IAlteration<T>>
	{
		public ConfigurationSource( params IAlteration<T>[] configurators ) : base( configurators ) {}
	}
}
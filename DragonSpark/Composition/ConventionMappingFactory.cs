using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using System;

namespace DragonSpark.Composition
{
	public sealed class ConventionMappingFactory : CacheWithImplementedFactoryBase<Type, ConventionMapping?>
	{
		public static IParameterizedSource<Type, ConventionMapping?> Default { get; } = new ConventionMappingFactory();
		ConventionMappingFactory() {}

		protected override ConventionMapping? Create( Type parameter )
		{
			var @interface = ConventionInterfaces.Default.Get( parameter );
			var result = @interface != null ? new ConventionMapping( @interface, parameter ) : (ConventionMapping?)null;
			return result;
		}
	}
}
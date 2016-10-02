using DragonSpark.Sources.Parameterized;
using System;

namespace DragonSpark.Composition
{
	public sealed class ConventionMappingFactory : ParameterizedSourceBase<Type, ConventionMapping>
	{
		public static IParameterizedSource<Type, ConventionMapping> Default { get; } = new ConventionMappingFactory().ToCache();
		ConventionMappingFactory() {}

		public override ConventionMapping Get( Type parameter )
		{
			var @interface = ConventionInterfaces.Default.Get( parameter );
			var result = @interface != null ? new ConventionMapping( @interface, parameter ) : default(ConventionMapping);
			return result;
		}
	}
}
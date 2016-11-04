using System;
using DragonSpark.Sources.Scopes;

namespace DragonSpark.Specifications
{
	public class ConfigurableSpecification<T> : ParameterizedSingletonScope<T, bool>, ISpecification<T>
	{
		public ConfigurableSpecification( Func<object, Func<T, bool>> global ) : base( global ) {}
		public ConfigurableSpecification( Func<T, bool> factory ) : base( factory ) {}
		public bool IsSatisfiedBy( T parameter ) => Get( parameter );
	}
}
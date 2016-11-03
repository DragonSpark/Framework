using DragonSpark.Sources.Scopes;
using System;

namespace DragonSpark.Specifications
{

	public class DelegatedSpecification<T> : SpecificationBase<T>
	{
		readonly Func<T, bool> @delegate;

		public DelegatedSpecification( Func<T, bool> @delegate )
		{
			this.@delegate = @delegate;
		}

		public override bool IsSatisfiedBy( T parameter ) => @delegate.Invoke( parameter );
	}

	public class ConfigurableSpecification<T> : ParameterizedSingletonScope<T, bool>, ISpecification<T>
	{
		public ConfigurableSpecification( Func<object, Func<T, bool>> global ) : base( global ) {}
		public ConfigurableSpecification( Func<T, bool> factory ) : base( factory ) {}
		public bool IsSatisfiedBy( T parameter ) => Get( parameter );
	}
}
using DragonSpark.Sources.Parameterized;
using DragonSpark.Specifications;
using System;

namespace DragonSpark.Activation
{
	public class CompositeActivator : CompositeFactory<Type, object>, IActivator
	{
		readonly ISpecification<Type> specification;

		public CompositeActivator( params IActivator[] activators ) : this( new AnySpecification<Type>( activators ), activators ) {}

		public CompositeActivator( ISpecification<Type> specification, params IActivator[] activators ) : base( activators )
		{
			this.specification = specification;
		}

		public object GetService( Type serviceType ) => Get( serviceType );

		public bool IsSatisfiedBy( Type parameter ) => specification.IsSatisfiedBy( parameter );

		public bool IsSatisfiedBy( object parameter ) => parameter is Type && IsSatisfiedBy( (Type)parameter );
	}
}
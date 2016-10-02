using DragonSpark.Sources;
using System;

namespace DragonSpark.Specifications
{
	public class SuppliedDelegatedSpecification<T> : SpecificationBase<object>
	{
		readonly Func<T, bool> source;
		readonly Func<T> parameterSource;

		public SuppliedDelegatedSpecification( ISpecification<T> specification, T parameter ) : this( specification, Factory.For( parameter ) ) {}
		public SuppliedDelegatedSpecification( ISpecification<T> specification, Func<T> parameterSource ) : this( specification.ToSpecificationDelegate(), parameterSource ) {}

		public SuppliedDelegatedSpecification( Func<T, bool> source, T parameter ) : this( source, Factory.For( parameter ) ) {}
		public SuppliedDelegatedSpecification( Func<T, bool> source, Func<T> parameterSource )// : base( Where<object>.Always )
		{
			this.source = source;
			this.parameterSource = parameterSource;
		}

		public override bool IsSatisfiedBy( object parameter ) => source( parameterSource() );
	}
}
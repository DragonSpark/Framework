using DragonSpark.Sources;
using System;
using System.Runtime.InteropServices;

namespace DragonSpark.Specifications
{
	public class SuppliedDelegatedSpecification<T> : SpecificationBase<object>
	{
		readonly Func<T, bool> source;
		readonly Func<T> parameterSource;

		public SuppliedDelegatedSpecification( ISpecification<T> specification, T parameter ) : this( specification.ToSpecificationDelegate(), parameter ) {}
		public SuppliedDelegatedSpecification( ISpecification<T> specification, Func<T> parameterSource ) : this( specification.ToSpecificationDelegate(), parameterSource ) {}

		public SuppliedDelegatedSpecification( Func<T, bool> source, T parameter ) : this( source, Factory.For( parameter ) ) {}
		public SuppliedDelegatedSpecification( Func<T, bool> source, Func<T> parameterSource )
		{
			this.source = source;
			this.parameterSource = parameterSource;
		}

		public override bool IsSatisfiedBy( [Optional]object parameter ) => source( parameterSource() );
	}
}
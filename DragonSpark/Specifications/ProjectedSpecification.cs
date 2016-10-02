using System;

namespace DragonSpark.Specifications
{
	public class ProjectedSpecification<TOrigin, TDestination> : SpecificationBase<TDestination>
	{
		readonly Func<TOrigin, bool> @delegate;
		readonly Func<TDestination, TOrigin> coerce;

		public ProjectedSpecification( ISpecification<TOrigin> inner, Func<TDestination, TOrigin> projection ) : this( inner.ToSpecificationDelegate(), projection ) {}

		public ProjectedSpecification( Func<TOrigin, bool> @delegate, Func<TDestination, TOrigin> coerce )
		{
			this.@delegate = @delegate;
			this.coerce = coerce;
		}

		public override bool IsSatisfiedBy( TDestination parameter ) => @delegate( coerce( parameter ) );
	}
}
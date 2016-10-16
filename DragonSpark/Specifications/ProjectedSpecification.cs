using System;

namespace DragonSpark.Specifications
{
	public sealed class ProjectedSpecification<TOrigin, TDestination> : SpecificationBase<TDestination>
	{
		readonly Func<TOrigin, bool> @delegate;
		readonly Func<TDestination, TOrigin> coerce;

		public ProjectedSpecification( Func<TOrigin, bool> @delegate, Func<TDestination, TOrigin> coerce )
		{
			this.@delegate = @delegate;
			this.coerce = coerce;
		}

		public override bool IsSatisfiedBy( TDestination parameter ) => @delegate( coerce( parameter ) );
	}
}
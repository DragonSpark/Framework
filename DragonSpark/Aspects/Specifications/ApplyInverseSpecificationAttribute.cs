using System;

namespace DragonSpark.Aspects.Specifications
{
	public sealed class ApplyInverseSpecificationAttribute : ApplySpecificationAttribute
	{
		public ApplyInverseSpecificationAttribute( Type specificationType ) : base( specificationType ) {}

		protected override ISpecification DetermineSpecification() => new Specification( base.DetermineSpecification() );

		sealed class Specification : ISpecification
		{
			readonly ISpecification specification;

			public Specification( ISpecification specification )
			{
				this.specification = specification;
			}

			public bool IsSatisfiedBy( object parameter ) => !specification.IsSatisfiedBy( parameter );
		}
	}
}
using DragonSpark.Specifications;

namespace DragonSpark.Aspects.Specifications
{
	public sealed class SpecificationAdapter<T> : ISpecification
	{
		readonly ISpecification<T> specification;

		public SpecificationAdapter( ISpecification<T> specification )
		{
			this.specification = specification;
		}

		public bool IsSatisfiedBy( object parameter ) => parameter is T && specification.IsSatisfiedBy( (T)parameter );
	}
}
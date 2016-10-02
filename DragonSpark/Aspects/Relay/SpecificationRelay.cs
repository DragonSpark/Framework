using DragonSpark.Specifications;

namespace DragonSpark.Aspects.Relay
{
	public class SpecificationRelay<T> : ISpecificationRelay
	{
		readonly ISpecification<T> specification;
		public SpecificationRelay( ISpecification<T> specification )
		{
			this.specification = specification;
		}

		public bool IsSatisfiedBy( object parameter ) => parameter is T && specification.IsSatisfiedBy( (T)parameter );
	}
}
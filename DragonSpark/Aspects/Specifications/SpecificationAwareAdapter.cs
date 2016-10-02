namespace DragonSpark.Aspects.Specifications
{
	sealed class SpecificationAwareAdapter<T> : ISpecificationAware
	{
		public SpecificationAwareAdapter( ISpecificationAware<T> aware ) : this( new SpecificationAdapter<T>( aware.Specification ) ) {}

		SpecificationAwareAdapter( ISpecification specification )
		{
			Specification = specification;
		}

		public ISpecification Specification { get; }
	}
}
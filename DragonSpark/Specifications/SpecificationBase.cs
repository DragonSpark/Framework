namespace DragonSpark.Specifications
{
	public abstract class SpecificationBase<T> : ISpecification<T>
	{
		public abstract bool IsSatisfiedBy( T parameter );
	}
}
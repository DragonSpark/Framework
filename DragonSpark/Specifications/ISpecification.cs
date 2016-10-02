namespace DragonSpark.Specifications
{
	public interface ISpecification<in T>
	{
		bool IsSatisfiedBy( T parameter );
	}
}
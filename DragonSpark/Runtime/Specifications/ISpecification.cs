namespace DragonSpark.Runtime.Specifications
{
	public interface ISpecification
	{
		bool IsSatisfiedBy( object context );
	}
}
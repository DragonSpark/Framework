namespace DragonSpark.Aspects.Specifications
{
	public interface ISpecification
	{
		bool IsSatisfiedBy( object parameter );
	}
}
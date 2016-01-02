namespace DragonSpark.Runtime.Specifications
{
	public interface ISpecification
	{
		bool IsSatisfiedBy( object context );
	}

	public interface ISpecification<in TContext> : ISpecification
	{
		bool IsSatisfiedBy( TContext parameter );
	}
}
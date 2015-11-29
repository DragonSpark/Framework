namespace DragonSpark.Runtime.Specifications
{
	public abstract class SpecificationBase<TContext> : ISpecification
	{
		protected SpecificationBase( TContext context )
		{
			Context = context;
		}

		protected TContext Context { get; }

		bool ISpecification.IsSatisfiedBy( object context )
		{
			return IsSatisfiedBy( context );
		}


		protected abstract bool IsSatisfiedBy( object context );
	}
}
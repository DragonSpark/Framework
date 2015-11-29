namespace DragonSpark.Runtime.Specifications
{
	public abstract class Specification<TContext> : SpecificationBase<TContext>
	{
		protected Specification( TContext context ) : base( context )
		{}

		protected override bool IsSatisfiedBy( object context )
		{
			var result = context is TContext && IsSatisfiedByContext( (TContext)context );
			return result;
		}

		protected abstract bool IsSatisfiedByContext( TContext context );
	}
}
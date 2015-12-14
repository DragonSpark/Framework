namespace DragonSpark.Runtime.Specifications
{
	public abstract class SpecificationBase<TContext> : ISpecification<TContext>
	{
		bool ISpecification.IsSatisfiedBy( object context )
		{
			var item = context is TContext ? (TContext)context : default(TContext);
			var result = IsSatisfiedBy( item );
			return result;
		}

		public bool IsSatisfiedBy( TContext context )
		{
			return IsSatisfiedByParameter( context );
		}

		protected virtual bool IsSatisfiedByParameter( TContext parameter )
		{
			var result = parameter != null;
			return result;
		}
	}
}
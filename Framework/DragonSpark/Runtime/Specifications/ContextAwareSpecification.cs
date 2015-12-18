namespace DragonSpark.Runtime.Specifications
{
	public abstract class ContextAwareSpecificationBase<TContext> : SpecificationBase<TContext>
	{
		protected ContextAwareSpecificationBase( TContext context )
		{
			Context = context;
		}

		protected TContext Context { get; }
	}
}
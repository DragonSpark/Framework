namespace DragonSpark.Runtime.Specifications
{
	public abstract class ContextAwareSpecificationBase<TContext> : ContextAwareSpecificationBase<TContext, TContext>
	{
		protected ContextAwareSpecificationBase( TContext context ) : base( context )
		{}
	}

	public abstract class ContextAwareSpecificationBase<TContext, TParameter> : SpecificationBase<TParameter>
	{
		protected ContextAwareSpecificationBase( TContext context )
		{
			Context = context;
		}

		protected TContext Context { get; }
	}
}
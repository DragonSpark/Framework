namespace DragonSpark.Specifications
{
	public abstract class SpecificationWithContextBase<TParameter, TContext> : SpecificationBase<TParameter>
	{
		protected SpecificationWithContextBase( TContext context )
		{
			Context = context;
		}

		protected TContext Context { get; }
	}
}
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
			return IsSatisfiedByContext( context );
		}

		protected virtual bool IsSatisfiedByContext( TContext context )
		{
			var result = context != null;
			return result;
		}
	}

	public class AlwaysSpecification : ISpecification
	{
		public static AlwaysSpecification Instance { get; } = new AlwaysSpecification();

		public bool IsSatisfiedBy( object context )
		{
			return true;
		}
	}

	public class NeverSpecification : ISpecification
	{
		public static NeverSpecification Instance { get; } = new NeverSpecification();

		public bool IsSatisfiedBy( object context )
		{
			return false;
		}
	}
}
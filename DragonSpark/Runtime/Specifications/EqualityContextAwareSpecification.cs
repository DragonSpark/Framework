namespace DragonSpark.Runtime.Specifications
{
	public class EqualityContextAwareSpecification : ContextAwareSpecificationBase<object>
	{
		public EqualityContextAwareSpecification( object context ) : base( context )
		{}
		protected override bool IsSatisfiedByContext( object context )
		{
			var result = Equals( Context, context );
			return result;
		}
	}
}
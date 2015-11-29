namespace DragonSpark.Runtime.Specifications
{
	public class EqualitySpecification : Specification<object>
	{
		public EqualitySpecification( object context ) : base( context )
		{}
		protected override bool IsSatisfiedByContext( object context )
		{
			var result = Equals( Context, context );
			return result;
		}
	}
}
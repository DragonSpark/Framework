namespace DragonSpark.Runtime.Specifications
{
	public class EqualityContextAwareSpecification : ContextAwareSpecificationBase<object>
	{
		public EqualityContextAwareSpecification( object context ) : base( context )
		{}
		protected override bool Verify( object parameter )
		{
			var result = Equals( Context, parameter );
			return result;
		}
	}
}
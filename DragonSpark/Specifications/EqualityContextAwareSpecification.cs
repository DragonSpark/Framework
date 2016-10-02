namespace DragonSpark.Specifications
{
	public class EqualityContextAwareSpecification : SpecificationWithContextBase<object>
	{
		public EqualityContextAwareSpecification( object context ) : base( context ) {}

		public override bool IsSatisfiedBy( object parameter ) => Equals( Context, parameter );
	}
}
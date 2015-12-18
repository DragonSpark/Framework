namespace DragonSpark.Runtime.Specifications
{
	public static class SpecificationExtensions
	{
		public static ISpecification Or( this ISpecification @this, ISpecification other )
		{
			return new AnySpecification( @this, other );
		}

		public static ISpecification And( this ISpecification @this, ISpecification other )
		{
			return new AllSpecification( @this, other );
		}
	}
}
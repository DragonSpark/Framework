namespace DragonSpark.Runtime.Specifications
{
	public class FixedSpecification : ISpecification
	{
		readonly bool satisfied;

		public FixedSpecification( bool satisfied )
		{
			this.satisfied = satisfied;
		}

		public bool IsSatisfiedBy( object context ) => satisfied;
	}
}
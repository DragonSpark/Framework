using System;

namespace DragonSpark.Runtime.Specifications
{
	public class FixedSpecification : ISpecification
	{
		readonly Func<bool> resolver;

		public FixedSpecification( bool satisfied ) : this( () => satisfied )
		{}

		public FixedSpecification( Func<bool> resolver )
		{
			this.resolver = resolver;
		}

		public bool IsSatisfiedBy( object context )
		{
			return resolver();
		}
	}
}
using System;

namespace DragonSpark.Runtime.Specifications
{
	public class ProvidedSpecification : ISpecification
	{
		readonly Func<bool> resolver;

		public ProvidedSpecification( Func<bool> resolver )
		{
			this.resolver = resolver;
		}

		public bool IsSatisfiedBy( object context )
		{
			var result = resolver();
			return result;
		}
	}
}
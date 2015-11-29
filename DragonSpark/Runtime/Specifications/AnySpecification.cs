using System.Linq;

namespace DragonSpark.Runtime.Specifications
{
	public class AnySpecification : CompositeSpecification
	{
		public AnySpecification( params ISpecification[] specifications ) : base( specifications.Any )
		{}
	}
}
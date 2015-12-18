using System.Linq;

namespace DragonSpark.Runtime.Specifications
{
	public class AllSpecification : CompositeSpecification
	{
		public AllSpecification( params ISpecification[] specifications ) : base( specifications.All )
		{}
	}
}
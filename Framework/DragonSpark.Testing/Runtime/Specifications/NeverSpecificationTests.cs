using DragonSpark.Runtime.Specifications;
using Xunit;

namespace DragonSpark.Testing.Runtime.Specifications
{
	public class NeverSpecificationTests
	{
		[Fact]
		public void Never()
		{
			Assert.False( NeverSpecification.Instance.IsSatisfiedBy( null ), null );
		} 
	}
}
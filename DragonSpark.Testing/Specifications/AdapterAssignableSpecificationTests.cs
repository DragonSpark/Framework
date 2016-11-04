using DragonSpark.Specifications;
using DragonSpark.TypeSystem;
using Xunit;

namespace DragonSpark.Testing.Specifications
{
	public class AdapterAssignableSpecificationTests
	{
		[Fact]
		public void Coverage()
		{
			Assert.True( new AdapterAssignableSpecification( GetType() ).IsSatisfiedBy( GetType() ) );
			Assert.True( new AdapterAssignableSpecification( GetType().Adapt() ).IsSatisfiedBy( GetType() ) );
			Assert.False( new AdapterAssignableSpecification( GetType() ).IsSatisfiedBy( typeof(string) ) );
			Assert.False( new AdapterAssignableSpecification( GetType().Adapt() ).IsSatisfiedBy( typeof(string) ) );
		}
	}
}
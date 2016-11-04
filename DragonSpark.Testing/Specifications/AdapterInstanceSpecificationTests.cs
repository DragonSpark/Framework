using DragonSpark.Specifications;
using DragonSpark.TypeSystem;
using Xunit;

namespace DragonSpark.Testing.Specifications
{
	public class AdapterInstanceSpecificationTests
	{
		[Fact]
		public void Coverage()
		{
			Assert.True( new AdapterInstanceSpecification( GetType() ).IsSatisfiedBy( this ) );
			Assert.True( new AdapterInstanceSpecification( GetType().Adapt() ).IsSatisfiedBy( this ) );
			Assert.False( new AdapterInstanceSpecification( GetType() ).IsSatisfiedBy( GetType() ) );
			Assert.False( new AdapterInstanceSpecification( GetType().Adapt() ).IsSatisfiedBy( GetType() ) );
		}
	}
}
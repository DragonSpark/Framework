using DragonSpark.Runtime.Specifications;
using Xunit;

namespace DragonSpark.Testing.Runtime.Specifications
{
	public class SpecificationExtensionsTests
	{
		[Fact]
		public void And()
		{
			var sut = new FixedSpecification( true ).And( new FixedSpecification( true ) );
			Assert.True( sut.IsSatisfiedBy( null ) );
		} 

		[Fact]
		public void AndNot()
		{
			var sut = new FixedSpecification( true ).And( new FixedSpecification( false ) );
			Assert.False( sut.IsSatisfiedBy( null ) );
		}

		[Fact]
		public void Or()
		{
			var sut = new FixedSpecification( true ).Or( new FixedSpecification( false ) );
			Assert.True( sut.IsSatisfiedBy( null ) );
		}
	}
}
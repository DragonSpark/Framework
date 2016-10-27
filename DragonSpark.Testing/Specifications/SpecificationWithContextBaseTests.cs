using DragonSpark.Specifications;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace DragonSpark.Testing.Specifications
{
	public class SpecificationWithContextBaseTests
	{
		[Theory, AutoData]
		public void Equal( [Frozen]object item, EqualitySpecification sut )
		{
			Assert.True( sut.IsSatisfiedBy( item ) );
			Assert.False( sut.IsSatisfiedBy( new object() ) );
		} 
	}
}
using DragonSpark.Runtime.Specifications;
using DragonSpark.Testing.Framework.Setup;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace DragonSpark.Testing.Runtime.Specifications
{
	public class EqualityContextAwareSpecificationTests
	{
		[Theory, MoqAutoData]
		public void Equal( [Frozen]object item, EqualityContextAwareSpecification sut )
		{
			Assert.True( sut.IsSatisfiedBy( item ) );
			Assert.False( sut.IsSatisfiedBy( new object() ) );
		} 
	}
}
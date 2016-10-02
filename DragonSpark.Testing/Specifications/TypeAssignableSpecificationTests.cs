using DragonSpark.Specifications;
using DragonSpark.Testing.Objects;
using Xunit;

namespace DragonSpark.Testing.Specifications
{
	public class TypeAssignableSpecificationTests
	{
		[Fact]
		public void Test()
		{
			var sut = TypeAssignableSpecification.Defaults.Get( typeof(IInterface) );

			Assert.True( sut.IsSatisfiedBy( typeof(Class) ) );
			Assert.False( sut.IsSatisfiedBy( GetType() ) );
		} 
	}
}
using DragonSpark.Runtime.Specifications;
using DragonSpark.Testing.Objects;
using Xunit;

namespace DragonSpark.Testing.Runtime.Specifications
{
	public class TypeAssignableSpecificationTests
	{
		[Fact]
		public void Test()
		{
			var sut = new TypeAssignableSpecification( typeof(IInterface) );

			Assert.True( sut.IsSatisfiedBy( typeof(Class) ) );
			Assert.False( sut.IsSatisfiedBy( GetType() ) );
		} 
	}
}
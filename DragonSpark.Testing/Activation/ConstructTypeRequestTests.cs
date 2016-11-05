using DragonSpark.Activation;
using Xunit;

namespace DragonSpark.Testing.Activation
{
	public class ConstructTypeRequestTests
	{
		[Fact]
		public void Equality()
		{
			var first = new ConstructTypeRequest( typeof(Target) );
			var second = new ConstructTypeRequest( typeof(Target) );
			Assert.Equal( first.GetHashCode(), second.GetHashCode() );
			Assert.True( first == second );
			Assert.False( first != second );
		}

		class Target {}
	}
}
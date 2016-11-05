using DragonSpark.Sources.Parameterized;
using Xunit;

namespace DragonSpark.Testing.Sources.Parameterized
{
	public class AlterationAdapterTests
	{
		[Fact]
		public void Verify()
		{
			var instance = new object();
			var sut = new AlterationAdapter<object>( o => instance );
			Assert.Same( instance, sut.Get( new object() ) );
		}
	}
}
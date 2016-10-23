using DragonSpark.Sources;
using Xunit;

namespace DragonSpark.Testing.Sources.Parameterized
{
	public class ConfiguringFactoryTests
	{
		[Fact]
		public void Create()
		{
			var count = 0;
			var sut = new ConfiguringFactory<object>( () => new object(), () => count++ );
			Assert.Equal( 0, count );
			Assert.NotNull( sut.Get() );
			Assert.Equal( 1, count );
		}
	}
}
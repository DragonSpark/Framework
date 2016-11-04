using DragonSpark.Composition;
using Xunit;

namespace DragonSpark.Testing.Composition
{
	public class SingletonExportFactoryTests
	{
		[Fact]
		public void Coverage()
		{
			Assert.Null( SingletonExportFactory.Default.Get( GetType() ) );
		}

		public string PropertyName { get; set; }
	}
}
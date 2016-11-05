using DragonSpark.Windows.FileSystem;
using Xunit;

namespace DragonSpark.Windows.Testing.FileSystem
{
	public class TimestampNameFactoryTests
	{
		[Fact]
		public void Verify()
		{
			var result = TimestampNameFactory.Default.Get();
			Assert.Equal( "1976-06-07--23-17-57", result );
		}
	}
}
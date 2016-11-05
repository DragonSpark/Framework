using DragonSpark.Windows.Setup;
using Xunit;

namespace DragonSpark.Windows.Testing.Setup
{
	public class InputOutputTests
	{
		[Fact]
		public void Coverage()
		{
			Assert.NotNull( InputOutput.Default.Reader );
			Assert.NotNull( InputOutput.Default.Writer );
		}
	}
}
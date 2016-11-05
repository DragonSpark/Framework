using Xunit;

namespace DragonSpark.Windows.Testing
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
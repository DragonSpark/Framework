using DragonSpark.Windows.Setup;
using Xunit;

namespace DragonSpark.Windows.Testing.Setup
{
	public class ReadKeyCommandTests
	{
		[Fact]
		public void Verify() => new ReadKeyCommand { Message = "Hello World", Exiting = "Exiting..." }.Execute();
	}
}
using DragonSpark.Windows.Diagnostics;
using Serilog;
using Xunit;

namespace DragonSpark.Windows.Testing.Diagnostics
{
	public class AddConsoleSinkCommandTests
	{
		[Fact]
		public void Coverage()
		{
			var sut = new AddConsoleSinkCommand();
			sut.OutputTemplate = sut.OutputTemplate;
			sut.Execute( new LoggerConfiguration() );
		}
	}
}
using DragonSpark.Windows.Diagnostics;
using Serilog;
using Xunit;

namespace DragonSpark.Windows.Testing.Diagnostics
{
	public class AddTraceSinkCommandTests
	{
		[Fact]
		public void Coverage()
		{
			var sut = new AddTraceSinkCommand();
			sut.OutputTemplate = sut.OutputTemplate;
			sut.Execute( new LoggerConfiguration() );
		}
	}
}
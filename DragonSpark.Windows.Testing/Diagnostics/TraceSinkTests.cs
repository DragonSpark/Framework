using DragonSpark.Windows.Diagnostics;
using Moq;
using Ploeh.AutoFixture.Xunit2;
using Serilog.Events;
using Serilog.Formatting;
using System.IO;
using Xunit;

namespace DragonSpark.Windows.Testing.Diagnostics
{
	public class TraceSinkTests
	{
		[Theory, DragonSpark.Testing.Framework.Application.AutoData]
		void Coverage( [Frozen]ITextFormatter formatter, TraceSink sut, LogEvent item )
		{
			var mock = Mock.Get( formatter );
			mock.Setup( textFormatter => textFormatter.Format( item, It.IsAny<StringWriter>() ) ).Verifiable();
			sut.Emit( item );

			mock.Verify( textFormatter => textFormatter.Format( item, It.IsAny<StringWriter>() ), Times.Once );
		}
	}
}
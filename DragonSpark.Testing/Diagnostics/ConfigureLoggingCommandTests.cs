using DragonSpark.Diagnostics;
using Xunit;

namespace DragonSpark.Testing.Diagnostics
{
	public class ConfigureLoggingCommandTests
	{
		[Fact]
		public void Coverage()
		{
			using ( new DefaultConfigureLoggingCommand() ) {}
			new DelegatedTextCommand( IgnoredOutputCommand.Default.Execute ).Execute( string.Empty );
			DebugOutputCommand.Default.Execute( string.Empty );
		}
	}
}
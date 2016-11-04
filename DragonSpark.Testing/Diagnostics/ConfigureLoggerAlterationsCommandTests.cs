using DragonSpark.Diagnostics;
using DragonSpark.Sources.Scopes;
using Xunit;

namespace DragonSpark.Testing.Diagnostics
{
	public class ConfigureLoggerAlterationsCommandTests
	{
		[Fact]
		public void Coverage()
		{
			var before = LoggerAlterations.Default.Get();
			ConfigureLoggerAlterationsCommand.Default.Execute( LoggerAlterations.Default.Scoped );
			Assert.Equal( before, LoggerAlterations.Default.Get() );
			new DelegatedTextCommand( IgnoredOutputCommand.Default.Execute ).Execute( string.Empty );
			DebugOutputCommand.Default.Execute( string.Empty );
		}
	}
}
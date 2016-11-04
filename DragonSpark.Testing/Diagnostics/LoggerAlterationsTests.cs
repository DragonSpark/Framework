using DragonSpark.Diagnostics;
using Xunit;

namespace DragonSpark.Testing.Diagnostics
{
	public class LoggerAlterationsTests
	{
		[Fact]
		public void Coverage()
		{
			Assert.NotNull( LoggerAlterations.Configure.Implementation );
			new DelegatedTextCommand( IgnoredOutputCommand.Default.Execute ).Execute( string.Empty );
			DebugOutputCommand.Default.Execute( string.Empty );
		}
	}
}
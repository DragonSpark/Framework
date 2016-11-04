using DragonSpark.Diagnostics;
using DragonSpark.Sources.Scopes;
using Xunit;

namespace DragonSpark.Testing.Diagnostics
{
	public class LoggerAlterationsTests
	{
		[Fact]
		public void Coverage()
		{
			var before = LoggerAlterations.Default.Get();
			LoggerAlterations.Configure.Implementation.Execute( LoggerAlterations.Default.Scoped );
			Assert.Equal( before, LoggerAlterations.Default.Get() );
			new DelegatedTextCommand( IgnoredOutputCommand.Default.Execute ).Execute( string.Empty );
			DebugOutputCommand.Default.Execute( string.Empty );
		}
	}
}
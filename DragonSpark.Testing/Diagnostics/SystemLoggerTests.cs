using DragonSpark.Diagnostics;
using DragonSpark.Sources;
using Serilog;
using System.Collections.Immutable;
using Xunit;

namespace DragonSpark.Testing.Diagnostics
{
	public class SystemLoggerTests
	{
		[Fact]
		public void Verify()
		{
			var history = LoggingHistory.Default.Get();
			var sink = new LoggerHistorySink();
			LoggingConfiguration.Default.Seed.Assign( o => new LoggerConfiguration().WriteTo.Sink( sink ) );

			var system = SystemLogger.Default.Get();
			system.Information( "Hello World!" );

			Assert.Empty( sink.Events );
			var events = history.Events.ToImmutableHashSet();
			Assert.Single( events );

			var logger = Logger.Default.ToScope().Get();
			Assert.Empty( history.Events );
			var set = sink.Events.ToImmutableHashSet();
			Assert.Single( set );
			Assert.Equal( events, set );

			Assert.NotSame( system, logger );
			
			Assert.Same( logger, SystemLogger.Default.Get() );
		}
	}
}
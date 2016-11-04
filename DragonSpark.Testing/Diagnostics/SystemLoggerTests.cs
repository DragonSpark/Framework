using DragonSpark.Diagnostics;
using DragonSpark.Sources.Scopes;
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
			LoggerFactory.Default.Configuration.Assign( x => new LoggerConfiguration().WriteTo.Sink( sink ) );

			var system = SystemLogger.Default.Get();
			system.Information( "Hello World!" );

			Assert.Empty( sink.Events );
			var events = history.Events.ToImmutableHashSet();
			Assert.Single( events );

			var logger = Logger.Default.ToExecutionScope().Get();
			Assert.Empty( history.Events );
			var set = sink.Events.ToImmutableHashSet();
			Assert.Single( set );
			Assert.Equal( events, set );

			Assert.NotSame( system, logger );
			
			Assert.Same( logger, SystemLogger.Default.Get() );
		}
	}
}
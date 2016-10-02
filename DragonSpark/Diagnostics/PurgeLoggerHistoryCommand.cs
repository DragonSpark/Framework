using Serilog.Events;
using System;
using System.Collections.Immutable;

namespace DragonSpark.Diagnostics
{
	public sealed class PurgeLoggerHistoryCommand : PurgeLoggerHistoryCommandBase<LogEvent>
	{
		public static PurgeLoggerHistoryCommand Default { get; } = new PurgeLoggerHistoryCommand();
		PurgeLoggerHistoryCommand() : this( LoggingHistory.Default.Get ) {}

		public PurgeLoggerHistoryCommand( Func<ILoggerHistory> historySource ) : base( historySource, events => events.ToImmutableArray() ) {}
	}
}
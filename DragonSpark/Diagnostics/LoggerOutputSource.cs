using DragonSpark.Sources;
using Serilog;
using Serilog.Events;
using System;

namespace DragonSpark.Diagnostics
{
	public sealed class LoggerOutputSource : SourceBase<Action<LogEvent>>
	{
		public static LoggerOutputSource Default { get; } = new LoggerOutputSource();
		LoggerOutputSource() : this( Defaults.Factory ) {}

		readonly Func<ILogger> source;

		public LoggerOutputSource( Func<ILogger> source )
		{
			this.source = source;
		}

		public override Action<LogEvent> Get() => source().Write;
	}
}
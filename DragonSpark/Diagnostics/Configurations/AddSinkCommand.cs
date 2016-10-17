using Serilog.Configuration;
using Serilog.Events;
using System;

namespace DragonSpark.Diagnostics.Configurations
{
	public abstract class AddSinkCommand : ConfigureLoggerBase<LoggerSinkConfiguration>
	{
		protected AddSinkCommand() : this( LogEventLevel.Verbose ) {}

		protected AddSinkCommand( LogEventLevel restrictedToMinimumLevel ) : base( configuration => configuration.WriteTo )
		{
			RestrictedToMinimumLevel = restrictedToMinimumLevel;
		}

		public IFormatProvider FormatProvider { get; set; }

		public LogEventLevel RestrictedToMinimumLevel { get; set; }
	}
}
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using DragonSpark.Diagnostics.Logging.Configuration;
using System;

namespace DragonSpark.Diagnostics.Logging
{
	public class SeqConfiguration : ILoggingSinkConfiguration
	{
		readonly LoggingLevelSwitch _switch;
		readonly Uri                      _uri;

		public SeqConfiguration(Uri uri) : this(uri, LoggingLevelController.Default) {}

		public SeqConfiguration(Uri uri, LoggingLevelSwitch @switch)
		{
			_uri    = uri;
			_switch = @switch;
		}

		public LoggerConfiguration Get(LoggerSinkConfiguration parameter)
			=> parameter.Seq(_uri.ToString(), controlLevelSwitch: _switch);
	}
}
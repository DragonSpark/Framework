using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using DragonSpark.Diagnostics.Logging.Configuration;
using DragonSpark.Model.Selection.Alterations;
using System;

namespace DragonSpark.Diagnostics.Logging
{
	public class LoggerSinkDecoration : ILoggingConfiguration
	{
		readonly Action<LoggerSinkConfiguration>    _configure;
		readonly LoggingLevelSwitch                 _switch;
		readonly Func<ILogEventSink, ILogEventSink> _sink;

		public LoggerSinkDecoration(IAlteration<ILogEventSink> sink, ILoggingSinkConfiguration configuration)
			: this(sink.Get, configuration) {}

		public LoggerSinkDecoration(Func<ILogEventSink, ILogEventSink> sink, ILoggingSinkConfiguration configuration)
			: this(sink, configuration.Then().Terminate(), LoggingLevelController.Default) {}

		public LoggerSinkDecoration(Func<ILogEventSink, ILogEventSink> sink, Action<LoggerSinkConfiguration> configure,
		                            LoggingLevelSwitch @switch)
		{
			_sink      = sink;
			_configure = configure;
			_switch    = @switch;
		}

		public LoggerConfiguration Get(LoggerConfiguration parameter)
			=> LoggerSinkConfiguration.Wrap(parameter.WriteTo, _sink, _configure, LogEventLevel.Information, _switch);
	}
}
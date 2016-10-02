using DragonSpark.Sources.Parameterized;
using Serilog.Events;
using Serilog.Formatting.Display;
using System.IO;

namespace DragonSpark.Diagnostics
{
	public sealed class LogEventTextFactory : ParameterizedSourceBase<LogEvent, string>
	{
		
		public static LogEventTextFactory Default { get; } = new LogEventTextFactory();
		LogEventTextFactory( string template = Defaults.Template ) : this( new MessageTemplateTextFormatter( template, null ) ) {}

		readonly MessageTemplateTextFormatter formatter;

		public LogEventTextFactory( MessageTemplateTextFormatter formatter )
		{
			this.formatter = formatter;
		}

		public override string Get( LogEvent parameter )
		{
			var writer = new StringWriter();
			formatter.Format( parameter, writer );
			var result = writer.ToString().Trim();
			return result;
		}
	}
}
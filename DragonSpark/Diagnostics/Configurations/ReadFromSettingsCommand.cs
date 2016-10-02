using PostSharp.Patterns.Contracts;
using Serilog.Configuration;

namespace DragonSpark.Diagnostics.Configurations
{
	public class ReadFromSettingsCommand : ReadFromCommandBase
	{
		public ReadFromSettingsCommand( ILoggerSettings settings )
		{
			Settings = settings;
		}

		[Required]
		public ILoggerSettings Settings { [return: Required]get; set; }

		protected override void Configure( LoggerSettingsConfiguration configuration ) => configuration.Settings( Settings );
	}
}
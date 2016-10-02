using DragonSpark.Diagnostics;
using DragonSpark.Diagnostics.Configurations;
using PostSharp.Patterns.Contracts;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;

namespace DragonSpark.Windows.Diagnostics
{
	public class AddConsoleSinkCommand : AddSinkCommand
	{
		public AddConsoleSinkCommand() : this( Defaults.Template, LogEventLevel.Verbose ) {}

		public AddConsoleSinkCommand( [NotEmpty]string outputTemplate, LogEventLevel restrictedToMinimumLevel ) : base( restrictedToMinimumLevel )
		{
			OutputTemplate = outputTemplate;
		}

		[NotEmpty]
		public string OutputTemplate { [return: NotEmpty]get; set; }

		protected override void Configure( LoggerSinkConfiguration configuration )
			=> configuration.ColoredConsole( RestrictedToMinimumLevel, OutputTemplate, FormatProvider );
	}
}
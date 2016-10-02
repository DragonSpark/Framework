using DragonSpark.Diagnostics;
using DragonSpark.Diagnostics.Configurations;
using PostSharp.Patterns.Contracts;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;

namespace DragonSpark.Windows.Diagnostics
{
	public class AddRollingFileSinkCommand : AddSinkCommand
	{
		public AddRollingFileSinkCommand() : this( Defaults.Template, 1073741824, 31, LogEventLevel.Verbose ) {}

		public AddRollingFileSinkCommand( string outputTemplate, long fileSizeLimitBytes, int retainedFileCountLimit, LogEventLevel restrictedToMinimumLevel ) : base( restrictedToMinimumLevel )
		{
			OutputTemplate = outputTemplate;
			FileSizeLimitBytes = fileSizeLimitBytes;
			RetainedFileCountLimit = retainedFileCountLimit;
		}

		[NotEmpty]
		public string PathFormat { [return: NotEmpty]get; set; }

		public string OutputTemplate { get; set; }

		public long FileSizeLimitBytes { get; set; }

		public int RetainedFileCountLimit { get; set; }

		protected override void Configure( LoggerSinkConfiguration configuration ) 
			=> configuration.RollingFile( PathFormat, RestrictedToMinimumLevel, OutputTemplate, FormatProvider, FileSizeLimitBytes, RetainedFileCountLimit );
	}
}
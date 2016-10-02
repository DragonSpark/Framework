using Serilog;
using Serilog.Configuration;
using System.IO;

namespace DragonSpark.Diagnostics.Configurations
{
	public class AddTextWriterCommand : AddSinkCommand
	{
		public AddTextWriterCommand() : this( new StringWriter(), "{Timestamp} [{Level}] {Message}{NewLine}{Exception}" ) {}

		public AddTextWriterCommand( TextWriter writer, string outputTemplate )
		{
			Writer = writer;
			OutputTemplate = outputTemplate;
		}

		public TextWriter Writer { get; set; }

		public string OutputTemplate { get; set; }

		protected override void Configure( LoggerSinkConfiguration configuration ) => configuration.TextWriter( Writer, RestrictedToMinimumLevel, OutputTemplate, FormatProvider );
	}
}
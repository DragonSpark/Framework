using Serilog;
using Serilog.Configuration;
using System.IO;

namespace DragonSpark.Diagnostics.Configurations
{
	public class AddTextWriterCommand : AddSinkCommand
	{
		const string Template = "{Timestamp} [{Level}] {Message}{NewLine}{Exception}";

		public AddTextWriterCommand() : this( Template ) {}

		public AddTextWriterCommand( string outputTemplate ) : this( new StringWriter(), outputTemplate ) {}

		public AddTextWriterCommand( TextWriter writer, string outputTemplate = Template )
		{
			Writer = writer;
			OutputTemplate = outputTemplate;
		}

		public TextWriter Writer { get; set; }

		public string OutputTemplate { get; set; }

		protected override void Configure( LoggerSinkConfiguration configuration ) => configuration.TextWriter( Writer, RestrictedToMinimumLevel, OutputTemplate, FormatProvider );
	}
}
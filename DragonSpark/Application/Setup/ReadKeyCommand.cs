using DragonSpark.Commands;
using JetBrains.Annotations;
using System.ComponentModel;
using System.IO;

namespace DragonSpark.Application.Setup
{
	public class ReadKeyCommand : CommandBase<IInputOutput>
	{
		[DefaultValue( "Press Enter to Continue..." ), UsedImplicitly]
		public string Message { get; set; }

		[DefaultValue( "Now Exiting... Have a Nice Day. :)" ), UsedImplicitly]
		public string Exiting { get; set; }

		public override void Execute( IInputOutput parameter )
		{
			parameter.Writer.WriteLine();
			parameter.Writer.Write( Message );
			parameter.Reader.ReadLine();

			parameter.Writer.Write( Exiting );
		}
	}

	public interface IInputOutput
	{
		TextReader Reader { get; }
		TextWriter Writer { get; }
	}

	public class InputOutput : IInputOutput
	{
		public InputOutput( TextWriter writer, TextReader reader )
		{
			Writer = writer;
			Reader = reader;
		}

		public TextWriter Writer { get; }
		public TextReader Reader { get; }
	}
}

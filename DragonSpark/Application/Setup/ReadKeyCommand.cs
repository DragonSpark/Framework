using DragonSpark.Commands;
using JetBrains.Annotations;
using System.ComponentModel;

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
}

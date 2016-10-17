using DragonSpark.Commands;
using System;
using System.ComponentModel;

namespace DragonSpark.Windows.Setup
{
	public class ReadKeyCommand : RunCommandBase
	{
		[DefaultValue( "Press Enter to Continue..." )]
		public string Message { get; set; }

		[DefaultValue( "Now Exiting... Have a Nice Day. :)" )]
		public string Exiting { get; set; }

		public override void Execute()
		{
			Console.WriteLine();
			Console.Write( Message );
			Console.ReadLine();

			Console.Write( Exiting );
		}
	}
}
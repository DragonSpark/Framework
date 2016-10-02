using System;
using System.ComponentModel;
using DragonSpark.Commands;

namespace DragonSpark.Windows.Setup
{
	public class ReadKeyCommand : CommandBase<object>
	{
		[DefaultValue( "Press Enter to Continue..." )]
		public string Message { get; set; }

		[DefaultValue( "Now Exiting... Have a Nice Day. :)" )]
		public string Closing { get; set; }

		public override void Execute( object parameter )
		{
			Console.WriteLine();
			Console.Write( Message );
			Console.ReadLine();

			Console.Write( Closing );
		}
	}
}
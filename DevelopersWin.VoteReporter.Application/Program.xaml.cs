using DragonSpark.Setup;
using DragonSpark.Setup.Commands;

namespace DevelopersWin.VoteReporter.Application
{
	public partial class Program
	{
		public Program()
		{
			InitializeComponent();
			Commands.Add( new AssignServiceLocatorCommand() );
		}

		static void Main( string[] args )
		{
			new Program().Run( args );
		}
	}
}

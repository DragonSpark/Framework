using DragonSpark.ComponentModel;
using DragonSpark.Runtime;

namespace DragonSpark.Setup.Commands
{
	public class RunProgramCommand : SetupCommand
	{
		[Activate]
		public IProgram Program { get; set; }

		protected override void Execute( SetupContext context )
		{
			Program.Run( context.GetArguments() );
		}
	}
}
using DragonSpark.ComponentModel;

namespace DragonSpark.Setup
{
	public class RunProgramCommand : SetupCommand
	{
		[Activate]
		public IProgram Program { get; set; }

		protected override void Execute( SetupContext context )
		{
			Program.Run( context.GetArguments<object>() );
		}
	}
}
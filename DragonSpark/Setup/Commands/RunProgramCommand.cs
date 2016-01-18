using DragonSpark.ComponentModel;
using DragonSpark.Runtime;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Setup.Commands
{
	public class RunProgramCommand : SetupCommand
	{
		[Locate]
		public IProgram Program { [return: Required]get; set; }

		protected override void OnExecute( ISetupParameter parameter ) => Program.Run( parameter.Arguments );
	}
}
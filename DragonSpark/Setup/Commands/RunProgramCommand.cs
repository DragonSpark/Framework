using DragonSpark.ComponentModel;
using DragonSpark.Runtime;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Setup.Commands
{
	public class RunProgramCommand : SetupCommandBase
	{
		[Locate]
		public IProgram Program { [return: Required]get; set; }

		protected override void OnExecute( object parameter ) => Program.Run( parameter );
	}
}
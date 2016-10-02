using DragonSpark.Commands;
using DragonSpark.ComponentModel;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Application
{
	public class RunProgramCommand : CommandBase<object>
	{
		[Service]
		public IProgram Program { [return: Required]get; set; }

		public override void Execute( object parameter ) => Program.Run( parameter );
	}
}
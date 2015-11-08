using DragonSpark.ComponentModel;

namespace DragonSpark.Setup
{
	public interface ISetup
	{
		void Run( object arguments = null );
	}

	public interface IProgram
	{
		void Run( object arguments );
	}

	public abstract class Program<TArguments> : IProgram
	{
		void IProgram.Run( object arguments )
		{
			Run( (TArguments)arguments );
		}

		protected abstract void Run( TArguments arguments );
	}

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
using DragonSpark.Setup;

namespace DragonSpark.Testing.Framework.Setup
{
	public abstract class SetupAutoDataCommandBase : SetupCommand
	{
		protected override void Execute( SetupContext context )	
		{
			var setup = context.GetArguments<SetupAutoDataParameter>();
			OnSetup( context, setup );
		}

		protected abstract void OnSetup( SetupContext context, SetupAutoDataParameter setup );
	}
}
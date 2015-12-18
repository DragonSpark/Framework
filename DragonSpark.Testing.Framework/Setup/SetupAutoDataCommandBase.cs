using DragonSpark.Setup;

namespace DragonSpark.Testing.Framework.Setup
{
	public abstract class SetupAutoDataCommandBase : SetupCommand
	{
		protected override void Execute( SetupContext context )	
		{
			var setup = context.GetArguments<SetupAutoDataContext>();
			OnSetup( context, setup );
		}

		protected abstract void OnSetup( SetupContext context, SetupAutoDataContext setup );
	}
}
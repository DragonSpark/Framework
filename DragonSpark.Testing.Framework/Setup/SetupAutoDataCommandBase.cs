using DragonSpark.Setup;

namespace DragonSpark.Testing.Framework.Setup
{
	public abstract class SetupAutoDataCommandBase : SetupCommand
	{
		protected override void Execute( ISetupParameter parameter )	
		{
			var setup = parameter.GetArguments<SetupAutoDataParameter>();
			OnSetup( parameter, setup );
		}

		protected abstract void OnSetup( ISetupParameter parameter, SetupAutoDataParameter setup );
	}
}
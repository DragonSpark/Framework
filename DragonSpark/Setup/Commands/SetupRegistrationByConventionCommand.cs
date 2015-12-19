using DragonSpark.Setup.Registration;

namespace DragonSpark.Setup.Commands
{
	public abstract class SetupRegistrationByConventionCommand : UnityRegistrationCommand
	{
		protected override void Execute( SetupContext context )
		{
			var service = DetermineService( context );
			var provider = DetermineProfileProvider( context );
			var profile = provider.Retrieve();
			service.Register( profile );
		}

		protected abstract IConventionRegistrationProfileProvider DetermineProfileProvider( SetupContext context );

		protected abstract IConventionRegistrationService DetermineService( SetupContext context );
	}
}
namespace DragonSpark.Setup
{
	public abstract class SetupRegistrationByConventionCommand : SetupCommand
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
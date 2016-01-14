using DragonSpark.Activation.IoC;
using DragonSpark.Setup.Commands;
using DragonSpark.Setup.Registration;

namespace DragonSpark.Windows.Setup
{
	public class SetupRegistrationByConventionCommand : DelegatedSetupCommand<RegisterFromMetadataCommand, ConventionRegistrationProfile> {}

	public class RegisterFromMetadataCommand : DragonSpark.Setup.Registration.RegisterFromMetadataCommand
	{
		public RegisterFromMetadataCommand( PersistingServiceRegistry registry ) : base( registry ) {}
	}
	/*{
		[InjectionConstructor]
		public RegistrationByConventionCommand( IUnityContainer container, LifetimeManagerFactory<ContainerControlledLifetimeManager> factory ) : this( container, factory ) { }

		public RegistrationByConventionCommand( 
			[Required]IUnityContainer container, 
			[Required]IFactory<ActivateParameter, LifetimeManager> lifetimeFactory 
		) : base( new ICommand<ConventionRegistrationProfile>[]
		{
			new RegisterFromMetadataCommand( new ServiceRegistry( container, lifetimeFactory ) )
		} ) {}
	}*/
}
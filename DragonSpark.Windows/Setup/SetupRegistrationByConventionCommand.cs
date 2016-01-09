using DragonSpark.Activation.FactoryModel;
using DragonSpark.Activation.IoC;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Setup.Commands;
using DragonSpark.Setup.Registration;
using DragonSpark.TypeSystem;
using Microsoft.Practices.Unity;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Windows.Setup
{
	public class SetupRegistrationByConventionCommand : DelegatedSetupCommand<RegistrationByConventionCommand, ConventionRegistrationProfile>
	{ }

	public class RegistrationByConventionCommand : CompositeCommand<ConventionRegistrationProfile>
	{
		[InjectionConstructor]
		public RegistrationByConventionCommand( IAttributeProvider provider, IUnityContainer container, IMessageLogger messageLogger ) : this( provider, container, messageLogger, container.Resolve<LifetimeManagerFactory<ContainerControlledLifetimeManager>>() ) { }

		public RegistrationByConventionCommand( 
			[Required]IAttributeProvider provider, 
			[Required]IUnityContainer container, 
			[Required]IMessageLogger messageLogger, 
			[Required]IFactory<ActivateParameter, LifetimeManager> lifetimeFactory 
		) : base( new ICommand<ConventionRegistrationProfile>[]
		{
			new RegisterFromMetadataCommand( new ServiceRegistry( container, lifetimeFactory ) ),
			new ConventionRegistrationCommand( provider, container, messageLogger, lifetimeFactory )
		} ) {}
	}
}
using DragonSpark.Activation;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Activation.IoC;
using DragonSpark.ComponentModel;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using Microsoft.Practices.Unity;

namespace DragonSpark.Setup.Registration
{
	public class UnityConventionRegistrationService : IConventionRegistrationService
	{
		public UnityConventionRegistrationService( IUnityContainer container, IMessageLogger messageLogger  ) : this( container, messageLogger, new LifetimeManagerFactory<ContainerControlledLifetimeManager>( container.Resolve<IActivator>() ) )
		{}

		public UnityConventionRegistrationService( IUnityContainer container, IMessageLogger messageLogger, IFactory<ActivateParameter, LifetimeManager> factory  )
		{
			Container = container;
			MessageLogger = messageLogger;
			Factory = factory;
		}

		protected IUnityContainer Container { get; }
		protected IMessageLogger MessageLogger { get; }
		protected IFactory<ActivateParameter, LifetimeManager> Factory { get; }

		public virtual void Register( ConventionRegistrationProfile profile )
		{
			var registry = new ServiceRegistry( Container, MessageLogger, Factory );

			profile.Candidates
				.AsTypeInfos()
				.WhereDecorated<RegistrationBaseAttribute>()
				.Each( item => HostedValueLocator<IConventionRegistration>.Instance.Create( item.Item2 ).Each( registration =>
				{
					registration.Register( registry, item.Item2.AsType() );
				} ) );
		}
	}
}
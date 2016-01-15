using DragonSpark.Activation.FactoryModel;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Setup;
using DragonSpark.TypeSystem;
using Microsoft.Practices.Unity;
using PostSharp.Patterns.Contracts;
using System.Reflection;
using DragonSpark.Aspects;
using DragonSpark.Properties;
using DragonSpark.Runtime;
using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Activation.IoC
{
	public class UnityContainerFactory<TAssemblyProvider, TLogger> : UnityContainerFactory
		where TAssemblyProvider : IAssemblyProvider
		where TLogger : class, IMessageLogger

	{
		public UnityContainerFactory() : this( Activation.Activator.Activate<TAssemblyProvider>() ) {}

		public UnityContainerFactory( [Required]TAssemblyProvider provider ) : base( provider.Create(), MessageLoggerFactory<TLogger>.Instance.Create() ) {}
	}

	public class ServiceLocatorFactory<TAssemblyProvider, TLogger> : ServiceLocatorFactory
		where TAssemblyProvider : IAssemblyProvider
		where TLogger : class, IMessageLogger
	{
		public ServiceLocatorFactory() : this( new UnityContainerFactory<TAssemblyProvider, TLogger>().Create() ) { }

		public ServiceLocatorFactory( IUnityContainer container ) : base( container, new AssignLocationCommand( Services.Location, container, container.Logger() ) ) { }
	}

	public class UnityContainerFactory : FactoryBase<IUnityContainer>
	{
		readonly Assembly[] assemblies;
		readonly IMessageLogger logger;

		public UnityContainerFactory() : this( Assemblies.GetCurrent() ) {}

		public UnityContainerFactory( Assembly[] assemblies ) : this( assemblies, MessageLoggerFactory<RecordingMessageLogger>.Instance.Create() ) {}

		public UnityContainerFactory( [Required]Assembly[] assemblies, [Required]IMessageLogger logger )
		{
			this.assemblies = assemblies;
			this.logger = logger;
		}

		protected override IUnityContainer CreateItem()
		{
			var result = new UnityContainer()
				.Extend<RegistrationMonitorExtension>()
				.RegisterInstance( assemblies )
				.RegisterInstance( logger )
				.Extend<BuildPipelineExtension>();
			return result;
		}
	}

	public class FrozenDisposeContainerControlledLifetimeManager : ContainerControlledLifetimeManager
	{
		[Freeze]
		protected override void Dispose( bool disposing ) => base.Dispose( disposing );
	}

	public class ConfigureLocationCommand : Command<IServiceLocator>
	{
		readonly IServiceLocation location;
		readonly IUnityContainer container;
		readonly IMessageLogger logger;

		public ConfigureLocationCommand( [Required]IServiceLocation location, [Required]IUnityContainer container, [Required]IMessageLogger logger )
		{
			this.location = location;
			this.container = container;
			this.logger = logger;
		}

		protected override void OnExecute( IServiceLocator parameter )
		{
			logger.Information( Resources.ConfiguringServiceLocatorSingleton, Priority.Low );
			container.RegisterInstance( location );
			container.RegisterInstance( parameter, new FrozenDisposeContainerControlledLifetimeManager() );
		}
	}

	public class AssignLocationCommand : ConfigureLocationCommand
	{
		readonly IServiceLocation location;

		public AssignLocationCommand( IServiceLocation location, IUnityContainer container, IMessageLogger logger ) : base( location, container, logger )
		{
			this.location = location;
		}

		protected override void OnExecute( IServiceLocator parameter )
		{
			base.OnExecute( parameter );
			location.Assign( parameter );
		}
	}

	public class ServiceLocatorFactory : FactoryBase<IServiceLocator>
	{
		readonly IUnityContainer container;
		readonly ICommand<IServiceLocator> created;

		public ServiceLocatorFactory() : this( Factory.Create<UnityContainer>() ) {}

		protected ServiceLocatorFactory( [Required]IUnityContainer container ) : this( container, new ConfigureLocationCommand( Services.Location, container, container.Logger() ) ) {}

		protected ServiceLocatorFactory( [Required]IUnityContainer container, [Required]ICommand<IServiceLocator> created )
		{
			this.container = container;
			this.created = created;
		}

		protected override IServiceLocator CreateItem()
		{
			var result = new ServiceLocator( container );
			created.ExecuteWith( result );
			return result;
		}
	}
}
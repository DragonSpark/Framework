using DragonSpark.Activation.FactoryModel;
using DragonSpark.Aspects;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Properties;
using DragonSpark.Runtime;
using DragonSpark.Setup;
using DragonSpark.TypeSystem;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using PostSharp.Patterns.Contracts;
using System;

namespace DragonSpark.Activation.IoC
{
	public class UnityContainerFactory<TAssemblyProvider, TLogger> : UnityContainerFactory
		where TAssemblyProvider : IAssemblyProvider
		where TLogger : class, IMessageLogger
	{
		public static UnityContainerFactory<TAssemblyProvider, TLogger> Instance { get; } = new UnityContainerFactory<TAssemblyProvider, TLogger>();

		protected UnityContainerFactory() : this( Activation.Activator.Activate<TAssemblyProvider>() ) {}

		UnityContainerFactory( [Required]TAssemblyProvider provider ) : base( provider.Create, MessageLoggerFactory<TLogger>.Instance.Create ) {}
	}

	/*public class UnityContainerFactoryFactory : FactoryBase<UnityContainerFactory>
	{
		protected override UnityContainerFactory CreateItem()
		{
			var result = new UnityContainerFactory( Assemblies.GetCurrent(), MessageLoggerFactory<RecordingMessageLogger>.Instance.Create() );
			return result;
		}
	}*/

	public class UnityContainerFactory : FactoryBase<IUnityContainer>
	{
		readonly Assemblies.Get assemblies;
		readonly Func<IMessageLogger> logger;

		public UnityContainerFactory( [Required]Assemblies.Get assemblies, [Required]Func<IMessageLogger> logger )
		{
			this.assemblies = assemblies;
			this.logger = logger;
		}

		protected override IUnityContainer CreateItem()
		{
			var result = new UnityContainer()
				.Extend<RegistrationMonitorExtension>()
				.RegisterInstance( assemblies() )
				.RegisterInstance( logger() )
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

	public class AssignLocationCommand : Command<IServiceLocator>
	{
		readonly IServiceLocation location;

		public AssignLocationCommand( IServiceLocation location )
		{
			this.location = location;
		}

		protected override void OnExecute( IServiceLocator parameter ) => location.Assign( parameter );
	}

	public class AssignLocationCommandFactory : ConfigureLocationCommandFactory
	{
		public new static AssignLocationCommandFactory Instance { get; } = new AssignLocationCommandFactory();

		readonly IServiceLocation location;

		public AssignLocationCommandFactory() : this( Services.Location ) {}

		public AssignLocationCommandFactory( IServiceLocation location ) : base( location )
		{
			this.location = location;
		}

		protected override ICommand<IServiceLocator> CreateItem( IUnityContainer parameter ) => new CompositeCommand<IServiceLocator>( new AssignLocationCommand( location ), base.CreateItem( parameter ) );
	}

	/*public class ServiceLocatorFactoryFactory : FactoryBase<ServiceLocatorFactory>
	{
		readonly UnityContainerFactory factory;

		public ServiceLocatorFactoryFactory( [Required]UnityContainerFactory factory )
		{
			this.factory = factory;
		}

		protected override ServiceLocatorFactory CreateItem()
		{
			var result = new ServiceLocatorFactory( factory.Create );
			return result;
		}
	}*/

	public class ConfigureLocationCommandFactory : FactoryBase<IUnityContainer, ICommand<IServiceLocator>>
	{
		public static ConfigureLocationCommandFactory Instance { get; } = new ConfigureLocationCommandFactory();

		readonly IServiceLocation location;

		public ConfigureLocationCommandFactory() : this( Services.Location ) { }

		public ConfigureLocationCommandFactory( [Required]IServiceLocation location )
		{
			this.location = location;
		}

		protected override ICommand<IServiceLocator> CreateItem( IUnityContainer parameter ) => new ConfigureLocationCommand( location, parameter, parameter.Logger() );
	}

	public class ServiceLocatorFactory : FactoryBase<IServiceLocator>
	{
		readonly Func<IUnityContainer> containerFactory;
		readonly Func<IUnityContainer, ICommand<IServiceLocator>> commandFactory;

		// public ServiceLocatorFactory() : this( Factory.Create<UnityContainer>() ) {}

		public ServiceLocatorFactory( [Required]Func<IUnityContainer> containerFactory ) : this( containerFactory, AssignLocationCommandFactory.Instance.Create ) {}

		public ServiceLocatorFactory( [Required]Func<IUnityContainer> containerFactory, [Required]Func<IUnityContainer, ICommand<IServiceLocator>> commandFactory )
		{
			this.containerFactory = containerFactory;
			this.commandFactory = commandFactory;
		}

		protected override IServiceLocator CreateItem()
		{
			var container = containerFactory();
			var result = new ServiceLocator( container );
			commandFactory( container ).ExecuteWith( result );
			return result;
		}
	}
}
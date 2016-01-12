using DragonSpark.Activation.FactoryModel;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Setup;
using DragonSpark.TypeSystem;
using Microsoft.Practices.Unity;
using PostSharp.Patterns.Contracts;
using System.Reflection;
using DragonSpark.Properties;
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

		public ServiceLocatorFactory( IUnityContainer container ) : base( container ) { }
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
				.Extend<BuildPipelineExtension>()
				.Extend<ObjectBuilderExtension>();
			return result;
		}
	}

	public class ServiceLocatorFactory : FactoryBase<ServiceLocator>
	{
		readonly IServiceLocation location;
		readonly IUnityContainer container;
		readonly IMessageLogger logger;

		public ServiceLocatorFactory() : this( Factory.Create<UnityContainer>() ) {}

		public ServiceLocatorFactory( IUnityContainer container ) : this( Services.Location, container ) {}

		public ServiceLocatorFactory( IServiceLocation location, [Required]IUnityContainer container ) : this( location, container, container.Logger() ) {}

		public ServiceLocatorFactory( [Required]IServiceLocation location, [Required]IUnityContainer container, [Required]IMessageLogger logger )
		{
			this.location = location;
			this.container = container;
			this.logger = logger;
		}

		protected override ServiceLocator CreateItem()
		{
			logger.Information( Resources.ConfiguringServiceLocatorSingleton, Priority.Low );
			var result = new ServiceLocator( container );
			container.RegisterInstance( location );
			container.RegisterInstance<IServiceLocator>( result );
			location.Assign( result );
			return result;
		}
	}
}
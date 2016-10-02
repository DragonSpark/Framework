using DragonSpark.Configuration;
using DragonSpark.Runtime;
using System;
using System.Composition.Hosting;

namespace DragonSpark.Composition
{
	public sealed class CompositionHostFactory : ConfigurableFactoryBase<ContainerConfiguration, CompositionHost>
	{
		readonly IComposable<IDisposable> disposables;
		readonly static IConfigurationScope<ContainerConfiguration> DefaultConfiguration = new ConfigurationScope<ContainerConfiguration>( ContainerServicesConfigurator.Default, PartsContainerConfigurator.Default );

		public static CompositionHostFactory Default { get; } = new CompositionHostFactory();

		CompositionHostFactory() : this( Disposables.Default ) {}

		CompositionHostFactory( IComposable<IDisposable> disposables ) : base( () => new ContainerConfiguration(), DefaultConfiguration, parameter => parameter.CreateContainer() )
		{
			this.disposables = disposables;
		}

		public override CompositionHost Get()
		{
			var result = base.Get();
			disposables.Add( result );
			return result;
		}
	}
}
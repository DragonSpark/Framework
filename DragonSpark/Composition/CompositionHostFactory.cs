using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Sources;
using DragonSpark.Sources.Coercion;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Scopes;
using JetBrains.Annotations;
using System;
using System.Composition.Hosting;

namespace DragonSpark.Composition
{
	public sealed class CompositionHostFactory : DelegatedSource<CompositionHost>
	{
		readonly static Func<CompositionHost> Source = Configuration.Implementation.Then( configuration => configuration.CreateContainer() ).Get;

		public static CompositionHostFactory Default { get; } = new CompositionHostFactory();
		CompositionHostFactory() : this( Disposables.Default, Source ) {}

		readonly IComposable<IDisposable> disposables;

		[UsedImplicitly]
		public CompositionHostFactory( IComposable<IDisposable> disposables, Func<CompositionHost> source ) : base( source )
		{
			this.disposables = disposables;
		}

		public override CompositionHost Get() => disposables.Registered( base.Get() );

		public sealed class Configuration : Scope<ContainerConfiguration>
		{
			readonly static IAlteration<ContainerConfiguration>[] DefaultAlterations = { ContainerServicesConfigurator.Default, PartsContainerConfigurator.Default };
			public static Configuration Implementation { get; } = new Configuration();
			Configuration() : base( () => new AggregateSource<ContainerConfiguration>( DefaultAlterations.IncludeExports().Fixed() ).Get() ) {}
		}
	}
}
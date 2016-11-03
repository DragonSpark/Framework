using DragonSpark.Runtime;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Scopes;
using JetBrains.Annotations;
using System;
using System.Composition.Hosting;

namespace DragonSpark.Composition
{
	public sealed class CompositionHostFactory : DelegatedSource<CompositionHost>
	{
		readonly static Func<CompositionHost> Source = Configuration.Implementation.Coerce( configuration => configuration.CreateContainer() ).Get;

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
			public static Configuration Implementation { get; } = new Configuration();
			Configuration() : base( () => new AggregateSource<ContainerConfiguration>( Seed.Implementation.Get, Alterations.Implementation.Unwrap() ).Get() ) {}
		}

		public sealed class Seed : Scope<ContainerConfiguration>
		{
			public static Seed Implementation { get; } = new Seed();
			Seed() : base( () => new ContainerConfiguration() ) {}
		}

		// ReSharper disable once PossibleInfiniteInheritance
		public sealed class Alterations : SingletonScope<IAlterations<ContainerConfiguration>>
		{
			public static Alterations Implementation { get; } = new Alterations();
			Alterations() : base( () => new Alterations<ContainerConfiguration>( ContainerServicesConfigurator.Default, PartsContainerConfigurator.Default ) ) {}
		}
	}
}
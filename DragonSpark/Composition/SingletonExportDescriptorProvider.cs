using DragonSpark.Sources;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition.Hosting.Core;
using CompositeActivator = System.Composition.Hosting.Core.CompositeActivator;

namespace DragonSpark.Composition
{
	public sealed class SingletonExportDescriptorProvider : ExportDescriptorProvider
	{
		public static SingletonExportDescriptorProvider Default { get; } = new SingletonExportDescriptorProvider();
		SingletonExportDescriptorProvider() : this( Application.SingletonExports.Default.Get ) {}

		readonly Func<ImmutableArray<SingletonExport>> singletons;

		public SingletonExportDescriptorProvider( Func<ImmutableArray<SingletonExport>> singletons )
		{
			this.singletons = singletons;
		}

		public override IEnumerable<ExportDescriptorPromise> GetExportDescriptors( CompositionContract contract, DependencyAccessor descriptorAccessor )
		{
			foreach ( var singleton in singletons() )
			{
				if ( singleton.Contracts.Contains( contract ) )
				{
					yield return new ExportDescriptorPromise( contract, GetType().FullName, true, NoDependencies, new Factory( singleton.Factory ).Create );
				}
			}
		}

		sealed class Factory : DelegatedSource<object>
		{
			readonly CompositeActivator activate;

			public Factory( Func<object> provider ) : base( provider )
			{
				activate = Activate;
			}

			object Activate( LifetimeContext context, CompositionOperation operation ) => Get();

			public ExportDescriptor Create( IEnumerable<CompositionDependency> dependencies ) => ExportDescriptor.Create( activate, NoMetadata );
		}
	}
}
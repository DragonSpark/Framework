using DragonSpark.Activation.Location;
using DragonSpark.Application.Setup;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using System;
using System.Collections.Generic;
using System.Composition.Hosting.Core;

namespace DragonSpark.Composition
{
	public sealed class ServicesExportDescriptorProvider : ExportDescriptorProvider
	{
		public static ServicesExportDescriptorProvider Default { get; } = new ServicesExportDescriptorProvider();
		ServicesExportDescriptorProvider() : this( DefaultServices.Default ) {}

		readonly Func<Type, object> provider;

		public ServicesExportDescriptorProvider( IServiceProvider provider ) : this( new ActivatedServiceSource( provider ).Get ) {}

		ServicesExportDescriptorProvider( Func<Type, object> provider )
		{
			this.provider = provider;
		}

		public override IEnumerable<ExportDescriptorPromise> GetExportDescriptors( CompositionContract contract, DependencyAccessor descriptorAccessor )
		{
			CompositionDependency dependency;
			if ( !descriptorAccessor.TryResolveOptionalDependency( "Existing Request", contract, true, out dependency ) )
			{
				yield return new ExportDescriptorPromise( contract, GetType().FullName, true, NoDependencies, new Factory( provider, contract.ContractType ).Create );
			}
		}

		sealed class Factory : DelegatedSource<object>
		{
			readonly CompositeActivator activate;

			public Factory( Func<Type, object> provider, Type contract ) : base( provider.Fixed( contract ).ToCachedDelegate() )
			{
				activate = Activate;
			}

			object Activate( LifetimeContext context, CompositionOperation operation ) => Get();

			public ExportDescriptor Create( IEnumerable<CompositionDependency> dependencies ) => ExportDescriptor.Create( activate, NoMetadata );
		}
	}
}
using System.Collections.Generic;
using System.Composition.Hosting.Core;
using DragonSpark.Sources.Parameterized;

namespace DragonSpark.Composition
{
	public abstract class ExportDescriptorProviderBase : ExportDescriptorProvider
	{
		protected abstract class FactoryBase : ParameterizedSourceBase<IEnumerable<CompositionDependency>, ExportDescriptor>
		{
			readonly CompositeActivator create;

			protected FactoryBase()
			{
				create = Create;
			}

			public override ExportDescriptor Get( IEnumerable<CompositionDependency> parameter ) => ExportDescriptor.Create( create, NoMetadata );

			protected abstract object Create( LifetimeContext context, CompositionOperation operation );
		}
	}
}
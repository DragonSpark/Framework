using DragonSpark.Application;
using DragonSpark.Extensions;
using System.Collections.Immutable;
using System.Composition;

namespace DragonSpark.Composition
{
	sealed class ExportProvider : IExportProvider
	{
		readonly CompositionContext context;

		public ExportProvider( CompositionContext context )
		{
			this.context = context;
		}

		public ImmutableArray<T> GetExports<T>( string name = null ) => context.GetExports<T>( name ).WhereAssigned().Prioritize().ToImmutableArray();
	}
}
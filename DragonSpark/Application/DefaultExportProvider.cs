using DragonSpark.Composition;
using DragonSpark.TypeSystem;
using JetBrains.Annotations;
using System;
using System.Collections.Immutable;

namespace DragonSpark.Application
{
	sealed class DefaultExportProvider : IExportProvider
	{
		public static DefaultExportProvider Default { get; } = new DefaultExportProvider();
		DefaultExportProvider() : this( ApplicationPartsAssignedSpecification.Default.IsSatisfiedBy ) {}

		readonly Func<object, bool> specification;

		[UsedImplicitly]
		public DefaultExportProvider( Func<object, bool> specification )
		{
			this.specification = specification;
		}

		public ImmutableArray<T> GetExports<T>( string name = null ) => specification( name ) ? SingletonExportSource<T>.Default.Get() : Items<T>.Immutable;
	}
}
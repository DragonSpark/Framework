using DragonSpark.Sources;
using DragonSpark.Specifications;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DragonSpark.Composition
{
	sealed class SingletonExportSource<T> : ItemSourceBase<T>
	{
		public static SingletonExportSource<T> Default { get; } = new SingletonExportSource<T>();
		SingletonExportSource() : this( Application.SingletonExports.Default.Get ) {}

		readonly Func<ImmutableArray<SingletonExport>> exports;
		readonly Func<Type, bool> specification;

		public SingletonExportSource( Func<ImmutableArray<SingletonExport>> exports ) : this( TypeAssignableSpecification<T>.Default.ToSpecificationDelegate() )
		{
			this.exports = exports;
		}

		SingletonExportSource( Func<Type, bool> specification )
		{
			this.specification = specification;
		}

		protected override IEnumerable<T> Yield()
		{
			foreach ( var export in exports() )
			{
				if ( export.Contracts.Select( contract => contract.ContractType ).Any( specification ) )
				{
					var item = export.Factory();
					if ( item is T )
					{
						yield return (T)item;
					}
				}
			}
		}
	}
}
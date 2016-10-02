using DragonSpark.Application;
using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using DragonSpark.TypeSystem;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Windows.Runtime
{
	public abstract class ApplicationTypesBase : SuppliedTypeSource
	{
		readonly Func<ImmutableArray<Assembly>> assemblySource;
		readonly Alter<IEnumerable<Assembly>> filter;
		readonly Func<IEnumerable<Assembly>, IEnumerable<Type>> partsSource;

		protected ApplicationTypesBase( Func<ImmutableArray<Assembly>> assemblySource ) : this( assemblySource, ApplicationAssemblyFilter.Default.Get, PublicParts.Default.Get ) {}

		protected ApplicationTypesBase( Func<ImmutableArray<Assembly>> assemblySource, Alter<IEnumerable<Assembly>> filter, Func<IEnumerable<Assembly>, IEnumerable<Type>> partsSource )
		{
			this.assemblySource = assemblySource;
			this.filter = filter;
			this.partsSource = partsSource;
		}

		protected override IEnumerable<Type> Yield()
		{
			var filtered = filter( assemblySource().AsEnumerable() ).Fixed();
			var result = new AssemblyBasedTypeSource( filtered ).Union( partsSource( filtered ) );
			return result;
		}
	}
}
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using DragonSpark.Sources.Parameterized.Caching;

namespace DragonSpark.TypeSystem
{
	public abstract class PartsBase : FactoryCache<Assembly, ImmutableArray<Type>>
	{
		readonly Func<Assembly, IEnumerable<Type>> source;
		readonly Func<Assembly, ImmutableArray<Assembly>> locator;

		protected PartsBase( Func<Assembly, IEnumerable<Type>> source ) : this( source, AssemblyPartLocator.Default.Get ) {}

		protected PartsBase( Func<Assembly, IEnumerable<Type>> source, Func<Assembly, ImmutableArray<Assembly>> locator )
		{
			this.source = source;
			this.locator = locator;
		}

		protected override ImmutableArray<Type> Create( Assembly parameter ) => 
			locator( parameter ).Select( source ).Concat().ToImmutableArray();
	}
}
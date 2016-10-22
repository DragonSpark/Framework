using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace DragonSpark.TypeSystem
{
	public abstract class PartsBase : FactoryCache<Assembly, ImmutableArray<Type>>
	{
		readonly Func<Assembly, IEnumerable<Type>> source;
		readonly Func<Assembly, IEnumerable<Assembly>> locator;

		protected PartsBase( Func<Assembly, IEnumerable<Type>> source ) : this( source, AssemblyPartLocator.Default.AsEnumerable ) {}

		protected PartsBase( Func<Assembly, IEnumerable<Type>> source, Func<Assembly, IEnumerable<Assembly>> locator )
		{
			this.source = source;
			this.locator = locator;
		}

		protected override ImmutableArray<Type> Create( Assembly parameter ) => 
			locator( parameter ).Select( source ).Concat().ToImmutableArray();
	}
}
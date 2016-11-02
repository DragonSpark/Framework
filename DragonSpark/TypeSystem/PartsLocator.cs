using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace DragonSpark.TypeSystem
{
	public class PartsLocator : CacheWithImplementedFactoryBase<Assembly, ImmutableArray<Type>>
	{
		readonly Func<Assembly, IEnumerable<Type>> source;
		readonly Func<Assembly, IEnumerable<Assembly>> locator;

		public PartsLocator( Func<Assembly, IEnumerable<Type>> source ) : this( source, AssemblyPartLocator.Default.GetEnumerable ) {}

		[UsedImplicitly]
		protected PartsLocator( Func<Assembly, IEnumerable<Type>> source, Func<Assembly, IEnumerable<Assembly>> locator )
		{
			this.source = source;
			this.locator = locator;
		}

		protected override ImmutableArray<Type> Create( Assembly parameter ) => 
			locator( parameter ).Select( source ).Concat().ToImmutableArray();
	}
}
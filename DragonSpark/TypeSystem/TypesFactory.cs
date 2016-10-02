using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using DragonSpark.Sources.Parameterized.Caching;

namespace DragonSpark.TypeSystem
{
	public sealed class TypesFactory : ArgumentCache<ImmutableArray<Assembly>, ImmutableArray<Type>>
	{
		readonly static Func<Assembly, IEnumerable<Type>> All = AssemblyTypes.All.ToDelegate();

		public static TypesFactory Default { get; } = new TypesFactory();
		TypesFactory() : base( array => array.ToArray().SelectMany( All ).ToImmutableArray() ) {}
	}
}
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace DragonSpark.TypeSystem
{
	public sealed class TypesFactory : StructuralCache<ImmutableArray<Assembly>, ImmutableArray<Type>>
	{
		readonly static Func<Assembly, IEnumerable<Type>> All = AssemblyTypes.All.GetEnumerable;

		public static TypesFactory Default { get; } = new TypesFactory();
		TypesFactory() : base( array => array.ToArray().SelectMany( All ).ToImmutableArray() ) {}
	}
}
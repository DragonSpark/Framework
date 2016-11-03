using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Scopes;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;

namespace DragonSpark.TypeSystem
{
	public sealed class AllPartsLocator : ParameterizedSingletonScope<Assembly, ImmutableArray<Type>>
	{
		readonly static Func<Assembly, IEnumerable<Type>> Factory = AssemblyTypes.All.GetEnumerable;

		public static AllPartsLocator Default { get; } = new AllPartsLocator();
		AllPartsLocator() : base( o => new PartsLocator( Factory ).Get ) {}
	}
}
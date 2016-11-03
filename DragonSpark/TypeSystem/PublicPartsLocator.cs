using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Scopes;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;

namespace DragonSpark.TypeSystem
{
	public sealed class PublicPartsLocator : ParameterizedSingletonScope<Assembly, ImmutableArray<Type>>
	{
		readonly static Func<Assembly, IEnumerable<Type>> Factory = AssemblyTypes.Public.GetEnumerable;

		public static PublicPartsLocator Default { get; } = new PublicPartsLocator();
		PublicPartsLocator() : base( o => new PartsLocator( Factory ).Get ) {}
	}
}
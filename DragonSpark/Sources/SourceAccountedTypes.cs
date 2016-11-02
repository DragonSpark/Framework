using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.TypeSystem;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace DragonSpark.Sources
{
	public sealed class SourceAccountedTypes : CacheWithImplementedFactoryBase<Type, ImmutableArray<Type>>
	{
		public static SourceAccountedTypes Default { get; } = new SourceAccountedTypes();
		SourceAccountedTypes() {}

		protected override ImmutableArray<Type> Create( Type parameter ) => Candidates( parameter ).ToImmutableArray();

		static IEnumerable<Type> Candidates( Type type )
		{
			yield return type;
			foreach ( var implementation in type.Adapt().GetImplementations( typeof(ISource<>) ) )
			{
				yield return implementation.Adapt().GetInnerType();
			}
		}
	}
}
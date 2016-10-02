using System;
using System.Collections.Generic;
using System.Linq;
using DragonSpark.Sources.Parameterized.Caching;

namespace DragonSpark.Sources.Parameterized
{
	public sealed class ResultTypes : TypeLocatorBase
	{
		public static ICache<Type, Type> Default { get; } = new ResultTypes();
		ResultTypes() : base( typeof(IParameterizedSource<,>), typeof(ISource<>), typeof(Func<>), typeof(Func<,>) ) {}

		protected override Type From( IEnumerable<Type> genericTypeArguments ) => genericTypeArguments.Last();
	}
}
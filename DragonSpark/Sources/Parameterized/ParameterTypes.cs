using System;
using System.Collections.Generic;
using System.Linq;
using DragonSpark.Commands;
using DragonSpark.Sources.Parameterized.Caching;

namespace DragonSpark.Sources.Parameterized
{
	public sealed class ParameterTypes : TypeLocatorBase
	{
		public static ICache<Type, Type> Default { get; } = new ParameterTypes();
		ParameterTypes() : base( typeof(Func<,>), typeof(IParameterizedSource<,>), typeof(ICommand<>) ) {}

		protected override Type From( IEnumerable<Type> genericTypeArguments ) => genericTypeArguments.First();
	}
}
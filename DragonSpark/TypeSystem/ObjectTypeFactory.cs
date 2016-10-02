using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace DragonSpark.TypeSystem
{
	sealed class ObjectTypeFactory : ParameterizedSourceBase<IEnumerable<object>, ImmutableArray<Type>>
	{
		public static ObjectTypeFactory Default { get; } = new ObjectTypeFactory();
		ObjectTypeFactory() {}

		public override ImmutableArray<Type> Get( IEnumerable<object> parameter )
		{
			var items = parameter.Fixed();
			var length = items.Length;
			var types = new Type[length];
			for ( var i = 0; i < length; i++ )
			{
				types[i] = items[i]?.GetType();
			}
			var result = types.ToImmutableArray();
			return result;
		}
	}
}
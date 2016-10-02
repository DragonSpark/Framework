using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using DragonSpark.Extensions;


namespace DragonSpark.TypeSystem
{
	class MethodMapper // : ParameterizedSourceBase<Type, ImmutableArray<MethodMapping>>
	{
		readonly TypeAdapter adapter;

		public MethodMapper( TypeAdapter adapter )
		{
			this.adapter = adapter;
		}

		public ImmutableArray<MethodMapping> Get( Type parameter )
		{
			var generic = parameter.GetTypeInfo().IsGenericTypeDefinition ? adapter.GetImplementations( parameter ).FirstOrDefault() : null;
			var implementation = generic ?? ( parameter.Adapt().IsAssignableFrom( adapter.ReferenceType ) ? parameter : null );
			if ( implementation != null )
			{
				var map = adapter.Info.GetRuntimeInterfaceMap( implementation );
				var result = map.InterfaceMethods.Tuple( map.TargetMethods ).Select( tuple => new MethodMapping( tuple.Item1, tuple.Item2 ) ).ToImmutableArray();
				return result;
			}
			return Items<MethodMapping>.Immutable;
		}
	}
}
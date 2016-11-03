using DragonSpark.Application;
using DragonSpark.Sources.Scopes;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace DragonSpark.TypeSystem
{
	public class KnownTypes : ParameterizedSingletonScope<Type, ImmutableArray<Type>>
	{
		public static KnownTypes Default { get; } = new KnownTypes();
		KnownTypes() : base( type => ApplicationTypes.Default.Get().Where( type.Adapt().IsAssignableFrom ).ToImmutableArray() ) {}

		public ImmutableArray<Type> Get<T>() => Get( typeof(T) );
	}
}
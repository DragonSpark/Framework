using DragonSpark.Application;
using DragonSpark.Sources.Scopes;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace DragonSpark.TypeSystem
{
	public class KnownTypes : ParameterizedScope<Type, ImmutableArray<Type>>
	{
		public static KnownTypes Default { get; } = new KnownTypes();
		KnownTypes() : base( Factory.Singleton<Type, ImmutableArray<Type>>( type => ApplicationTypes.Default.Get().Where( type.Adapt().IsAssignableFrom ).ToImmutableArray() ) ) {}

		public ImmutableArray<Type> Get<T>() => Get( typeof(T) );
	}
}
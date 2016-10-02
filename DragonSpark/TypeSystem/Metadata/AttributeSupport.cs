using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using System;
using System.Reflection;

namespace DragonSpark.TypeSystem.Metadata
{
	static class AttributeSupport<T> where T : Attribute
	{
		public static IAttributeSource Local { get; } = new Cache( type => type.GetTypeInfo().GetCustomAttribute<T>() );
		public static IAttributeSource All { get; } = new Cache( type => type.GetTypeInfo().GetCustomAttribute<T>( true ) );

		public interface IAttributeSource : IParameterizedSource<Type, T>
		{
			bool Contains( Type instance );
		}
		sealed class Cache : Cache<Type, T>, IAttributeSource
		{
			public Cache( Func<Type, T> create ) : base( create ) {}

			public override bool Contains( Type instance ) => Get( instance ) != null;
		}
	}
}
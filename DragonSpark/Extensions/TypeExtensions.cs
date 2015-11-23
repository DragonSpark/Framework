using DragonSpark.Activation;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace DragonSpark.Extensions
{
	public static class TypeExtensions
	{
		static readonly ConcurrentDictionary<Type, TypeSupport> Extensions = new ConcurrentDictionary<Type, TypeSupport>();

		public static TypeSupport Extend( this Type @this )
		{
			return Extensions.GetOrAdd( @this, type => new TypeSupport( type ) );
		}

		public static TypeSupport Extend( this TypeInfo @this )
		{
			return Extend( @this.AsType() );
		}

		public static Assembly Assembly( this Type @this )
		{
			return Extend( @this ).Assembly;
		}

		/*
		static readonly IDictionary<Type,Type[]> KnownTypeCache = new Dictionary<Type, Type[]>();
		public static Type[] GetKnownTypes( this Type target )
		{
			var result = KnownTypeCache.Ensure( target, x => Services.With<IAssembliesProvider, Type[]>( y => y.GetAssemblies().SelectMany( z => z.DefinedTypes ).Where( z => z.IsSubclassOf( x ) && x.Namespace != "System.Data.Entity.DynamicProxies" ).ToArray() ) );
			return result;
		}*/
	}
}
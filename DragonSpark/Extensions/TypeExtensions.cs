using DragonSpark.TypeSystem;
using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace DragonSpark.Extensions
{
	public static class TypeExtensions
	{
		readonly static ConcurrentDictionary<Type, TypeAdapter> Extensions = new ConcurrentDictionary<Type, TypeAdapter>();

		public static TypeAdapter Adapt( this object @this )
		{
			return @this.GetType().Adapt();
		}

		public static TypeAdapter Adapt( this Type @this )
		{
			return @this.With( item => Extensions.GetOrAdd( item, t => new TypeAdapter( t ) ) );
		}

		public static TypeAdapter Adapt( this TypeInfo @this )
		{
			return @this.With( item => Extensions.GetOrAdd( item.AsType(), t => new TypeAdapter( @this ) ) );
		}

		public static Assembly Assembly( this Type @this )
		{
			return Adapt( @this ).Assembly;
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
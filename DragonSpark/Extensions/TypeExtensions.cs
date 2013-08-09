using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Extensions
{
	public static class TypeExtensions
	{
		readonly static IDictionary<Type,object> Cache = new Dictionary<Type, object>();
		static readonly IDictionary<Type,Type[]> KnownTypeCache = new Dictionary<Type, Type[]>();

		public static object GetDefaultValue( this Type target )
		{
			var result = Cache.Ensure( target, x => x.IsValueType ? Activator.CreateInstance( x ) : null );
			return result;
		}

		public static Type MakeGeneric( this Type target, params Type[] types )
		{
			var result = target.MakeGenericType( types );
			return result;
		}

		public static IEnumerable<Type> GetHierarchy( this Type target, bool includeRoot = true )
		{
			var result = new List<Type> { target };
			var current = target.BaseType;
			while ( current != null )
			{
				if ( current != typeof(object) || includeRoot )
				{
					result.Add( current );
				}
				current = current.BaseType;
			}
			return result;
		}


		public static Type GetCollectionElementType( this Type target )
		{
			var result = target.IsGenericType && typeof(IEnumerable).IsAssignableFrom( target ) ? target.GetGenericArguments().FirstOrDefault() : null;
			return result;
		}

		public static IEnumerable<Type> ResolveInterfaces( this Type target )
		{
			var result = target.ToEnumerable().Concat( target.GetInterfaces().SelectMany( ResolveInterfaces ) ).ToArray();
			return result;
		}

		public static Type[] GetKnownTypes( this Type target )
		{
			var result = KnownTypeCache.Ensure( target, x => AppDomain.CurrentDomain.GetAssemblies().SelectMany( y => y.GetTypes() ).Where( y => y.IsSubclassOf( x ) ).ToArray() );
			return result;
		}
	}
}
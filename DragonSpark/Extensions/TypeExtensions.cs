using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Extensions
{
	public static class TypeExtensions
	{
		readonly static IDictionary<Type,object> Cache = new Dictionary<Type, object>();

		public static object GetDefaultValue( this Type target )
		{
			var result = Cache.Ensure( target, x => x.GetTypeInfo().IsValueType ? Activator.CreateInstance( x ) : null );
			return result;
		}

		/*public static Type MakeGeneric( this Type target, params Type[] types )
		{
			var result = target.MakeGenericType( types );
			return result;
		}*/

		public static IEnumerable<Type> GetHierarchy( this Type target, bool includeRoot = true )
		{
			var result = new List<Type> { target };
			var current = target.GetTypeInfo().BaseType;
			while ( current != null )
			{
				if ( current != typeof(object) || includeRoot )
				{
					result.Add( current );
				}
				current = current.GetTypeInfo().BaseType;
			}
			return result;
		}


		public static Type GetItemType( this Type target )
		{
			var info = target.GetTypeInfo();
			var result = info.IsGenericType && /*typeof(IEnumerable).GetTypeInfo().IsAssignableFrom( info )*/ info.GenericTypeArguments.Any() ? info.GenericTypeArguments.FirstOrDefault() : 
				target.IsArray ? target.GetElementType() : null;
			return result;
		}

		public static IEnumerable<Type> GetAllInterfaces( this Type target )
		{
			var result = target.AsItem().Concat( target.GetTypeInfo().ImplementedInterfaces.SelectMany( GetAllInterfaces ) ).Where( x => x.GetTypeInfo().IsInterface ).Distinct().ToArray();
			return result;
		}

		/*
		static readonly IDictionary<Type,Type[]> KnownTypeCache = new Dictionary<Type, Type[]>();
		public static Type[] GetKnownTypes( this Type target )
		{
			var result = KnownTypeCache.Ensure( target, x => ServiceLocation.With<IAssembliesProvider, Type[]>( y => y.GetAssemblies().SelectMany( z => z.DefinedTypes ).Where( z => z.IsSubclassOf( x ) && x.Namespace != "System.Data.Entity.DynamicProxies" ).ToArray() ) );
			return result;
		}*/
	}
}
using System;
using System.Collections;
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

		public static bool IsAssignableFrom( this Type @this, Type other )
		{
			var result = @this.GetTypeInfo().IsAssignableFrom( other.GetTypeInfo() );
			return result;
		}

		public static Assembly Assembly( this Type @this )
		{
			var result = @this.GetTypeInfo().Assembly;
			return result;
		}

		public static bool CanActivateFrom<T>( this Type @this )
		{
			var result = @this.CanActivateFrom( typeof(T) );
			return result;
		}

		public static bool CanActivateFrom( this Type @this, Type other )
		{
			var result  = !@this.GetTypeInfo().IsAbstract && other.IsAssignableFrom( @this );
			return result;
		}

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

		public static Type GetEnumerableType( this Type @this )
		{
			var result = InnerType( @this, info => typeof(IEnumerable).GetTypeInfo().IsAssignableFrom( info ) );
			return result;
		}

		public static Type GetInnerType( this Type target )
		{
			return InnerType( target );
		}

		static Type InnerType( Type target, Func<TypeInfo, bool> check = null )
		{
			var info = target.GetTypeInfo();
			var result = info.IsGenericType && info.GenericTypeArguments.Any() && check.Transform( func => func( info ), () => true ) ? info.GenericTypeArguments.FirstOrDefault() :
				target.IsArray ? target.GetElementType() : null;
			return result;
		}

		public static IEnumerable<Type> GetAllInterfaces( this Type target )
		{
			var result = target.Append().Concat( target.GetTypeInfo().ImplementedInterfaces.SelectMany( GetAllInterfaces ) ).Where( x => x.GetTypeInfo().IsInterface ).Distinct().ToArray();
			return result;
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
using DragonSpark.Activation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Extensions
{
	public class DefaultValueFactory : FactoryBase<Type, object>
	{
		static readonly IDictionary<Type,object> Cache = new Dictionary<Type, object>();

		public static DefaultValueFactory Instance { get; } = new DefaultValueFactory();

		protected override object CreateFrom( Type resultType, Type parameter )
		{
			var result = Cache.Ensure( parameter, x => x.GetTypeInfo().IsValueType ? SystemActivator.Instance.Activate( x ) : null );
			return result;
		}
	}

	public static class TypeExtensions
	{
		public static object GetDefaultValue( this Type @this )
		{
			var result = DefaultValueFactory.Instance.Create( @this );
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

		public static bool IsSubclass( this Type @this, Type other )
		{
			var result = @this.GetTypeInfo().IsSubclassOf( other );
			return result;
		}

		public static Assembly Assembly( this Type @this )
		{
			var result = @this.GetTypeInfo().Assembly;
			return result;
		}

		public static bool CanActivate<T>( this Type @this )
		{
			var result = typeof(T).CanActivate( @this );
			return result;
		}

		public static bool CanActivate( this Type @this, Type instanceType )
		{
		    var result  = instanceType.CanActivate() && @this.IsAssignableFrom( instanceType );
			return result;
		}

		public static bool CanActivate( this Type @this )
		{
		    var info = @this.GetTypeInfo();
			var result = info.IsInterface || !info.IsAbstract;
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
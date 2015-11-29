using DragonSpark.Runtime;
using System;
using System.Reflection;

namespace DragonSpark.Extensions
{
	public static class TypeExtensions
	{
		public static TypeExtension Extend( this object @this )
		{
			return @this.GetType();
		}

		public static TypeExtension Extend( this Type @this )
		{
			return @this;
		}

		public static TypeExtension Extend( this TypeInfo @this )
		{
			return @this.AsType();
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
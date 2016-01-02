using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Extensions
{
	public static class AssemblyLocatorExtensions
	{
		public static Tuple<TAttribute, TypeInfo>[] GetAllTypesWith<TAttribute>( this IEnumerable<Assembly> target ) where TAttribute : Attribute
		{
			var result = target.SelectMany( assembly => assembly.DefinedTypes ).WhereDecorated<TAttribute>();
			return result;
		}

		public static Tuple<TAttribute, TypeInfo>[] WhereDecorated<TAttribute>( this IEnumerable<TypeInfo> target, bool inherit = false ) where TAttribute : Attribute
		{
			var result = target.Where( info => info.IsDecoratedWith<TAttribute>( inherit ) ).Select( info => new Tuple<TAttribute, TypeInfo>( info.GetAttribute<TAttribute>(), info ) ).ToArray();
			return result;
		}

		public static IEnumerable<Type> AsTypes( this IEnumerable<TypeInfo> target )
		{
			return target.Select( info => info.AsType() );
		}

		public static IEnumerable<TypeInfo> AsTypeInfos( this IEnumerable<Type> target )
		{
			return target.Select( info => info.GetTypeInfo() );
		}
	}
}
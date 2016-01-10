using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DragonSpark.TypeSystem;

namespace DragonSpark.Extensions
{
	public static class AssemblyLocatorExtensions
	{
		public static Tuple<TAttribute, TypeInfo>[] GetAllTypesWith<TAttribute>( this IEnumerable<Assembly> target, bool inherit = false ) where TAttribute : Attribute
			=> target.SelectMany( assembly => assembly.DefinedTypes ).WhereDecorated<TAttribute>( inherit );
		
		public static Tuple<TAttribute, TypeInfo>[] WhereDecorated<TAttribute>( this IEnumerable<TypeInfo> target, bool inherit = false ) where TAttribute : Attribute
			=> target.Where( info => Attributes.Get( info, inherit ).Has<TAttribute>() ).Select( info => new Tuple<TAttribute, TypeInfo>( Attributes.Get( info, inherit ).GetAttribute<TAttribute>(), info ) ).ToArray();

		public static IEnumerable<Type> AsTypes( this IEnumerable<TypeInfo> target ) => target.Select( info => info.AsType() );

		public static IEnumerable<TypeInfo> AsTypeInfos( this IEnumerable<Type> target ) => target.Select( info => info.GetTypeInfo() );
	}
}
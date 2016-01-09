using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DragonSpark.TypeSystem;

namespace DragonSpark.Extensions
{
	public static class AssemblyLocatorExtensions
	{
		public static Tuple<TAttribute, TypeInfo>[] GetAllTypesWith<TAttribute>( this IAttributeProvider @this, IEnumerable<Assembly> target, bool inherit = false ) where TAttribute : Attribute
			=> @this.WhereDecorated<TAttribute>( target.SelectMany( assembly => assembly.DefinedTypes ), inherit );
		
		public static Tuple<TAttribute, TypeInfo>[] WhereDecorated<TAttribute>( this IAttributeProvider provider, IEnumerable<TypeInfo> target, bool inherit = false ) where TAttribute : Attribute
			=> target.Where( info => provider.IsDecoratedWith<TAttribute>( info, inherit ) ).Select( info => new Tuple<TAttribute, TypeInfo>( provider.GetAttribute<TAttribute>( info ), info ) ).ToArray();

		public static IEnumerable<Type> AsTypes( this IEnumerable<TypeInfo> target ) => target.Select( info => info.AsType() );

		public static IEnumerable<TypeInfo> AsTypeInfos( this IEnumerable<Type> target ) => target.Select( info => info.GetTypeInfo() );
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Extensions
{
	public static class AssemblyLocatorExtensions
	{
		// static readonly Dictionary<Assembly, TypeInfo[]> TypeCache = new Dictionary<IAssemblyLocator, TypeInfo[]>();

		public static Tuple<TAttribute, TypeInfo>[] GetAllTypesWith<TAttribute>( this IEnumerable<Assembly> target ) where TAttribute : Attribute
		{
			var result = target.SelectMany( assembly => assembly.DefinedTypes ).WhereDecorated<TAttribute>();
			return result;
			/*var result = from type in TypeCache.Ensure( target, ResolveTypes )
			             let attribute = type.GetAttribute<TAttribute>()
			             where attribute != null
			             select new Tuple<TAttribute, TypeInfo>( attribute, type );
			return result;*/
		}

		public static Tuple<TAttribute, TypeInfo>[] WhereDecorated<TAttribute>( this IEnumerable<TypeInfo> target ) where TAttribute : Attribute
		{
			var result = target.Where( info => info.IsDecoratedWith<TAttribute>() ).Select( info => new Tuple<TAttribute, TypeInfo>( info.GetAttribute<TAttribute>(), info ) ).ToArray();
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

		/*static TypeInfo[] ResolveTypes( IAssemblyLocator target )
		{
			var query = from assembly in target.GetAll()
			            from type in assembly.DefinedTypes
			            select type;
			var result = query.ToArray();
			return result;
		}*/
	}
}
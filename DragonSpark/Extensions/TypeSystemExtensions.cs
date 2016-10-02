using DragonSpark.Application;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Type = System.Type;

namespace DragonSpark.Extensions
{
	public static class TypeSystemExtensions
	{
		public static IEnumerable<Type> Decorated<TAttribute>( this IEnumerable<Type> target ) where TAttribute : Attribute => target.Where( info => info.Has<TAttribute>() );

		public static IEnumerable<Type> AsTypes( this IEnumerable<TypeInfo> target ) => target.Select( info => info.AsType() );

		public static IEnumerable<TypeInfo> AsTypeInfos( this IEnumerable<Type> target ) => target.Select( info => info.GetTypeInfo() );

		public static ImmutableArray<Type> AsApplicationParts( this IEnumerable<Type> target ) => ApplicationParts.Assign( target.Fixed() );
	}
}
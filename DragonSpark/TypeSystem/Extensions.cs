using DragonSpark.Application;
using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace DragonSpark.TypeSystem
{
	public static class Extensions
	{
		readonly static Func<object, Type> CoerceType = TypeCoercer.Default.Get;

		readonly static TypeInfo Structural = typeof(IStructuralEquatable).GetTypeInfo();
		public static bool IsStructural( this Type @this ) => Structural.IsAssignableFrom( @this.GetTypeInfo() );

		public static Type GetMemberType(this MemberInfo memberInfo) => 
			( memberInfo as MethodInfo )?.ReturnType ??
			( memberInfo as PropertyInfo )?.PropertyType ?? 
			( memberInfo as FieldInfo )?.FieldType ?? 
			(memberInfo as TypeInfo)?.AsType();

		public static IEnumerable<Assembly> Assemblies( this IEnumerable<Type> @this ) => @this.Select( x => x.Assembly() ).Distinct();

		public static TypeAdapter Adapt( this object @this ) => CoerceType( @this ).Adapt();
		public static TypeAdapter Adapt( this TypeInfo @this ) => Adapt( @this.AsType() );
		public static TypeAdapter Adapt( this Type @this ) => TypeAdapterCache.Default.Get( @this );

		public static IEnumerable<Type> SelectTypes( this IEnumerable<TypeAdapter> @this ) => @this.Select( adapter => adapter.ReferenceType );
		public static IEnumerable<Type> SelectTypes( this IEnumerable<object> @this ) => @this.Select( CoerceType );
		public static ImmutableArray<Type> AsTypes( this IEnumerable<object> @this ) => @this.SelectTypes().ToImmutableArray();
		public static ImmutableArray<TypeAdapter> AsAdapters( this IEnumerable<object> @this ) => @this.Select( type => type.Adapt() ).ToImmutableArray();
		public static ImmutableArray<TypeAdapter> AsAdapters( this IEnumerable<Type> @this ) => @this.Select( type => type.Adapt() ).ToImmutableArray();

		public static Assembly Assembly( this Type @this ) => Adapt( @this ).Assembly;

		public static bool IsAssignableFrom( this ImmutableArray<Type> @this, Type type ) => @this.ToArray().IsAssignableFrom( type );
		public static bool IsAssignableFrom( this IEnumerable<Type> @this, Type type ) => @this.AsAdapters().IsAssignableFrom( type );
		public static bool IsAssignableFrom( this IEnumerable<TypeAdapter> @this, Type type ) => @this.ToImmutableArray().IsAssignableFrom( type );
		public static bool IsAssignableFrom( this ImmutableArray<TypeAdapter> @this, Type type )
		{
			foreach ( var adapter in @this )
			{
				if ( adapter.IsAssignableFrom( type ) )
				{
					return true;
				}
			}
			return false;
		}

		public static bool IsInstanceOfType( this IEnumerable<Type> @this, object instance ) => @this.AsAdapters().IsInstanceOfType( instance );
		public static bool IsInstanceOfType( this IEnumerable<TypeAdapter> @this, object instance ) => @this.ToImmutableArray().IsInstanceOfType( instance );
		public static bool IsInstanceOfType( this ImmutableArray<TypeAdapter> @this, object instance )
		{
			foreach ( var adapter in @this )
			{
				if ( adapter.IsInstanceOfType( instance ) )
				{
					return true;
				}
			}
			return false;
		}

		public static ImmutableArray<Type> GetParameterTypes( this MethodBase @this ) => Support.ParameterTypes.Get( @this );

		public static IEnumerable<Type> Decorated<TAttribute>( this IEnumerable<Type> target ) where TAttribute : Attribute => target.Where( info => info.Has<TAttribute>() );

		public static IEnumerable<Type> AsTypes( this IEnumerable<TypeInfo> target ) => target.Select( info => info.AsType() );

		public static IEnumerable<TypeInfo> AsTypeInfos( this IEnumerable<Type> target ) => target.Select( info => info.GetTypeInfo() );

		public static ImmutableArray<Type> AsApplicationParts( this IEnumerable<Type> target ) => ApplicationPartsFactory.Default.GetEnumerable( target ).Types;

		static class Support
		{
			public static IParameterizedSource<MethodBase, ImmutableArray<Type>> ParameterTypes { get; } = Caches.Create<MethodBase, ImmutableArray<Type>>( method => method.GetParameters().Select( info => info.ParameterType ).ToImmutableArray() );
		}
	}
}

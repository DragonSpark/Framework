using DragonSpark.TypeSystem;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Extensions
{
	public static class TypeExtensions
	{
		readonly static ConcurrentDictionary<Type, TypeAdapter> Extensions = new ConcurrentDictionary<Type, TypeAdapter>();

		public static Assembly[] Assemblies( [Required] this IEnumerable<Type> @this ) => @this.Select( x => x.Assembly() ).ToArray();

		public static TypeAdapter Adapt( this object @this )
		{
			return @this.GetType().Adapt();
		}

		public static TypeAdapter Adapt( this Type @this )
		{
			return @this.With( item => Extensions.GetOrAdd( item, t => new TypeAdapter( t ) ) );
		}

		public static TypeAdapter Adapt( this TypeInfo @this )
		{
			return @this.With( item => Extensions.GetOrAdd( item.AsType(), t => new TypeAdapter( @this ) ) );
		}

		public static Assembly Assembly( this Type @this )
		{
			return Adapt( @this ).Assembly;
		}
	}
}
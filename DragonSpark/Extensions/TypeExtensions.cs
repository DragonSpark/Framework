using DragonSpark.Runtime.Values;
using DragonSpark.TypeSystem;
using PostSharp.Patterns.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Type = System.Type;

namespace DragonSpark.Extensions
{
	public static class TypeExtensions
	{
		public static Assembly[] Assemblies( [Required] this IEnumerable<Type> @this ) => @this.Select( x => x.Assembly() ).ToArray();

		public static TypeAdapter Adapt( [Required]this Type @this ) => new AssociatedValue<TypeAdapter>( @this, () => new TypeAdapter( @this ) ).Item;

		public static TypeAdapter Adapt( this object @this ) => @this.GetType().Adapt();

		public static TypeAdapter Adapt( [Required]this TypeInfo @this ) => Adapt( @this.AsType() );

		public static Assembly Assembly( [Required]this Type @this ) => Adapt( @this ).Assembly;
	}
}
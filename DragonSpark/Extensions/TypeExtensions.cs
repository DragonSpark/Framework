using DragonSpark.Runtime.Values;
using DragonSpark.TypeSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PostSharp.Patterns.Contracts;
using Type = System.Type;

namespace DragonSpark.Extensions
{
	public static class TypeExtensions
	{
		public static Type GetMemberType(this MemberInfo memberInfo)
		{
		  if (memberInfo is MethodInfo)
			return ((MethodInfo) memberInfo).ReturnType;
		  if (memberInfo is PropertyInfo)
			return ((PropertyInfo) memberInfo).PropertyType;
		  if (memberInfo is FieldInfo)
			return ((FieldInfo) memberInfo).FieldType;
		  return (Type) null;
		}

		public static Assembly[] Assemblies( [Required] this IEnumerable<Type> @this ) => @this.Select( x => x.Assembly() ).ToArray();

		public static TypeAdapter Adapt( [Required]this Type @this ) => new AssociatedValue<TypeAdapter>( @this, () => new TypeAdapter( @this ) ).Item;

		public static TypeAdapter Adapt( this object @this ) => @this.GetType().Adapt();

		public static TypeAdapter Adapt( [Required]this TypeInfo @this ) => Adapt( @this.AsType() );

		public static Assembly Assembly( [Required]this Type @this ) => Adapt( @this ).Assembly;

		public static bool IsDefined<T>( [Required] this Type @this, bool inherited = false ) where T : Attribute => @this.GetTypeInfo().IsDefined( typeof(T), inherited );
	}
}
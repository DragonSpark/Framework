using DragonSpark.Aspects;
using DragonSpark.ComponentModel;
using DragonSpark.TypeSystem;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Extensions
{
	public static class AttributeProviderExtensions
	{
		public static TResult FromMetadata<TAttribute, TResult>( this IAttributeProvider @this, object target, Func<TAttribute, TResult> resolveValue, Func<TResult> resolveDefault = null ) where TAttribute : Attribute
			=> @this.GetAttributes<TAttribute>( target ).WithFirst( resolveValue, resolveDefault ?? DefaultFactory<TResult>.Instance.Create );

		public static bool IsDecoratedWith<TAttribute>( this IAttributeProvider @this, object member ) where TAttribute : Attribute => IsDecoratedWith<TAttribute>( @this, member, false );

		public static bool IsDecoratedWith<TAttribute>( this IAttributeProvider @this, object member, bool inherit ) where TAttribute : Attribute => @this.GetAttribute<TAttribute>( member, inherit ) != null;

		public static TAttribute GetAttribute<TAttribute>( this IAttributeProvider @this, object target, bool inherit = false ) where TAttribute : Attribute => @this.GetAttributes<TAttribute>( target, inherit ).FirstOrDefault();

		public static TAttribute[] GetAttributes<TAttribute>( [Required] this IAttributeProvider @this, [Required]object target, bool inherit = false ) where TAttribute : Attribute =>
			target.AsTo<Assembly, IEnumerable<TAttribute>>( assembly => assembly.GetCustomAttributes<TAttribute>(), () => @this.GetAttributes( target as MemberInfo ?? target.GetType().GetTypeInfo(), typeof(TAttribute), inherit ).Cast<TAttribute>() ).Fixed();
	}
}
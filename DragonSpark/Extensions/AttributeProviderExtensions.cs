using DragonSpark.TypeSystem;
using PostSharp.Patterns.Contracts;
using System;
using System.Linq;

namespace DragonSpark.Extensions
{
	public static class AttributeProviderExtensions
	{
		public static TResult From<TAttribute, TResult>( this object @this, Func<TAttribute, TResult> resolveValue, Func<TResult> resolveDefault = null ) where TAttribute : Attribute => From( Attributes.Get( @this ), resolveValue, resolveDefault );

		public static TResult From<TAttribute, TResult>( [Required]this IAttributeProvider @this, Func<TAttribute, TResult> resolveValue, Func<TResult> resolveDefault = null ) where TAttribute : Attribute => @this.GetAttributes<TAttribute>().WithFirst( resolveValue, resolveDefault );

		public static bool Has<TAttribute>( this object @this ) where TAttribute : Attribute => Has<TAttribute>( Attributes.Get( @this ) );

		public static bool Has<TAttribute>( [Required]this IAttributeProvider @this ) where TAttribute : Attribute => @this.Contains( typeof(TAttribute) );

		public static TAttribute GetAttribute<TAttribute>( this object @this ) where TAttribute : Attribute => GetAttribute<TAttribute>( Attributes.Get( @this ) );

		public static TAttribute GetAttribute<TAttribute>( [Required]this IAttributeProvider @this ) where TAttribute : Attribute => @this.GetAttributes<TAttribute>().FirstOrDefault();

		public static TAttribute[] GetAttributes<TAttribute>( this object @this ) where TAttribute : Attribute => GetAttributes<TAttribute>( Attributes.Get( @this ) );

		public static TAttribute[] GetAttributes<TAttribute>( [Required] this IAttributeProvider @this ) where TAttribute : Attribute => @this.GetAttributes( typeof(TAttribute) ).Cast<TAttribute>().Fixed();
	}
}
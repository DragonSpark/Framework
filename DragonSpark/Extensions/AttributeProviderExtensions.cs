using DragonSpark.TypeSystem;
using DragonSpark.TypeSystem.Metadata;
using System;
using System.Linq;

namespace DragonSpark.Extensions
{
	public static class AttributeProviderExtensions
	{
		public static TResult From<TAttribute, TResult>( this object @this, Func<TAttribute, TResult> resolveValue, Func<TResult> resolveDefault = null ) where TAttribute : Attribute =>
			@this.GetAttributes<TAttribute>().WithFirst( resolveValue, resolveDefault );

		public static bool Has<T>( this object @this ) where T : Attribute => Attributes.Get( @this ).Contains( typeof(T) );

		public static T GetAttribute<T>( this object @this ) where T : Attribute => @this.GetAttributes<T>().FirstOrDefault();

		public static T[] GetAttributes<T>( this object @this ) where T : Attribute
		{
			var attributeProvider = Attributes.Get( @this );
			var attributes = attributeProvider.GetAttributes( typeof(T) );
			var result = attributes as T[] ?? Items<T>.Default;
			return result;
		}
	}
}
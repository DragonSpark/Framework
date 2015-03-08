using System;
using System.IO.IsolatedStorage;
using System.Linq.Expressions;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Extensions
{
	public static class ApplicationSettingsExtensions
	{
		public static TResult Get<TItem,TResult>( this IsolatedStorageSettings target, Expression<Func<TItem, TResult>> expression, Func<TResult> defaultValue = null )
		{
			var key = string.Concat( typeof(TItem).AssemblyQualifiedName, "-", expression.GetMemberInfo().Name );
			var item = target.TryGet( key );
			var result = item.IsDefault() ? defaultValue.Transform( x => x() ) : item.To<TResult>();
			return result;
		}

		public static void Set<TOwner,TItem>( this IsolatedStorageSettings target, Expression<Func<TOwner,TItem>> expression, TItem item )
		{
			var key = string.Concat( typeof(TOwner).AssemblyQualifiedName, "-", expression.GetMemberInfo().Name );
			target[ key ] = item;
		}
	}
}
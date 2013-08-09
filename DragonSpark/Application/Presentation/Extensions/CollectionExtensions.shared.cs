using System;
using System.Collections.Generic;
using DragonSpark.Application.Presentation.ComponentModel;

namespace DragonSpark.Application.Presentation.Extensions
{
	public static class CollectionExtensions
	{
		public static NotificationCollection<TItem> ToNotificationCollection<TItem>( this IEnumerable<TItem> target, Action<string> action, string name )
		{
			var result = new NotificationCollection<TItem>( target, action, name );
			return result;
		}
	}
}
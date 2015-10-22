using System;
using System.Diagnostics.Tracing;
using DragonSpark.Extensions;
using EventSourceProxy;

namespace DragonSpark.Application.Setup
{
	public static class EventListenerExtensions
	{
		public static T ListenTo<T>( this EventListener target, EventLevel level = EventLevel.LogAlways, EventKeywords keywords = EventKeywords.None ) where T : class
		{
			var result = ListenTo( target, typeof(T), level, keywords );
			return (T)result;
		}

		public static object ListenTo( this EventListener target, Type eventSourceType, EventLevel level, EventKeywords keywords = EventKeywords.None )
		{
			var result = EventSourceImplementer.GetEventSource( eventSourceType );
			result.As<EventSource>( x => target.EnableEvents( x, level, keywords ) );
			return result;
		}

		public static T Ignore<T>( this EventListener target ) where T : class
		{
			var result = Ignore( target, typeof(T) );
			return (T)result;
		}

		public static object Ignore( this EventListener target, Type eventSourceType )
		{
			var result = EventSourceImplementer.GetEventSource( eventSourceType );
			result.As<EventSource>( target.DisableEvents );
			return result;
		}
	}
}
using DragonSpark.Extensions;
using DragonSpark.IoC;
using DragonSpark.Logging.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;

namespace DragonSpark.Logging
{
	public static class EventListenerRegistryExtensions
	{
		public static void EnableAll( this IEventListenerRegistry target )
		{
			target.GetAll().Apply( x => target.Retrieve( x ).Apply( y => x.ListenTo( y.Key, y.Value.Level, y.Value.Keywords ) ) );
		}

		public static void DisableAll( this IEventListenerRegistry target )
		{
			target.GetAll().Apply( x => target.Retrieve( x ).Apply( y => x.Ignore( y.Key ) ) );
		}
	}

	public interface IEventListenerRegistry
	{
		void Register( EventListener listener, Type eventSourceType, EventSourceRegistration registration );

		IEnumerable<EventListener> GetAll();

		IEnumerable<KeyValuePair<Type, EventSourceRegistration>> Retrieve( EventListener listener );
	}

	[Singleton( typeof(IEventListenerRegistry) )]
	class EventListenerRegistry : IEventListenerRegistry
	{
		readonly IDictionary<EventListener, IDictionary<Type, EventSourceRegistration>> cache = new Dictionary<EventListener, IDictionary<Type, EventSourceRegistration>>();

		public void Register( EventListener listener, Type eventSourceType, EventSourceRegistration registration )
		{
			Ensure( listener )[ eventSourceType ] = registration;
		}

		public IEnumerable<EventListener> GetAll()
		{
			var result = cache.Keys.ToArray();
			return result;
		}

		public IEnumerable<KeyValuePair<Type,EventSourceRegistration>> Retrieve( EventListener listener )
		{
			var result = Ensure( listener ).ToArray();
			return result;
		}

		IDictionary<Type, EventSourceRegistration> Ensure( EventListener listener )
		{
			var result = cache.Ensure( listener, x => new Dictionary<Type, EventSourceRegistration>() );
			return result;
		}
	}

	public class EventSourceRegistration
	{
		public EventSourceRegistration()
		{
			Level = EventLevel.LogAlways;
			Keywords = Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Keywords.All;
		}

		public EventLevel Level { get; set; }

		public EventKeywords Keywords { get; set; }
	}


}
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using DragonSpark.Extensions;

namespace DragonSpark.Windows.Setup
{
	public class EventListenerRegistry : IEventListenerRegistry
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
}
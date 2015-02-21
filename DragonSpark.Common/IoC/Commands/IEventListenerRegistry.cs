using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;

namespace DragonSpark.Common.IoC.Commands
{
	public interface IEventListenerRegistry
	{
		void Register( EventListener listener, Type eventSourceType, EventSourceRegistration registration );

		IEnumerable<EventListener> GetAll();

		IEnumerable<KeyValuePair<Type, EventSourceRegistration>> Retrieve( EventListener listener );
	}
}
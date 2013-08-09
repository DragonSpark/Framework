using DragonSpark.Extensions;
using DragonSpark.IoC;
using DragonSpark.Logging.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;
using System.Linq;

namespace DragonSpark.Logging
{
	public static class EventListenerRegistryExtensions
	{
		public static void EnableAll( this IEventListenerRegistry target )
		{
			target.GetAll().Apply( x => target.Retrieve( x ).Apply( y => x.ListenTo( y.EventSourceType, y.Level, y.Keywords ) ) );
		}

		public static void DisableAll( this IEventListenerRegistry target )
		{
			target.GetAll().Apply( x => target.Retrieve( x ).Apply( y => x.Ignore( y.EventSourceType ) ) );
		}
	}

	public interface IEventListenerRegistry
	{
		void Register( EventListener listener, EventListenerRegistration registration );

		IEnumerable<EventListener> GetAll();

		IEnumerable<EventListenerRegistration> Retrieve( EventListener listener );
	}

	[Singleton( typeof(IEventListenerRegistry) )]
	class EventListenerRegistry : IEventListenerRegistry
	{
		readonly IDictionary<EventListener, ICollection<EventListenerRegistration>> cache = new Dictionary<EventListener, ICollection<EventListenerRegistration>>();

		public void Register( EventListener listener, EventListenerRegistration registration )
		{
			Ensure( listener ).Add( registration );
		}

		public IEnumerable<EventListener> GetAll()
		{
			var result = cache.Keys.ToArray();
			return result;
		}

		public IEnumerable<EventListenerRegistration> Retrieve( EventListener listener )
		{
			var result = Ensure( listener ).ToArray();
			return result;
		}

		ICollection<EventListenerRegistration> Ensure( EventListener listener )
		{
			var result = cache.Ensure( listener, x => new Collection<EventListenerRegistration>() );
			return result;
		}
	}

	public class EventListenerRegistration
	{
		public EventListenerRegistration()
		{
			Level = EventLevel.LogAlways;
			Keywords = Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Keywords.All;
		}

		public Type EventSourceType { get; set; }

		public EventLevel Level { get; set; }

		public EventKeywords Keywords { get; set; }
	}


}
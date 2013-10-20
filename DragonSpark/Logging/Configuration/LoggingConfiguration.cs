using DragonSpark.Extensions;
using DragonSpark.IoC;
using DragonSpark.IoC.Configuration;
using DragonSpark.Objects;
using EventSourceProxy;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Windows.Markup;

namespace DragonSpark.Logging.Configuration
{
	[ContentProperty( "Definitions" )]
	public class LoggingConfiguration : IContainerConfigurationCommand
	{
		public bool EnableAll { get; set; }

		public void Configure( IUnityContainer container )
		{
			container.RegisterInstance( EventSourceImplementer.GetEventSourceAs<ILogger>() );
			container.TryResolve<IEventListenerRegistry>().With( x =>
			{
				Definitions.Apply( y =>
				{
					var listener = y.Factory.Create<EventListener>( container );
					y.Registrations.Apply( z => x.Register( listener, z.Key, z.Value ?? new EventSourceRegistration() ) );
				} );
				var types = Definitions.SelectMany( y => y.Registrations.Keys ).Distinct().ToArray();
				types.Apply( z => container.RegisterInstance( z, EventSourceImplementer.GetEventSource( z ) ) );
				EnableAll.IsTrue( x.EnableAll );
			} );
		}

		public Collection<EventListenerDefinition> Definitions
		{
			get { return definitions; }
		}	readonly Collection<EventListenerDefinition> definitions = new Collection<EventListenerDefinition>();
	}

	[ContentProperty( "Factory" )]
	public class EventListenerDefinition
	{
		public IFactory Factory { get; set; }

		public IDictionary<Type, EventSourceRegistration> Registrations { get; set; }
	}
}
using DragonSpark.Extensions;
using DragonSpark.IoC;
using DragonSpark.IoC.Configuration;
using DragonSpark.Objects;
using EventSourceProxy;
using Microsoft.Practices.Unity;
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
					y.Registrations.Apply( z => x.Register( listener, z ) );
				} );
				Definitions.SelectMany( y => y.Registrations.Select( z => z.EventSourceType ) ).Distinct().Apply( a => container.RegisterInstance( a, EventSourceImplementer.GetEventSource( a ) ) );
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

		public Collection<EventListenerRegistration> Registrations
		{
			get { return registrations; }
		}	readonly Collection<EventListenerRegistration> registrations = new Collection<EventListenerRegistration>();
	}
}
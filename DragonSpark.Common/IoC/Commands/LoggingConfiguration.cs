using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Markup;
using DragonSpark.Activation.IoC;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using EventSourceProxy;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;

namespace DragonSpark.Common.IoC.Commands
{
	[ContentProperty( "Definitions" )]
	public class LoggingConfiguration : IContainerConfigurationCommand
	{
		[Default( true )]
		public bool EnableAll { get; set; }

		public void Configure( IUnityContainer container )
		{
			container.TryResolve<IEventListenerRegistry>().With( x =>
			{
				Definitions.Where( y => y.Listener != null ).Apply( y =>
				{
					y.Registrations.Apply( z => x.Register( y.Listener, z.Key, z.Value ?? new EventSourceRegistration() ) );
				} );

				var types = Definitions.SelectMany( y => y.Registrations.Keys ).Distinct().ToArray();
				types.Apply( z =>
				{
					var source = EventSourceImplementer.GetEventSource( z );

					// TracingProxy.CreateWithActivityScope<>()
					z.GetTypeInfo().GetAllInterfaces().Apply( a => container.RegisterInstance( a, source ) );
				} );
				EnableAll.IsTrue( x.EnableAll );
			} );
		}

		public Collection<EventListenerDefinition> Definitions
		{
			get { return definitions; }
		}	readonly Collection<EventListenerDefinition> definitions = new Collection<EventListenerDefinition>();
	}
}
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using Microsoft.Practices.Unity;
using Prism;
using Prism.Unity;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Markup;

namespace DragonSpark.Application.Setup
{
	[ContentProperty( "Definitions" )]
	public class LoggingConfiguration : SetupCommand
	{
		[Default( true )]
		public bool EnableAll { get; set; }

		public Collection<EventListenerDefinition> Definitions { get; } = new Collection<EventListenerDefinition>();

		protected override void Execute( SetupContext context )
		{
			var container = context.Container();
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
	}
}
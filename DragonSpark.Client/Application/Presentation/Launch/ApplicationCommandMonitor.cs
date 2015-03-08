using System.Linq;
using DragonSpark.Application.Presentation.Commands;
using DragonSpark.Extensions;
using DragonSpark.IoC;
using DragonSpark.Objects;
using DragonSpark.Runtime;
using Microsoft.Practices.Prism.Events;

namespace DragonSpark.Application.Presentation.Launch
{
	[Singleton( Priority = Priority.Lowest )]
	public class ApplicationCommandMonitor : CommandMonitor
	{
		readonly IEventAggregator aggregator;

		readonly BitFlipper initialized = new BitFlipper(), loaded = new BitFlipper(), completed = new BitFlipper();

		public ApplicationCommandMonitor( IExceptionHandler handler, IEventAggregator aggregator ) : base( handler )
		{
			this.aggregator = aggregator;
		}

		protected override void Monitor( CommandMonitorContext context )
		{
			initialized.Check( () =>
			{
				var done = !context.Commands.Any();
				var items = new[] { ApplicationLaunchStatus.Loading }.Concat( done ? new[] { ApplicationLaunchStatus.Loaded, ApplicationLaunchStatus.Complete } : Enumerable.Empty<ApplicationLaunchStatus>() );
				items.Apply( Publish );
			} );

			base.Monitor( context );
		}

		[DefaultPropertyValue( "The application is currently busy." )]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}

		protected override void OnCompleted( System.Exception exception = null, bool wasCanceled = false )
		{
			loaded.Check( () => Publish( ApplicationLaunchStatus.Loaded ) );
			
			base.OnCompleted( exception, wasCanceled );
		}

		protected override void OnClose()
		{
			base.OnClose();

			completed.Check( () => Threading.Application.Start( () => Publish( ApplicationLaunchStatus.Complete ) ) );
		}

		void Publish( ApplicationLaunchStatus status )
		{
			aggregator.GetEvent<ApplicationLaunchEvent>().With( x => Threading.Application.Execute( () => x.Publish( status ) ) );
		}
	}
}
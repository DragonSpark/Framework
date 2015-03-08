using System.Threading;
using DragonSpark.Application.Presentation.Commands;
using DragonSpark.IoC;
using DragonSpark.Objects;
using Microsoft.Practices.Prism.Events;

namespace DragonSpark.Application.Presentation.Launch
{
	[Singleton( typeof(IOperation), Priority = Priority.Lowest )]
	public class LaunchInitializationOperation : OperationCommandBase
	{
		readonly ManualResetEvent initialized = new ManualResetEvent( false );

		public LaunchInitializationOperation( IEventAggregator aggregator )
		{
			aggregator.ExecuteWhenStatusIs( ApplicationLaunchStatus.Initialized, () => initialized.Set() );
		}

		[DefaultPropertyValue( "Initializing Application Launch Process." )]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}

		protected override void ExecuteCommand( ICommandMonitor commandMonitor )
		{
			initialized.WaitOne();
		}
	}
}
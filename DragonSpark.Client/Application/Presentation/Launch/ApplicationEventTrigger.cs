using System.Windows;
using System.Windows.Interactivity;
using DragonSpark.Application.Presentation.Extensions;
using DragonSpark.Extensions;
using DragonSpark.IoC;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;

namespace DragonSpark.Application.Presentation.Launch
{
    public class ApplicationEventTrigger : TriggerBase<FrameworkElement>
	{
		public ApplicationLaunchStatus TargetStatus
		{
			get { return GetValue( TargetStatusProperty ).To<ApplicationLaunchStatus>(); }
			set { SetValue( TargetStatusProperty, value ); }
		}	public static readonly DependencyProperty TargetStatusProperty = DependencyProperty.Register( "TargetStatus", typeof(ApplicationLaunchStatus), typeof(ApplicationEventTrigger), null );

		[Dependency]
		public IEventAggregator EventAggregator { get; set; }

		protected override void OnAttached()
		{
			base.OnAttached();

			AssociatedObject.EnsureLoaded( x =>
			{
				this.BuildUpOnce();

				EventAggregator.ExecuteWhenStatusIs( TargetStatus, Invoke );
			} );
		}

		void Invoke()
		{
			InvokeActions( null );
		}
	}
}
using DragonSpark.Client.Windows.Extensions;
using DragonSpark.Common.Application;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using Microsoft.Practices.Prism.PubSubEvents;
using System.Windows;
using System.Windows.Interactivity;

namespace DragonSpark.Client.Windows.Interaction
{
    public class SetupEventTrigger : TriggerBase<FrameworkElement>
    {
        public SetupStatus TargetStatus
        {
            get { return GetValue( TargetStatusProperty ).To<SetupStatus>(); }
            set { SetValue( TargetStatusProperty, value ); }
        }	public static readonly DependencyProperty TargetStatusProperty = DependencyProperty.Register( "TargetStatus", typeof(SetupStatus), typeof(SetupEventTrigger), new PropertyMetadata( SetupStatus.Initialized ) );

        [Activate]
        IEventAggregator EventAggregator { get; set; }

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
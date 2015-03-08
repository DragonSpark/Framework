using System.Windows;
using System.Windows.Data;
using System.Windows.Interactivity;
using Expression.Samples.Interactivity.DataHelpers;

namespace DragonSpark.Application.Presentation.Interaction
{
    public class BindingTrigger : TriggerBase<FrameworkElement>
	{
		readonly BindingListener listener;

		public BindingTrigger()
		{
			listener = new BindingListener( OnUpdated );
		}

		void OnUpdated( object sender, BindingChangedEventArgs e )
		{
			if ( e.EventArgs.NewValue != e.EventArgs.OldValue )
			{
				Threading.Application.Start( () => InvokeActions( Binding ) );
			}
		}

		public Binding Binding { get; set; }

		protected override void OnAttached()
		{
			base.OnAttached();
			listener.Element = AssociatedObject;
			listener.Binding = Binding;
		}
	}
}
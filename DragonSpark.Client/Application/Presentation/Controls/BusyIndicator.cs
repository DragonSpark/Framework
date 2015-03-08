using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DragonSpark.Application.Presentation.Extensions;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Controls
{
	/*public class TransitioningContentControl : System.Windows.Controls.TransitioningContentControl
	{
		protected override void OnContentChanged( object oldContent, object newContent )
		{
			base.OnContentChanged( oldContent, newContent );
		}
	}*/

	public class BusyIndicator : System.Windows.Controls.BusyIndicator
	{
		object FocusedElement { get; set; }

		protected override void OnIsBusyChanged(DependencyPropertyChangedEventArgs e)
		{
			if ( IsBusy )
			{
				FocusedElement = FocusManager.GetFocusedElement().As<FrameworkElement>().Transform( x => this.GetParentOfType<System.Windows.Controls.ChildWindow>() != null || x.IsInTree( this ) ? x :null );
			}
			else if ( FocusedElement != null )
			{
				FocusedElement.As<Control>( x => Dispatcher.BeginInvoke( () => x.Focus() )  );
				FocusedElement = null;
			}
			base.OnIsBusyChanged(e);
		}
	}
}
using System.ComponentModel;
using Xamarin.Forms;
using ProgressBar = System.Windows.Controls.ProgressBar;

namespace DragonSpark.Application.Client.Forms.Rendering
{
	public class ActivityIndicatorRenderer : ViewRenderer<ActivityIndicator, ProgressBar>
	{
		protected override void OnElementChanged( ElementChangedEventArgs<ActivityIndicator> e )
		{
			base.OnElementChanged( e );
			SetNativeControl( new ProgressBar() );
			Control.IsIndeterminate = Element.IsRunning;
		}

		protected override void OnElementPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.OnElementPropertyChanged( sender, e );
			if ( e.PropertyName == ActivityIndicator.IsRunningProperty.PropertyName )
			{
				Control.IsIndeterminate = Element.IsRunning;
			}
		}
	}
}

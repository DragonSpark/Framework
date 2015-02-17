using System.ComponentModel;
using ProgressBar = Xamarin.Forms.ProgressBar;

namespace DragonSpark.Client.Windows.Forms.Rendering
{
	public class ProgressBarRenderer : ViewRenderer<ProgressBar, System.Windows.Controls.ProgressBar>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<ProgressBar> e)
		{
			base.OnElementChanged(e);
			System.Windows.Controls.ProgressBar nativeControl = new System.Windows.Controls.ProgressBar
			{
				Minimum = 0.0,
				Maximum = 1.0,
				Value = base.Element.Progress
			};
			base.SetNativeControl(nativeControl);
		}
		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			string propertyName;
			if ((propertyName = e.PropertyName) != null)
			{
				if (!(propertyName == "Progress"))
				{
					return;
				}
				base.Control.Value = base.Element.Progress;
			}
		}
	}
}

using System.ComponentModel;
using System.Windows;
using Xamarin.Forms;
using Slider = Xamarin.Forms.Slider;

namespace DragonSpark.Client.Windows.Forms.Rendering
{
	public class SliderRenderer : ViewRenderer<Slider, System.Windows.Controls.Slider>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Slider> e)
		{
			base.OnElementChanged(e);
			System.Windows.Controls.Slider slider = new System.Windows.Controls.Slider
			{
				Minimum = base.Element.Minimum,
				Maximum = base.Element.Maximum,
				Value = base.Element.Value
			};
			base.SetNativeControl(slider);
			slider.ValueChanged += new RoutedPropertyChangedEventHandler<double>(this.HandleValueChanged);
		}
		private void HandleValueChanged(object sender, RoutedPropertyChangedEventArgs<double> routedPropertyChangedEventArgs)
		{
			((IElementController)base.Element).SetValueFromRenderer(global::Xamarin.Forms.Slider.ValueProperty, base.Control.Value);
		}
		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			string propertyName;
			if ((propertyName = e.PropertyName) != null)
			{
				if (propertyName == "Minimum")
				{
					base.Control.Minimum = base.Element.Minimum;
					return;
				}
				if (propertyName == "Maximum")
				{
					base.Control.Maximum = base.Element.Maximum;
					return;
				}
				if (!(propertyName == "Value"))
				{
					return;
				}
				if (base.Control.Value != base.Element.Value)
				{
					base.Control.Value = base.Element.Value;
				}
			}
		}
	}
}

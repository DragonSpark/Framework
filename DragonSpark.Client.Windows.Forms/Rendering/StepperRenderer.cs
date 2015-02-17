using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Xamarin.Forms;

namespace DragonSpark.Client.Windows.Forms.Rendering
{
	public class StepperRenderer : ViewRenderer<Stepper, Border>
	{
		private readonly StackPanel panel = new StackPanel();
		private System.Windows.Controls.Button upButton;
		private System.Windows.Controls.Button downButton;
		protected override void OnElementChanged(ElementChangedEventArgs<Stepper> e)
		{
			base.OnElementChanged(e);
			Border border = new Border();
			border.Child = this.panel;
			this.panel.HorizontalAlignment = HorizontalAlignment.Right;
			this.panel.Orientation = Orientation.Horizontal;
			this.upButton = new System.Windows.Controls.Button
			{
				Content = "+",
				Width = 100.0
			};
			this.downButton = new System.Windows.Controls.Button
			{
				Content = "-",
				Width = 100.0
			};
			this.panel.Children.Add(this.downButton);
			this.panel.Children.Add(this.upButton);
			base.SetNativeControl(border);
			this.upButton.Click += new RoutedEventHandler(this.UpButtonOnClick);
			this.downButton.Click += new RoutedEventHandler(this.DownButtonOnClick);
			this.UpdateButtons();
		}
		private void UpButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
		{
			((IElementController)base.Element).SetValueFromRenderer(Stepper.ValueProperty, Math.Min(base.Element.Maximum, base.Element.Value + base.Element.Increment));
		}
		private void DownButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
		{
			((IElementController)base.Element).SetValueFromRenderer(Stepper.ValueProperty, Math.Max(base.Element.Minimum, base.Element.Value - base.Element.Increment));
		}
		private void UpdateButtons()
		{
			this.upButton.IsEnabled = (base.Element.Value < base.Element.Maximum);
			this.downButton.IsEnabled = (base.Element.Value > base.Element.Minimum);
		}
		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			string propertyName;
			if ((propertyName = e.PropertyName) != null)
			{
				if (!(propertyName == "Minimum") && !(propertyName == "Maximum") && !(propertyName == "Value"))
				{
					return;
				}
				this.UpdateButtons();
			}
		}
	}
}

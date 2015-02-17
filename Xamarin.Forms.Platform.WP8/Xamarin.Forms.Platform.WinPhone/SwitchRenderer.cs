using Microsoft.Phone.Controls.Primitives;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
namespace Xamarin.Forms.Platform.WinPhone
{
	public class SwitchRenderer : ViewRenderer<Switch, Border>
	{
		private readonly ToggleSwitchButton toggleSwitch = new ToggleSwitchButton();
		protected override void OnElementChanged(ElementChangedEventArgs<Switch> e)
		{
			base.OnElementChanged(e);
			Border nativeControl = new Border
			{
				Child = this.toggleSwitch
			};
			this.toggleSwitch.IsChecked = new bool?(base.Element.IsToggled);
			this.toggleSwitch.Checked += delegate(object sender, RoutedEventArgs args)
			{
				((IElementController)base.Element).SetValueFromRenderer(Switch.IsToggledProperty, true);
			};
			this.toggleSwitch.Unchecked += delegate(object sender, RoutedEventArgs args)
			{
				((IElementController)base.Element).SetValueFromRenderer(Switch.IsToggledProperty, false);
			};
			this.toggleSwitch.HorizontalAlignment = HorizontalAlignment.Right;
			base.SetNativeControl(nativeControl);
		}
		protected override void UpdateNativeWidget()
		{
			base.UpdateNativeWidget();
			this.UpdateSwitchIsEnabled();
		}
		private void UpdateSwitchIsEnabled()
		{
			this.toggleSwitch.IsEnabled = base.Element.IsEnabled;
		}
		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if (e.PropertyName == Switch.IsToggledProperty.PropertyName)
			{
				if (this.toggleSwitch.IsChecked != base.Element.IsToggled)
				{
					this.toggleSwitch.IsChecked = new bool?(base.Element.IsToggled);
					return;
				}
			}
			else
			{
				if (e.PropertyName == VisualElement.IsEnabledProperty.PropertyName)
				{
					this.UpdateSwitchIsEnabled();
				}
			}
		}
	}
}

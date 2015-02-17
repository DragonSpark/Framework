using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Xamarin.Forms;

namespace DragonSpark.Client.Windows.Compensations.Rendering
{
	public class SwitchRenderer : ViewRenderer<Switch, Border>
	{
		readonly ToggleButton toggleSwitch = new ToggleButton();

		protected override void OnElementChanged( ElementChangedEventArgs<Switch> e )
		{
			base.OnElementChanged( e );
			var nativeControl = new Border
			{
				Child = toggleSwitch
			};
			toggleSwitch.IsChecked = Element.IsToggled;
			toggleSwitch.Checked += ( sender, args ) => ( (IElementController)Element ).SetValueFromRenderer( Switch.IsToggledProperty, true );
			toggleSwitch.Unchecked += ( sender, args ) => ( (IElementController)Element ).SetValueFromRenderer( Switch.IsToggledProperty, false );
			toggleSwitch.HorizontalAlignment = HorizontalAlignment.Right;
			SetNativeControl( nativeControl );
		}

		protected override void UpdateNativeWidget()
		{
			base.UpdateNativeWidget();
			UpdateSwitchIsEnabled();
		}

		void UpdateSwitchIsEnabled()
		{
			toggleSwitch.IsEnabled = Element.IsEnabled;
		}

		protected override void OnElementPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.OnElementPropertyChanged( sender, e );
			if ( e.PropertyName == Switch.IsToggledProperty.PropertyName )
			{
				if ( toggleSwitch.IsChecked != Element.IsToggled )
				{
					toggleSwitch.IsChecked = Element.IsToggled;
				}
			}
			else
			{
				if ( e.PropertyName == VisualElement.IsEnabledProperty.PropertyName )
				{
					UpdateSwitchIsEnabled();
				}
			}
		}
	}
}

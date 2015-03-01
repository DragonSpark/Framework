using System;
using System.ComponentModel;
using System.Windows;
using Xamarin.Forms;

namespace DragonSpark.Application.Client.Forms.Rendering
{
	public class TimePickerRenderer : ViewRenderer<TimePicker, Xceed.Wpf.Toolkit.TimePicker>
	{
		protected override void OnElementChanged( ElementChangedEventArgs<TimePicker> e )
		{
			base.OnElementChanged( e );
			var timePicker = new Xceed.Wpf.Toolkit.TimePicker
			{
				Value = DateTime.Today.Add( Element.Time )
			};
			timePicker.ValueChanged += TimePickerOnValueChanged;
			SetNativeControl( timePicker );
			UpdateFormatString();
		}

		void TimePickerOnValueChanged( object sender, RoutedPropertyChangedEventArgs<object> routedPropertyChangedEventArgs )
		{
			if ( Control.Value.HasValue )
			{
				( (IElementController)Element ).SetValueFromRenderer( TimePicker.TimeProperty, Control.Value.Value - DateTime.Today );
			}
		}

		void UpdateFormatString()
		{
			Control.FormatString = "{0:" + Element.Format + "}";
		}

		protected override void OnElementPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.OnElementPropertyChanged( sender, e );
			if ( e.PropertyName == "Time" )
			{
				Control.Value = DateTime.Today.Add( Element.Time );
				return;
			}
			if ( e.PropertyName == TimePicker.FormatProperty.PropertyName )
			{
				UpdateFormatString();
			}
		}

		internal override void OnModelFocusChangeRequested( object sender, VisualElement.FocusRequestArgs args )
		{
			var control = Control;
			if ( control == null )
			{
				return;
			}
			if ( args.Focus )
			{
				control.IsOpen = 
				// typeof(DateTimePickerBase).InvokeMember( "OpenPickerPage", BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic, Type.DefaultBinder, control, null );
				args.Result = true;
				return;
			}
			base.UnfocusControl( control );
			args.Result = true;
		}
	}
}

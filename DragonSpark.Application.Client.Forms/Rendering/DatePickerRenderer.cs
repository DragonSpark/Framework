using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xceed.Wpf.Toolkit;

namespace DragonSpark.Application.Client.Forms.Rendering
{
	public class DatePickerRenderer : ViewRenderer<DatePicker, DateTimePicker>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
		{
			base.OnElementChanged(e);
			var datePicker = new DateTimePicker
			{
				Value = base.Element.Date
			};
			datePicker.ValueChanged += DatePickerOnValueChanged;
			base.SetNativeControl(datePicker);
			UpdateFormatString();
		}
		private void DatePickerOnValueChanged(object sender, EventArgs dateTimeValueChangedEventArgs)
		{
			if (Control.Value.HasValue)
			{
				((IElementController)base.Element).SetValueFromRenderer(DatePicker.DateProperty, Control.Value.Value);
			}
		}
		private void UpdateFormatString()
		{
			Control.FormatString = string.Format( "{{0:{0}}}", base.Element.Format );
		}
		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			switch ( e.PropertyName )
			{
				case "Date":
					Control.Value = Element.Date;
					break;
				default:
					if (e.PropertyName == global::Xamarin.Forms.DatePicker.FormatProperty.PropertyName)
					{
						UpdateFormatString();
					}
					break;
			}
			base.OnElementPropertyChanged(sender, e);
		}

		internal override void OnModelFocusChangeRequested(object sender, VisualElement.FocusRequestArgs args)
		{
			var control = Control;
			if (control == null)
			{
				return;
			}
			if (args.Focus)
			{
				control.IsOpen =
				// typeof(DateTimePickerBase).InvokeMember("OpenPickerPage", BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic, Type.DefaultBinder, control, null);
				args.Result = true;
				return;
			}
			UnfocusControl(control);
			args.Result = true;
		}
	}
}

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Xamarin.Forms;
using Button = Xamarin.Forms.Button;

namespace DragonSpark.Client.Windows.Forms.Rendering
{
	public class ButtonRenderer : ViewRenderer<Button, System.Windows.Controls.Button>
	{
		private bool fontApplied;
		protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
		{
			base.OnElementChanged(e);
			System.Windows.Controls.Button button = new System.Windows.Controls.Button();
			button.Click += new RoutedEventHandler(this.HandleButtonClick);
			base.SetNativeControl(button);
			this.UpdateContent();
			if (base.Element.BackgroundColor != global::Xamarin.Forms.Color.Default)
			{
				this.UpdateBackground();
			}
			if (base.Element.TextColor != global::Xamarin.Forms.Color.Default)
			{
				this.UpdateTextColor();
			}
			if (base.Element.BorderColor != global::Xamarin.Forms.Color.Default)
			{
				this.UpdateBorderColor();
			}
			if (base.Element.BorderWidth != 0.0)
			{
				this.UpdateBorderWidth();
			}
			this.UpdateFont();
		}
		private void HandleButtonClick(object sender, RoutedEventArgs e)
		{
			global::Xamarin.Forms.Button element = base.Element;
			if (element != null)
			{
				((IButtonController)element).SendClicked();
			}
		}
		private void UpdateContent()
		{
			if (base.Element.Image != null)
			{
				base.Control.Content = new StackPanel
				{
					Orientation = Orientation.Horizontal,
					Children = 
					{
						new System.Windows.Controls.Image
						{
							Source = new BitmapImage(new Uri("/" + base.Element.Image.File, UriKind.Relative)),
							Width = 30.0,
							Height = 30.0,
							Margin = new System.Windows.Thickness(0.0, 0.0, 20.0, 0.0)
						},
						new TextBlock
						{
							Text = base.Element.Text
						}
					}
				};
				return;
			}
			base.Control.Content = base.Element.Text;
		}
		private void UpdateFont()
		{
			if (base.Control == null || base.Element == null)
			{
				return;
			}
			if (base.Element.Font == Font.Default && !this.fontApplied)
			{
				return;
			}
			Font font = (base.Element.Font == Font.Default) ? Font.SystemFontOfSize(NamedSize.Medium) : base.Element.Font;
			base.Control.ApplyFont(font);
			this.fontApplied = true;
		}
		private void UpdateBackground()
		{
			base.Control.Background = ((base.Element.BackgroundColor != global::Xamarin.Forms.Color.Default) ? base.Element.BackgroundColor.ToBrush() : ((Brush)System.Windows.Application.Current.Resources["PhoneBackgroundBrush"]));
		}
		private void UpdateTextColor()
		{
			base.Control.Foreground = ((base.Element.TextColor != global::Xamarin.Forms.Color.Default) ? base.Element.TextColor.ToBrush() : ((Brush)System.Windows.Application.Current.Resources["PhoneForegroundBrush"]));
		}
		private void UpdateBorderColor()
		{
			base.Control.BorderBrush = ((base.Element.BorderColor != global::Xamarin.Forms.Color.Default) ? base.Element.BorderColor.ToBrush() : ((Brush)System.Windows.Application.Current.Resources["PhoneForegroundBrush"]));
		}
		private void UpdateBorderWidth()
		{
			base.Control.BorderThickness = ((base.Element.BorderWidth == 0.0) ? new System.Windows.Thickness(3.0) : new System.Windows.Thickness(base.Element.BorderWidth));
		}
		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if (e.PropertyName == global::Xamarin.Forms.Button.TextProperty.PropertyName || e.PropertyName == global::Xamarin.Forms.Button.ImageProperty.PropertyName)
			{
				this.UpdateContent();
				return;
			}
			if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName)
			{
				this.UpdateBackground();
				return;
			}
			if (e.PropertyName == global::Xamarin.Forms.Button.TextColorProperty.PropertyName)
			{
				this.UpdateTextColor();
				return;
			}
			if (e.PropertyName == global::Xamarin.Forms.Button.FontProperty.PropertyName)
			{
				this.UpdateFont();
				return;
			}
			if (e.PropertyName == global::Xamarin.Forms.Button.BorderColorProperty.PropertyName)
			{
				this.UpdateBorderColor();
				return;
			}
			if (e.PropertyName == global::Xamarin.Forms.Button.BorderWidthProperty.PropertyName)
			{
				this.UpdateBorderWidth();
			}
		}
	}
}

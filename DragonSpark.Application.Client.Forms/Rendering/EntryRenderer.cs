using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Xamarin.Forms;
using Color = Xamarin.Forms.Color;
using Grid = System.Windows.Controls.Grid;
using Size = System.Windows.Size;
using TextChangedEventArgs = System.Windows.Controls.TextChangedEventArgs;

namespace DragonSpark.Application.Client.Forms.Rendering
{
	public class EntryRenderer : ViewRenderer<Entry, Grid>
	{
		EntryTextBox textBox;
		PasswordBox passwordBox;
		bool passwordBoxHasFocus;

		protected override void OnElementChanged( ElementChangedEventArgs<Entry> e )
		{
			base.OnElementChanged( e );
			textBox = new EntryTextBox();
			textBox.LostFocus += OnTextBoxUnfocused;
			passwordBox = new PasswordBox();
			SetNativeControl( new Grid
			{
				Children =
				{
					textBox,
					passwordBox
				}
			} );
			UpdateText();
			UpdateInputScope();
			UpdatePlaceholder();
			UpdateColor();
			textBox.TextChanged += TextBoxOnTextChanged;
			textBox.KeyUp += TextBoxOnKeyUp;
			/*passwordBox.PasswordChanged += PasswordBoxOnPasswordChanged;
			passwordBox.KeyUp += TextBoxOnKeyUp;*/
			this.passwordBox.IsHitTestVisible = false;
			passwordBox.GotFocus += delegate
			{
				this.passwordBoxHasFocus = true;
				this.UpdateControl();
			};
			passwordBox.LostFocus += delegate
			{
				this.passwordBoxHasFocus = false;
				this.UpdateControl();
				if ( base.Element.TextColor != Color.Default && !string.IsNullOrEmpty( base.Element.Text ) )
				{
					this.passwordBox.Foreground = base.Element.TextColor.ToBrush();
				}
			};
			passwordBox.IsEnabled = ( textBox.IsEnabled = Element.IsEnabled );
			UpdateControl();
		}

		void OnTextBoxUnfocused( object sender, RoutedEventArgs e )
		{
			if ( Element.TextColor == Color.Default )
			{
				return;
			}
			if ( !string.IsNullOrEmpty( Element.Text ) )
			{
				textBox.Foreground = Element.TextColor.ToBrush();
			}
		}

		void UpdateInputScope()
		{
			textBox.InputScope = Element.Keyboard.ToInputScope();
		}

		internal override void OnModelFocusChangeRequested( object sender, VisualElement.FocusRequestArgs args )
		{
			var control = textBox.IsHitTestVisible ? (Control)textBox : passwordBox;
			if ( args.Focus )
			{
				args.Result = control.Focus();
				return;
			}
			UnfocusControl( control );
			args.Result = true;
		}

		public override SizeRequest GetDesiredSize( double widthConstraint, double heightConstraint )
		{
			if ( Children.Count == 0 )
			{
				return default( SizeRequest );
			}
			var availableSize = new Size( widthConstraint, heightConstraint );
			var frameworkElement = (FrameworkElement)Control.Children[0];
			var width = frameworkElement.Width;
			var height = frameworkElement.Height;
			frameworkElement.Height = double.NaN;
			frameworkElement.Width = double.NaN;
			frameworkElement.Measure( availableSize );
			var request = new global::Xamarin.Forms.Size( Math.Ceiling( frameworkElement.DesiredSize.Width ), Math.Ceiling( frameworkElement.DesiredSize.Height ) );
			frameworkElement.Width = width;
			frameworkElement.Height = height;
			return new SizeRequest( request );
		}

		void TextBoxOnKeyUp( object sender, KeyEventArgs keyEventArgs )
		{
			if ( keyEventArgs.Key == Key.Enter )
			{
				Element.SendCompleted();
			}
		}

		void TextBoxOnTextChanged( object sender, TextChangedEventArgs textChangedEventArgs )
		{
			( (IElementController)Element ).SetValueFromRenderer( Entry.TextProperty, textBox.Text );
		}

		protected override void OnElementPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.OnElementPropertyChanged( sender, e );
			if ( e.PropertyName == Entry.TextProperty.PropertyName )
			{
				UpdateControl();
				UpdateText();
				return;
			}
			if ( e.PropertyName == Entry.PlaceholderProperty.PropertyName )
			{
				UpdatePlaceholder();
				return;
			}
			if ( e.PropertyName == Entry.IsPasswordProperty.PropertyName )
			{
				UpdateControl();
				return;
			}
			if ( e.PropertyName == VisualElement.IsEnabledProperty.PropertyName )
			{
				passwordBox.IsEnabled = ( textBox.IsEnabled = Element.IsEnabled );
				return;
			}
			if ( e.PropertyName == Entry.TextColorProperty.PropertyName )
			{
				UpdateColor();
				return;
			}
			if ( e.PropertyName == InputView.KeyboardProperty.PropertyName )
			{
				UpdateInputScope();
			}
		}

		void UpdateText()
		{
			textBox.Text = ( Element.Text ?? "" );
			textBox.Select( textBox.Text.Length, 0 );
			passwordBox.Password = ( Element.Text ?? "" );
		}

		void UpdateColor()
		{
			if ( textBox == null )
			{
				return;
			}
			var element = Element;
			if ( element != null )
			{
				if ( !string.IsNullOrEmpty( element.Text ) )
				{
					if ( element.TextColor != Color.Default )
					{
						textBox.Foreground = element.TextColor.ToBrush();
						passwordBox.Foreground = textBox.Foreground;
						return;
					}
					textBox.Foreground = (Brush)System.Windows.Controls.Control.ForegroundProperty.GetMetadata( typeof(EntryTextBox) ).DefaultValue;
					passwordBox.Foreground = (Brush)System.Windows.Controls.Control.ForegroundProperty.GetMetadata( typeof(PasswordBox) ).DefaultValue;
				}
			}
			else
			{
				textBox.Foreground = (Brush)System.Windows.Controls.Control.ForegroundProperty.GetMetadata( typeof(EntryTextBox) ).DefaultValue;
				passwordBox.Foreground = (Brush)System.Windows.Controls.Control.ForegroundProperty.GetMetadata( typeof(PasswordBox) ).DefaultValue;
			}
		}

		void UpdatePlaceholder()
		{
			textBox.Hint = ( Element.Placeholder ?? "" );
		}

		private void UpdateControl()
		{
			if (!base.Element.IsPassword)
			{
				this.passwordBox.Opacity = 0.0;
				this.textBox.Opacity = 1.0;
				return;
			}
			if (!string.IsNullOrEmpty(base.Element.Text) || this.passwordBoxHasFocus)
			{
				this.passwordBox.Opacity = 1.0;
				this.textBox.Opacity = 0.0;
				return;
			}
			this.passwordBox.Opacity = 0.0;
			this.textBox.Opacity = 1.0;
		}

		void PasswordBoxOnPasswordChanged( object sender, RoutedEventArgs routedEventArgs )
		{
			Element.SetValue( Entry.TextProperty, passwordBox.Password );
		}
	}
}

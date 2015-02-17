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

namespace DragonSpark.Client.Windows.Forms.Rendering
{
	public class EntryRenderer : ViewRenderer<Entry, Grid>
	{
		EntryTextBox entryTextBox;
		PasswordBox passwordBox;
		bool passwordBoxHasFocus;

		protected override void OnElementChanged( ElementChangedEventArgs<Entry> e )
		{
			base.OnElementChanged( e );
			entryTextBox = new EntryTextBox();
			entryTextBox.LostFocus += OnTextBoxUnfocused;
			passwordBox = new PasswordBox();
			SetNativeControl( new Grid
			{
				Children =
				{
					entryTextBox,
					passwordBox
				}
			} );
			UpdateText();
			UpdateInputScope();
			UpdatePlaceholder();
			UpdateColor();
			entryTextBox.TextChanged += TextBoxOnTextChanged;
			entryTextBox.KeyUp += TextBoxOnKeyUp;
			passwordBox.PasswordChanged += PasswordBoxOnPasswordChanged;
			passwordBox.KeyUp += TextBoxOnKeyUp;
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
			passwordBox.IsEnabled = ( entryTextBox.IsEnabled = Element.IsEnabled );
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
				entryTextBox.Foreground = Element.TextColor.ToBrush();
			}
		}

		void UpdateInputScope()
		{
			entryTextBox.InputScope = Element.Keyboard.ToInputScope();
		}

		internal override void OnModelFocusChangeRequested( object sender, VisualElement.FocusRequestArgs args )
		{
			var control = entryTextBox.IsHitTestVisible ? (Control)entryTextBox : passwordBox;
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
			( (IElementController)Element ).SetValueFromRenderer( Entry.TextProperty, entryTextBox.Text );
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
				passwordBox.IsEnabled = ( entryTextBox.IsEnabled = Element.IsEnabled );
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
			entryTextBox.Text = ( Element.Text ?? "" );
			entryTextBox.Select( entryTextBox.Text.Length, 0 );
			passwordBox.Password = ( Element.Text ?? "" );
		}

		void UpdateColor()
		{
			if ( entryTextBox == null )
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
						entryTextBox.Foreground = element.TextColor.ToBrush();
						passwordBox.Foreground = entryTextBox.Foreground;
						return;
					}
					entryTextBox.Foreground = (Brush)System.Windows.Controls.Control.ForegroundProperty.GetMetadata( typeof(EntryTextBox) ).DefaultValue;
					passwordBox.Foreground = (Brush)System.Windows.Controls.Control.ForegroundProperty.GetMetadata( typeof(PasswordBox) ).DefaultValue;
				}
			}
			else
			{
				entryTextBox.Foreground = (Brush)System.Windows.Controls.Control.ForegroundProperty.GetMetadata( typeof(EntryTextBox) ).DefaultValue;
				passwordBox.Foreground = (Brush)System.Windows.Controls.Control.ForegroundProperty.GetMetadata( typeof(PasswordBox) ).DefaultValue;
			}
		}

		void UpdatePlaceholder()
		{
			entryTextBox.Hint = ( Element.Placeholder ?? "" );
		}

		void UpdateControl()
		{
			if ( Element.IsPassword )
			{
				if ( !string.IsNullOrEmpty( Element.Text ) || passwordBoxHasFocus )
				{
					passwordBox.Opacity = 1.0;
					entryTextBox.Opacity = 0.0;
				}
				else
				{
					passwordBox.Opacity = 0.0;
					entryTextBox.Opacity = 1.0;
				}
				passwordBox.IsHitTestVisible = true;
				entryTextBox.IsHitTestVisible = false;
				return;
			}
			passwordBox.Opacity = 0.0;
			entryTextBox.Opacity = 1.0;
			passwordBox.IsHitTestVisible = false;
			entryTextBox.IsHitTestVisible = true;
		}

		void PasswordBoxOnPasswordChanged( object sender, RoutedEventArgs routedEventArgs )
		{
			Element.SetValue( Entry.TextProperty, passwordBox.Password );
		}
	}
}

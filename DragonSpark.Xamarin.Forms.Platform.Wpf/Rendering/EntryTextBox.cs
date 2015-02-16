using DragonSpark.Extensions;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DragonSpark.Xamarin.Forms.Platform.Wpf.Rendering
{
	[TemplatePart( Name = "Text", Type = typeof(TextBox) ), TemplatePart( Name = "HintContent", Type = typeof(ContentControl) ), TemplatePart( Name = "LengthIndicator", Type = typeof(TextBlock) ), TemplateVisualState( Name = "LengthIndicatorHidden", GroupName = "LengthIndicatorStates" ), TemplateVisualState( Name = "ReadOnly", GroupName = "CommonStates" ), TemplateVisualState( Name = "LengthIndicatorVisible", GroupName = "LengthIndicatorStates" ), TemplateVisualState( Name = "Normal", GroupName = "CommonStates" ), TemplateVisualState( Name = "Disabled", GroupName = "CommonStates" ), TemplateVisualState( Name = "Unfocused", GroupName = "FocusStates" ), TemplateVisualState( Name = "Focused", GroupName = "FocusStates" )]
	public class EntryTextBox : TextBox
	{
		Grid rootGrid;
		TextBox textBox;
		TextBlock measurementTextBlock;
		readonly Brush foregroundBrushInactive = (Brush)System.Windows.Application.Current.Resources["PhoneTextBoxReadOnlyBrush"];
		Brush foregroundBrushEdit;
		ContentControl hintContent;
		Border hintBorder;
		TextBlock lengthIndicator;
		bool ignorePropertyChange;
		bool ignoreFocus;
		public static readonly DependencyProperty HintProperty = DependencyProperty.Register( "Hint", typeof(string), typeof(EntryTextBox), new PropertyMetadata( OnHintPropertyChanged ) );
		public static readonly DependencyProperty HintStyleProperty = DependencyProperty.Register( "HintStyle", typeof(Style), typeof(EntryTextBox), null );
		public static readonly DependencyProperty ActualHintVisibilityProperty = DependencyProperty.Register( "ActualHintVisibility", typeof(Visibility), typeof(EntryTextBox), null );
		public static readonly DependencyProperty LengthIndicatorVisibleProperty = DependencyProperty.Register( "LengthIndicatorVisible", typeof(bool), typeof(EntryTextBox), null );
		public static readonly DependencyProperty LengthIndicatorThresholdProperty = DependencyProperty.Register( "LengthIndicatorThreshold", typeof(int), typeof(EntryTextBox), new PropertyMetadata( OnLengthIndicatorThresholdChanged ) );
		public static readonly DependencyProperty DisplayedMaxLengthProperty = DependencyProperty.Register( "DisplayedMaxLength", typeof(int), typeof(EntryTextBox), new PropertyMetadata( DisplayedMaxLengthChanged ) );
		public static readonly DependencyProperty ActionIconProperty = DependencyProperty.Register( "ActionIcon", typeof(ImageSource), typeof(EntryTextBox), new PropertyMetadata( OnActionIconChanged ) );
		public static readonly DependencyProperty HidesActionItemWhenEmptyProperty = DependencyProperty.Register( "HidesActionItemWhenEmpty", typeof(bool), typeof(EntryTextBox), new PropertyMetadata( false, OnActionIconChanged ) );
		public event EventHandler ActionIconTapped;
		protected Border ActionIconBorder { get; set; }

		public string Hint
		{
			get { return GetValue( HintProperty ) as string; }
			set { SetValue( HintProperty, value ); }
		}

		public Style HintStyle
		{
			get { return GetValue( HintStyleProperty ) as Style; }
			set { SetValue( HintStyleProperty, value ); }
		}

		public Visibility ActualHintVisibility
		{
			get { return (Visibility)GetValue( ActualHintVisibilityProperty ); }
			set { SetValue( ActualHintVisibilityProperty, value ); }
		}

		public bool LengthIndicatorVisible
		{
			get { return (bool)GetValue( LengthIndicatorVisibleProperty ); }
			set { SetValue( LengthIndicatorVisibleProperty, value ); }
		}

		public int LengthIndicatorThreshold
		{
			get { return (int)GetValue( LengthIndicatorThresholdProperty ); }
			set { SetValue( LengthIndicatorThresholdProperty, value ); }
		}

		public int DisplayedMaxLength
		{
			get { return (int)GetValue( DisplayedMaxLengthProperty ); }
			set { SetValue( DisplayedMaxLengthProperty, value ); }
		}

		public ImageSource ActionIcon
		{
			get { return GetValue( ActionIconProperty ) as ImageSource; }
			set { SetValue( ActionIconProperty, value ); }
		}

		public bool HidesActionItemWhenEmpty
		{
			get { return (bool)GetValue( HidesActionItemWhenEmptyProperty ); }
			set { SetValue( HidesActionItemWhenEmptyProperty, value ); }
		}

		static void OnHintPropertyChanged( DependencyObject sender, DependencyPropertyChangedEventArgs args )
		{
			var textBox = sender as EntryTextBox;
			if ( textBox != null && textBox.hintContent != null )
			{
				textBox.UpdateHintVisibility();
			}
		}

		void UpdateHintVisibility()
		{
			if ( hintContent != null )
			{
				if ( string.IsNullOrEmpty( Text ) )
				{
					ActualHintVisibility = Visibility.Visible;
					Foreground = foregroundBrushInactive;
					return;
				}
				ActualHintVisibility = Visibility.Collapsed;
				Foreground = foregroundBrushEdit;
			}
		}

		protected override void OnLostFocus( RoutedEventArgs e )
		{
			UpdateHintVisibility();
			base.OnLostFocus( e );
		}

		protected override void OnGotFocus( RoutedEventArgs e )
		{
			if ( ignoreFocus )
			{
				ignoreFocus = false;
				System.Windows.Application.Current.MainWindow.Content.As<Frame>( x => x.Focus() );
				return;
			}
			Foreground = foregroundBrushEdit;
			if ( hintContent != null )
			{
				ActualHintVisibility = Visibility.Collapsed;
			}
			base.OnGotFocus( e );
		}

		static void OnLengthIndicatorThresholdChanged( DependencyObject sender, DependencyPropertyChangedEventArgs args )
		{
			var textBox = sender as EntryTextBox;
			if ( textBox.ignorePropertyChange )
			{
				textBox.ignorePropertyChange = false;
				return;
			}
			if ( textBox.LengthIndicatorThreshold < 0 )
			{
				textBox.ignorePropertyChange = true;
				textBox.SetValue( LengthIndicatorThresholdProperty, args.OldValue );
				throw new ArgumentOutOfRangeException( "LengthIndicatorThreshold", "The length indicator visibility threshold must be greater than zero." );
			}
		}

		static void DisplayedMaxLengthChanged( DependencyObject sender, DependencyPropertyChangedEventArgs args )
		{
			var textBox = sender as EntryTextBox;
			if ( textBox.DisplayedMaxLength > textBox.MaxLength && textBox.MaxLength > 0 )
			{
				throw new ArgumentOutOfRangeException( "DisplayedMaxLength", "The displayed maximum length cannot be greater than the MaxLength." );
			}
		}

		void UpdateLengthIndicatorVisibility()
		{
			if ( rootGrid == null || lengthIndicator == null )
			{
				return;
			}
			var flag = true;
			if ( LengthIndicatorVisible )
			{
				lengthIndicator.Text = string.Format( CultureInfo.InvariantCulture, "{0}/{1}", Text.Length, ( DisplayedMaxLength > 0 ) ? DisplayedMaxLength : MaxLength );
				if ( Text.Length >= LengthIndicatorThreshold )
				{
					flag = false;
				}
			}
			VisualStateManager.GoToState( this, flag ? "LengthIndicatorHidden" : "LengthIndicatorVisible", false );
		}

		static void OnActionIconChanged( DependencyObject sender, DependencyPropertyChangedEventArgs args )
		{
			var textBox = sender as EntryTextBox;
			if ( textBox != null )
			{
				textBox.UpdateActionIconVisibility();
			}
		}

		void UpdateActionIconVisibility()
		{
			if ( ActionIconBorder != null )
			{
				if ( ActionIcon == null || ( HidesActionItemWhenEmpty && string.IsNullOrEmpty( Text ) ) )
				{
					ActionIconBorder.Visibility = Visibility.Collapsed;
					hintBorder.Padding = new Thickness( 0.0 );
					return;
				}
				ActionIconBorder.Visibility = Visibility.Visible;
				if ( TextWrapping != TextWrapping.Wrap )
				{
					hintBorder.Padding = new Thickness( 0.0, 0.0, 48.0, 0.0 );
				}
			}
		}

		void OnActionIconTapped( object sender, RoutedEventArgs e )
		{
			ignoreFocus = true;
			var actionIconTapped = ActionIconTapped;
			if ( actionIconTapped != null )
			{
				actionIconTapped( this, e );
			}
		}

		void ResizeTextBox()
		{
			if ( ActionIcon == null || TextWrapping != TextWrapping.Wrap )
			{
				return;
			}
			measurementTextBlock.Width = ActualWidth;
			if ( measurementTextBlock.ActualHeight > ActualHeight - 72.0 )
			{
				Height = ActualHeight + 72.0;
				return;
			}
			if ( ActualHeight > measurementTextBlock.ActualHeight + 144.0 )
			{
				Height = ActualHeight - 72.0;
			}
		}

		public EntryTextBox()
		{
			DefaultStyleKey = typeof(EntryTextBox);
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			if ( textBox != null )
			{
				textBox.TextChanged -= OnTextChanged;
			}
			if ( ActionIconBorder != null )
			{
				ActionIconBorder.MouseLeftButtonDown -= OnActionIconTapped;
			}
			rootGrid = ( GetTemplateChild( "RootGrid" ) as Grid );
			textBox = ( GetTemplateChild( "Text" ) as System.Windows.Controls.TextBox );
			foregroundBrushEdit = Foreground;
			hintContent = ( GetTemplateChild( "HintContent" ) as ContentControl );
			hintBorder = ( GetTemplateChild( "HintBorder" ) as Border );
			if ( hintContent != null )
			{
				UpdateHintVisibility();
			}
			lengthIndicator = ( GetTemplateChild( "LengthIndicator" ) as TextBlock );
			ActionIconBorder = ( GetTemplateChild( "ActionIconBorder" ) as Border );
			if ( rootGrid != null && lengthIndicator != null )
			{
				UpdateLengthIndicatorVisibility();
			}
			if ( textBox != null )
			{
				textBox.TextChanged += OnTextChanged;
			}
			if ( ActionIconBorder != null )
			{
				ActionIconBorder.MouseLeftButtonDown += OnActionIconTapped;
				UpdateActionIconVisibility();
			}
			measurementTextBlock = ( GetTemplateChild( "MeasurementTextBlock" ) as TextBlock );
		}

		void OnTextChanged( object sender, RoutedEventArgs e )
		{
			UpdateLengthIndicatorVisibility();
			UpdateActionIconVisibility();
			UpdateHintVisibility();
			ResizeTextBox();
		}
	}
}
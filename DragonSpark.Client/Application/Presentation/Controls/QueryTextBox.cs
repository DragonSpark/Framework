using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using DragonSpark.Application.Presentation.Extensions;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Controls
{
	[TemplatePart( Name = "QueryClearButton", Type = typeof(Button) ), TemplatePart( Name = "QueryImageBorder", Type = typeof(Button) ), TemplateVisualState( Name = "Watermarked", GroupName = "QueryStates" ), TemplateVisualState( Name = "Cleared", GroupName = "QueryStates" ), TemplateVisualState( Name = "Queried", GroupName = "QueryStates" )]
	public class QueryTextBox : TextBox
	{
		public event EventHandler Queried = delegate { };

		readonly DispatcherTimer queryTimer;

		public QueryTextBox()
		{
			DefaultStyleKey = typeof(QueryTextBox);
			queryTimer = new DispatcherTimer { Interval = SearchEventTimeDelay.TimeSpan };
			queryTimer.Tick += ( sender, args ) => UpdateText();
			TextChanged += ( sender, args ) =>
			{
			    UpdateState();
			    queryTimer.Stop();

			    if ( Text != CurrentQuery )
			    {
			        queryTimer.Start();
			    }
			};
		}

		public void Reset()
		{
			Text = string.Empty;
			UpdateText();
			this.GetParentOfType<Control>().NotNull( x => x.Focus() );
			UpdateState();
		}

		protected override void OnGotFocus(RoutedEventArgs e)
		{
			UpdateState();
			base.OnGotFocus(e);
		}

		protected override void OnLostFocus(RoutedEventArgs e)
		{
			UpdateState();
			base.OnLostFocus(e);
		}

		void UpdateState()
		{
			var state = string.IsNullOrEmpty( Text ) ? ( FocusManager.GetFocusedElement() == this ? "Cleared" : "Watermarked" ) : "Queried";
			VisualStateManager.GoToState( this, state, true );
		}

		void UpdateText()
		{
			queryTimer.Stop();
			CurrentQuery = Text;
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			GetTemplateChild( "QueryClearButton" ).As<Button>( x =>
			{
			    x.Click +=	( sender, args ) => CurrentQuery = Text = string.Empty;
			} );
			GetTemplateChild( "QueryImageBorder" ).As<Button>( x =>
			{
			    x.Click += ( sender, args ) => Focus();
			} );
		}

		protected override void OnKeyDown( KeyEventArgs e )
		{
			switch ( e.Key )
			{
				case Key.Escape:
					Text = string.Empty;
					break;
				case Key.Enter:
					UpdateText();
					break;
				default:
					base.OnKeyDown( e );
					break;
			}
		}

		protected virtual void OnQueried( EventArgs e )
		{
			Queried( this, e );
		}

		public string CurrentQuery
		{
			get { return (string)GetValue( CurrentQueryProperty ); }
			set { SetValue( CurrentQueryProperty, value ); }
		}	public static readonly DependencyProperty CurrentQueryProperty = DependencyProperty.Register( "CurrentQuery", typeof(string), typeof(QueryTextBox), new PropertyMetadata( string.Empty, OnCurrentQueryChanged ) );

		static void OnCurrentQueryChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
		{
			d.As<QueryTextBox>( x => x.Dispatcher.BeginInvoke( () => x.OnQueried( EventArgs.Empty ) ) );
		}

		public Style WatermarkStyle
		{
			get { return (Style)GetValue( WatermarkStyleProperty ); }
			set { SetValue( WatermarkStyleProperty, value ); }
		}	public static readonly DependencyProperty WatermarkStyleProperty = DependencyProperty.Register( "WatermarkStyle", typeof(Style), typeof(QueryTextBox), null );

		public Duration SearchEventTimeDelay
		{
			get { return (Duration)GetValue( SearchEventTimeDelayProperty ); }
			set { SetValue( SearchEventTimeDelayProperty, value ); }
		}	public static readonly DependencyProperty SearchEventTimeDelayProperty = DependencyProperty.Register( "SearchEventTimeDelay", typeof(Duration), typeof(QueryTextBox), new PropertyMetadata( new Duration( new TimeSpan( 0, 0, 0, 0, 500 ) ), OnSearchEventTimeDelayChanged ) );
		
		static void OnSearchEventTimeDelayChanged( DependencyObject o, DependencyPropertyChangedEventArgs e )
		{
			o.As<QueryTextBox>( x =>
			{
				x.queryTimer.Interval = e.NewValue.To<Duration>().TimeSpan;
				x.queryTimer.Stop();
			} );
		}
	}
}
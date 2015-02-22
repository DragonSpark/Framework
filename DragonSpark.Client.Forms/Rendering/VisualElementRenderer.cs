using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DragonSpark.Extensions;
using Xamarin.Forms;
using Color = Xamarin.Forms.Color;
using Size = System.Windows.Size;

namespace DragonSpark.Application.Client.Forms.Rendering
{
	public class VisualElementRenderer<TElement, TNativeElement> : Canvas, IVisualElementRenderer where TElement : VisualElement where TNativeElement : FrameworkElement
	{
		readonly List<EventHandler<VisualElementChangedEventArgs>> elementChangedHandlers = new List<EventHandler<VisualElementChangedEventArgs>>();
		VisualElementTracker tracker;
		bool autoPackage = true;
		bool autoTrack = true;
		Brush initialBrush;
		public event EventHandler<ElementChangedEventArgs<TElement>> ElementChanged;

		event EventHandler<VisualElementChangedEventArgs> IVisualElementRenderer.ElementChanged
		{
			add { elementChangedHandlers.Add( value ); }
			remove { elementChangedHandlers.Remove( value ); }
		}

		public TNativeElement Control { get; private set; }

		public UIElement ContainerElement
		{
			get { return this; }
		}

		public TElement Element { get; set; }

		VisualElement IVisualElementRenderer.Element
		{
			get { return Element; }
		}

		VisualElementPackager Packager { get; set; }

		protected VisualElementTracker Tracker
		{
			get { return tracker; }
			set
			{
				if ( tracker == value )
				{
					return;
				}
				if ( tracker != null )
				{
					tracker.Dispose();
					tracker.Updated -= HandleTrackerUpdated;
				}
				tracker = value;
				if ( tracker != null )
				{
					tracker.Updated += HandleTrackerUpdated;
				}
			}
		}

		protected bool AutoPackage
		{
			get { return autoPackage; }
			set { autoPackage = value; }
		}

		protected bool AutoTrack
		{
			get { return autoTrack; }
			set { autoTrack = value; }
		}

		public void SetElement( VisualElement element )
		{
			var oldElement = Element;
			Element = (TElement)element;
			Element.PropertyChanged += OnElementPropertyChanged;
			Element.FocusChangeRequested += OnModelFocusChangeRequested;
			if ( AutoPackage && Packager == null )
			{
				Packager = new VisualElementPackager( this );
			}
			if ( AutoTrack && Tracker == null )
			{
				Tracker = new VisualElementTracker<TElement, FrameworkElement>
				{
					Model = Element,
					Element = this
				};
			}
			if ( Packager != null )
			{
				Packager.Load();
			}
			OnElementChanged( new ElementChangedEventArgs<TElement>( oldElement, Element ) );
		}

		protected virtual void OnElementChanged( ElementChangedEventArgs<TElement> e )
		{
			var e2 = new VisualElementChangedEventArgs( e.OldElement, e.NewElement );
			for ( var i = 0; i < elementChangedHandlers.Count; i++ )
			{
				elementChangedHandlers[i]( this, e2 );
			}
			var elementChanged = ElementChanged;
			if ( elementChanged != null )
			{
				elementChanged( this, e );
			}
		}

		internal void UnfocusControl( Control control )
		{
			if ( control == null || !control.IsEnabled )
			{
				return;
			}
			control.IsEnabled = false;
			control.IsEnabled = true;
		}

		internal virtual void OnModelFocusChangeRequested( object sender, VisualElement.FocusRequestArgs args )
		{
			var control = Control as Control;
			if ( control == null )
			{
				return;
			}
			if ( args.Focus )
			{
				args.Result = control.Focus();
				return;
			}
			UnfocusControl( control );
			args.Result = true;
		}

		public virtual SizeRequest GetDesiredSize( double widthConstraint, double heightConstraint )
		{
			if ( Children.Count == 0 )
			{
				return default( SizeRequest );
			}
			var availableSize = new Size( widthConstraint, heightConstraint );
			var frameworkElement = (FrameworkElement)Children[0];
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

		protected virtual void OnElementPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == VisualElement.IsEnabledProperty.PropertyName )
			{
				UpdateEnabled();
				return;
			}
			if ( e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName )
			{
				UpdateBackgroundColor();
			}
		}

		protected virtual void UpdateBackgroundColor()
		{
			var control = Control as Control;
			if ( initialBrush == null )
			{
				initialBrush = ( ( control == null ) ? Background : control.Background );
			}
			if ( control != null )
			{
				var arg_75_0 = control;
				var tElement = Element;
				Brush arg_75_1;
				if ( !( tElement.BackgroundColor != Color.Default ) )
				{
					arg_75_1 = initialBrush;
				}
				else
				{
					var tElement2 = Element;
					arg_75_1 = tElement2.BackgroundColor.ToBrush();
				}
				arg_75_0.Background = arg_75_1;
				return;
			}
			var tElement3 = Element;
			Brush arg_BE_1;
			if ( !( tElement3.BackgroundColor != Color.Default ) )
			{
				arg_BE_1 = initialBrush;
			}
			else
			{
				var tElement4 = Element;
				arg_BE_1 = tElement4.BackgroundColor.ToBrush();
			}
			Background = arg_BE_1;
		}

		void UpdateEnabled()
		{
			if ( Control is Control )
			{
				var arg_36_0 = Control as Control;
				var tElement = Element;
				arg_36_0.IsEnabled = tElement.IsEnabled;
			}
		}

		void HandleTrackerUpdated( object sender, EventArgs e )
		{
			UpdateNativeWidget();
		}

		protected virtual void UpdateNativeWidget()
		{
			UpdateEnabled();
		}

		protected void SetNativeControl( TNativeElement element )
		{
			Control = element;
			Children.Add( element );
			var tElement = Element;
			tElement.IsNativeStateConsistent = false;
			element.Loaded += delegate
			{
				var tElement2 = this.Element;
				tElement2.IsNativeStateConsistent = true;
			};
			
			element.GotFocus += OnGotFocus;
			element.LostFocus += OnLostFocus;
			tracker.Child = element;
			UpdateBackgroundColor();
		}

		protected virtual void OnGotFocus( object sender, RoutedEventArgs e )
		{
			this.Element.To<IElementController>().SetValueFromRenderer( VisualElement.IsFocusedPropertyKey, true );
		}

		protected virtual void OnLostFocus( object sender, RoutedEventArgs e )
		{
			this.Element.To<IElementController>().SetValueFromRenderer( VisualElement.IsFocusedPropertyKey, false );
		}
	}
}

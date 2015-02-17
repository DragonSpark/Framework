using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Xamarin.Forms;
using Color = Xamarin.Forms.Color;

namespace DragonSpark.Client.Windows.Forms.Rendering
{
	class BackgroundTracker<T> : BackgroundTracker<Page, T> where T : FrameworkElement
	{
		public BackgroundTracker( DependencyProperty backgroundProperty ) : base( backgroundProperty )
		{}

		public BackgroundTracker( T element, DependencyProperty backgroundProperty ) : base( element, backgroundProperty )
		{}
	}

	class BackgroundTracker<TPage, T> : VisualElementTracker<TPage, T> where T : FrameworkElement where TPage : Page
	{
		readonly DependencyProperty backgroundProperty;

		bool backgroundNeedsUpdate = true;

		public BackgroundTracker( DependencyProperty backgroundProperty ) : this( null, backgroundProperty )
		{}

		public BackgroundTracker( T element, DependencyProperty backgroundProperty )
		{
			if ( backgroundProperty == null )
			{
				throw new ArgumentNullException( "backgroundProperty" );
			}
			this.backgroundProperty = backgroundProperty;

			Element = element;
		}

		protected override void HandlePropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName || e.PropertyName == Page.BackgroundImageProperty.PropertyName )
			{
				UpdateBackground();
			}
			base.HandlePropertyChanged( sender, e );
		}

		void UpdateBackground()
		{
			if ( Model != null && Element != null )
			{
				if ( Model.BackgroundImage != null )
				{
					var element = Element;
					element.SetValue( backgroundProperty, new ImageBrush
					{
						ImageSource = new BitmapImage( new Uri( Model.BackgroundImage, UriKind.Relative ) )
					} );
				}
				else
				{
					if ( Model.BackgroundColor != Color.Default )
					{
						var element2 = Element;
						element2.SetValue( backgroundProperty, Model.BackgroundColor.ToBrush() );
					}
				}
				backgroundNeedsUpdate = false;
			}
		}

		protected override void UpdateNativeControl()
		{
			base.UpdateNativeControl();
			if ( backgroundNeedsUpdate )
			{
				UpdateBackground();
			}
		}
	}
}

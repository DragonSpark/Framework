using DragonSpark.Extensions;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using DragonSpark.Xamarin.Forms.Platform.Wpf.Application;
using Xamarin.Forms;
using Binding = System.Windows.Data.Binding;
using Page = Xamarin.Forms.Page;
using Size = Xamarin.Forms.Size;

namespace DragonSpark.Xamarin.Forms.Platform.Wpf.Rendering
{
	public class TabbedPageRenderer : TabControl, IVisualElementRenderer
	{
		static TabbedPageRenderer()
		{
			DefaultStyleKeyProperty.OverrideMetadata( typeof(TabbedPageRenderer), new FrameworkPropertyMetadata( typeof(TabbedPageRenderer) ) );
		}

		public event EventHandler<VisualElementChangedEventArgs> ElementChanged = delegate {};
		
		readonly BackgroundTracker<TabbedPage, TabControl> tracker;
		
		public UIElement ContainerElement
		{
			get { return this; }
		}

		public VisualElement Element
		{
			get { return tracker.Model; }
		}

		public TabbedPageRenderer()
		{
			tracker = new BackgroundTracker<TabbedPage, TabControl>( new TabControl(), BackgroundProperty );

			SetBinding( ShellProperties.TitleProperty, new Binding( "Title" ) );
			SetBinding( ItemsSourceProperty, new Binding( "Children" ) );
			
			// HeaderTemplate = (DataTemplate)Application.Current.Resources["TabbedPageHeader"];
			// base.ItemTemplate = (DataTemplate)Application.Current.Resources["TabbedPage"];
			// this.ItemTemplate
			SelectionChanged += OnSelectionChanged;
		}

		public SizeRequest GetDesiredSize( double widthConstraint, double heightConstraint )
		{
			return new SizeRequest( new Size( widthConstraint, heightConstraint ) );
		}

		public void SetElement( VisualElement element )
		{
			element.As<TabbedPage>( page =>
			{
				tracker.Model = page;
				DataContext = element;
				page.PropertyChanged += OnPropertyChanged;
				Loaded += delegate { tracker.Model.SendAppearing(); };
				Unloaded += delegate { tracker.Model.SendDisappearing(); };
				OnElementChanged( new VisualElementChangedEventArgs( page, element ) );
			} );
		}

		protected virtual void OnElementChanged( VisualElementChangedEventArgs e )
		{
			var elementChanged = ElementChanged;
			if ( elementChanged != null )
			{
				elementChanged( this, e );
			}
		}

		void OnSelectionChanged( object sender, SelectionChangedEventArgs e )
		{
			tracker.Model.CurrentPage = (Page)base.SelectedItem;
		}

		void OnPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == "CurrentPage" )
			{
				var currentPage = tracker.Model.CurrentPage;
				if ( currentPage != null )
				{
					base.SelectedItem = currentPage;
				}
			}
		}
	}
}

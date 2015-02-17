using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using DragonSpark.Extensions;
using Xamarin.Forms;
using Xceed.Wpf.Toolkit.Panels;
using Binding = System.Windows.Data.Binding;
using DataTemplate = System.Windows.DataTemplate;
using Page = Xamarin.Forms.Page;
using Size = Xamarin.Forms.Size;

namespace DragonSpark.Client.Windows.Forms.Rendering
{
	public class CarouselPageRenderer : Carousel, IVisualElementRenderer
	{
		public event EventHandler<VisualElementChangedEventArgs> ElementChanged = delegate {};

		readonly BackgroundTracker<CarouselPage, Panel> tracker;
		readonly ConditionMonitor assign = new ConditionMonitor();

		public CarouselPageRenderer()
		{
			tracker = new BackgroundTracker<CarouselPage, Panel>( this, BackgroundProperty );

			SetBinding( ShellProperties.TitleProperty, new Binding( "Title" ) );
		}

		public UIElement ContainerElement
		{
			get { return tracker.Element; }
		}

		public VisualElement Element
		{
			get { return tracker.Model; }
		}

		public SizeRequest GetDesiredSize( double widthConstraint, double heightConstraint )
		{
			return new SizeRequest( new Size( widthConstraint, heightConstraint ) );
		}

		static readonly BindableProperty PageContainerProperty = BindableProperty.CreateAttached( "PageContainer", typeof(CarouselPageItemContainer), typeof(CarouselPageRenderer), null );

		static CarouselPageItemContainer GetContainer( BindableObject bindable )
		{
			return (CarouselPageItemContainer)bindable.GetValue( PageContainerProperty );
		}

		static void SetContainer( BindableObject bindable, CarouselPageItemContainer container )
		{
			bindable.SetValue( PageContainerProperty, container );
		}

		public void SetElement( VisualElement element )
		{
			var old = tracker.Model;

			old.With( page =>
			{
				page.PagesChanged -= OnPagesChanged;
				page.PropertyChanged -= OnPropertyChanged;
				
			} );

			element.As<CarouselPage>( page =>
			{
				DataContext = tracker.Model = page;
			
				assign.Apply( () =>
				{
					ChildEntered += OnSelectionChanged;
					Loaded += ( sender, args ) => tracker.Model.SendAppearing();
					Unloaded += ( sender, args ) => tracker.Model.SendDisappearing();
				} );

				OnPagesChanged( page, new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Reset ) );
				
				page.PagesChanged += OnPagesChanged;
				page.PropertyChanged += OnPropertyChanged;
			} );
			
			OnElementChanged( new VisualElementChangedEventArgs( old, element ) );
		}

		void InsertItem( object item, int index, bool newItem )
		{
			item.As<Page>( page =>
			{
				var container = GetContainer( page ) ?? Create( page );
				Children.Insert( index, container );
			} );
		}

		CarouselPageItemContainer Create( Page page )
		{
			var result = ItemContainerTemplate.Transform( x => x.LoadContent() ) as CarouselPageItemContainer ?? new CarouselPageItemContainer();
			result.DataContext = result.Content = page;
			SetContainer( page, result );
			return result;
		}

		public DataTemplate ItemContainerTemplate
		{
			get { return GetValue( ItemContainerTemplateProperty ).To<DataTemplate>(); }
			set { SetValue( ItemContainerTemplateProperty, value ); }
		}	public static readonly DependencyProperty ItemContainerTemplateProperty = DependencyProperty.Register( "ItemContainerTemplate", typeof(DataTemplate), typeof(CarouselPageRenderer), null );

		protected virtual void OnElementChanged( VisualElementChangedEventArgs e )
		{
			ElementChanged( this, e );
		}

		void OnPagesChanged( object sender, NotifyCollectionChangedEventArgs e )
		{
			e.Apply( InsertItem, ( o, i ) => Children.RemoveAt( i ), Reset );
		}

		void Reset()
		{
			Children.Clear();
			
			var num = 0;
			tracker.Model.Children.Apply( current => InsertItem( current, num++, true ) );

			Select();
		}

		void OnSelectionChanged( object sender, EventArgs e )
		{
			tracker.Model.CurrentPage = CenteredChild.AsTo<CarouselPageItemContainer, ContentPage>( x => x.Content as ContentPage );
		}

		void Select( Page page = null )
		{
			var container = GetContainer( page ?? tracker.Model.CurrentPage );
			CenterChild( container, TimeSpan.FromSeconds( 1 ), false );
		}

		void OnPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch ( e.PropertyName )
			{
				case "CurrentPage":
					Select();
					break;
			}
		}
	}

	public class CarouselPageItemContainer : ContentControl
	{}
}

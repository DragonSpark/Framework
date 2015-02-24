using DragonSpark.Extensions;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

namespace DragonSpark.Application.Client.Controls
{
	[TemplatePart(Name = "LayoutTranslation", Type = typeof(TranslateTransform))]
	[TemplatePart(Name = "MenuItemPanel", Type = typeof(ItemsControl))]
	[TemplatePart(Name = "MenuButton", Type = typeof(Button))]
	public class ApplicationBar : ItemsControl
	{
		public static readonly DependencyProperty InstanceProperty = DependencyProperty.RegisterAttached( "Instance", typeof(ApplicationBar), typeof(ApplicationBar), new PropertyMetadata( OnInstancePropertyChanged ) );

		static void OnInstancePropertyChanged( DependencyObject o, DependencyPropertyChangedEventArgs e )
		{}

		public static ApplicationBar EnsureInstance( FrameworkElement element )
		{
			var result = GetInstance( element ) ?? new ApplicationBar().With( bar => SetInstance( element, bar ) );
			return result;
		}

		public static ApplicationBar GetInstance( FrameworkElement element )
		{
			return (ApplicationBar)element.GetValue( InstanceProperty );
		}

		public static void SetInstance( FrameworkElement element, ApplicationBar value )
		{
			element.SetValue( InstanceProperty, value );
		}

		TranslateTransform layoutTranslation;

		ItemsControl menuItemPanel;

		Button menuButton;

		/// <summary>
		///     Initializes a new Application Bar
		/// </summary>
		public ApplicationBar()
		{
			SizeChanged += AdjustHorizontalSize;
			Loaded += OnLoaded;
			Unloaded += OnUnloaded;
		}
		
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			menuItemPanel.With( x => x.LayoutUpdated -= AdjustHeightToShowOnlyIcons );
			menuButton.With( x => x.Click -= SwitchMenuItemVisibility );

			layoutTranslation = (TranslateTransform)GetTemplateChild( "LayoutTranslation" );

			menuItemPanel = (ItemsControl)GetTemplateChild( "MenuItemPanel" );
			menuItemPanel.LayoutUpdated += AdjustHeightToShowOnlyIcons;

			menuButton =  (Button)GetTemplateChild( "MenuButton" );
			menuButton.Click += SwitchMenuItemVisibility;

		}

		/*/// <summary>
		/// Unsubscribes the applicationbar from the event aggregator
		/// </summary>
		~ApplicationBar()
		{
			if (!ViewModelBase.IsDesignMode)
				Kernel.Instance.EventAggregator.Unsubscribe(this);
		}*/

		/// <summary>
		///     Initializes the navigation service
		/// </summary>
		/// <param name="sender">Sending UI Element</param>
		/// <param name="e">Routed Events</param>
		void OnLoaded( object sender, RoutedEventArgs e )
		{
			GetNavigationService().With( ns =>
			{
				ns.Navigating += HideOnNavigation;
				ns.Navigated += ShowWhenNavigated;
			} );
		}

		NavigationService GetNavigationService()
		{
			FrameworkElement element = this;

			NavigationService result;
			// Walk up, until we get an navigation service
			do
			{
				result = NavigationService.GetNavigationService( element );
				element = element.Parent as FrameworkElement;
			}
			while ( result == null && element != null );
			
			return result;
		}

		/// <summary>
		///     de-Register the navigation service
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnUnloaded( object sender, RoutedEventArgs e )
		{
			GetNavigationService().With( ns =>
			{
				ns.Navigating -= HideOnNavigation;
				ns.Navigated -= ShowWhenNavigated;
			} );
		}

		#region State Mangement
		/// <summary>
		///     Visibility of the menuItems
		/// </summary>
		Visibility menuItemVisibility = Visibility.Collapsed;

		/// <summary>
		///     True, if an animation is running
		/// </summary>
		bool isAnimating;

		/// <summary>
		///     True, on the first start - this forces the window to slide in
		/// </summary>
		bool firstStart = true;
		#endregion

		#region Animation Properties
		/// <summary>
		///     Sliding Animation
		/// </summary>
		DoubleAnimation slideAnimation;

		/// <summary>
		///     Duration of the Sliding Animation
		/// </summary>
		static readonly TimeSpan AnimationDuration = TimeSpan.FromMilliseconds( 200 );
		#endregion

		#region Menu Icons
		/*/// <summary>
		///     Gets or sets the Application Bar Icons
		/// </summary>
		[Category( "Behavior" ), Description( "Contains the icons shown in the application bar." )]
		public ObservableCollection<ApplicationBarIconButton> Icons { get; set; }*/

		/*/// <summary>
		///     This method will be called, if the icon collection changes
		/// </summary>
		/// <param name="sender">The sending ui element</param>
		/// <param name="e">Event arguments</param>
		void OnIconCollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
		{
			switch ( e.Action )
			{
				case NotifyCollectionChangedAction.Add:
					AddIcons( e.NewStartingIndex, e.NewItems.Cast<ApplicationBarIconButton>() );
					break;
				case NotifyCollectionChangedAction.Remove:
					RemoveIcons( e.OldItems.Cast<ApplicationBarIconButton>() );
					break;
				case NotifyCollectionChangedAction.Replace:
					throw new NotImplementedException( "Replace is not implemented" );
				case NotifyCollectionChangedAction.Move:
					throw new NotImplementedException( "Move is not implemented" );
				case NotifyCollectionChangedAction.Reset:
					ResetIcons();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}*/

		/// <summary>
		///     Reset all icons
		/// </summary>
		/*void ResetIcons()
		{
			// Reset all icon panel childs
			iconPanel.Children.Clear();
		}*/

		/// <summary>
		///     Gets or sets the Data Context
		/// </summary>
		/*public new object DataContext
		{
			get { return base.DataContext; }
			set
			{
				base.DataContext = value;

				foreach ( var icon in Icons )
				{
					icon.DataContext = value;
				}

				foreach ( var menuItem in MenuItems )
				{
					menuItem.DataContext = value;
				}
			}
		}*/

		/*/// <summary>
		///     Add new Icons
		/// </summary>
		/// <param name="newStartingIndex">the starting index to add the icons</param>
		/// <param name="newItems">Enumerable collection of new application bar icons to add</param>
		void AddIcons( int newStartingIndex, IEnumerable<ApplicationBarIconButton> newItems )
		{
			foreach ( var newItem in newItems )
			{
				// Set the DataContext of the child element
				newItem.DataContext = newItem.DataContext ?? DataContext;

				var stack = new StackPanel { Orientation = Orientation.Vertical, Tag = newItem };

				// Button 
				var content = new Image
				{
					Stretch = Stretch.UniformToFill,
					Width = (Double)TryFindResource( "ApplicationBarIconSize" ),
					Height = (Double)TryFindResource( "ApplicationBarIconSize" )
				};

				var button = new Button
				{
					Style = (Style)TryFindResource( "ChromeButtonStyle" ),
					Margin = new Thickness( 12, 3, 12, 3 ),
					Content = content,
					Focusable = newItem.Focusable,
					IsDefault = newItem.IsDefault,
					IsCancel = newItem.IsCancel
				};
				stack.Children.Add( button );

				// Description text
				var textBlock = new TextBlock
				{
					Text = newItem.Description,
					HorizontalAlignment = HorizontalAlignment.Center,
					Style = (Style)TryFindResource( "WinTextSmallStyle" ),
					Focusable = newItem.Focusable
				};
				stack.Children.Add( textBlock );

				// Add bindings
				var binding = new Binding { Source = newItem, Path = new PropertyPath( ApplicationBarIconButton.CommandProperty ) };
				button.SetBinding( ButtonBase.CommandProperty, binding );

				binding = new Binding { Source = newItem, Path = new PropertyPath( ApplicationBarIconButton.DescriptionProperty ) };
				textBlock.SetBinding( TextBlock.TextProperty, binding );

				binding = new Binding { Source = newItem, Path = new PropertyPath( ApplicationBarIconButton.ImageSourceProperty ) };
				content.SetBinding( Image.SourceProperty, binding );

				binding = new Binding { Source = newItem, Path = new PropertyPath( ApplicationBarIconButton.IsDefaultProperty ) };
				button.SetBinding( Button.IsDefaultProperty, binding );

				binding = new Binding { Source = newItem, Path = new PropertyPath( ApplicationBarIconButton.IsCancelProperty ) };
				button.SetBinding( Button.IsCancelProperty, binding );

				binding = new Binding { Source = newItem, Path = new PropertyPath( VisibilityProperty ) };
				stack.SetBinding( VisibilityProperty, binding );

				// Add it to the collection
				iconPanel.Children.Insert( newStartingIndex, stack );
				newStartingIndex++;
			}
		}*/

		/*/// <summary>
		///     Add new Icons
		/// </summary>
		/// <param name="oldItems">Enumerable collection of new application bar icons to remove</param>
		void RemoveIcons( IEnumerable<ApplicationBarIconButton> oldItems )
		{
			var removeableIcons = iconPanel.Children.Cast<FrameworkElement>()
				.Join( oldItems,
					element => element.Tag, oldItem => oldItem,
					( element, oldItem ) => element ).ToList();

			// Remove it from the iconpanel
			foreach ( var toRemove in removeableIcons )
			{
				iconPanel.Children.Remove( toRemove );
			}
		}*/
		#endregion

		#region Menu Items
		/*/// <summary>
		///     Gets or sets the Application Bar Icons
		/// </summary>
		[Category( "Behavior" ), Description( "Contains the menu items shown in the application bar." )]
		public ObservableCollection<ApplicationBarMenuItem> MenuItems { get; set; }*/
/*
		/// <summary>
		///     This method will be called, if the MenuItem collection changes
		/// </summary>
		/// <param name="sender">The sending ui element</param>
		/// <param name="e">Event arguments</param>
		void OnMenuItemCollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
		{
			switch ( e.Action )
			{
				case NotifyCollectionChangedAction.Add:
					AddMenuItems( e.NewStartingIndex, e.NewItems.Cast<ApplicationBarMenuItem>() );
					break;
				case NotifyCollectionChangedAction.Remove:
					RemoveMenuItems( e.OldItems.Cast<ApplicationBarMenuItem>() );
					break;
				case NotifyCollectionChangedAction.Replace:
					throw new NotImplementedException( "Replace is not implemented" );
				case NotifyCollectionChangedAction.Move:
					throw new NotImplementedException( "Move is not implemented" );
				case NotifyCollectionChangedAction.Reset:
					ResetMenuItems();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		/// <summary>
		///     Reset all MenuItems
		/// </summary>
		void ResetMenuItems()
		{
			// Reset all MenuItem panel childs
			menuItemPanel.Children.Clear();
		}*/

		public DataTemplate MenuItemTemplate
		{
			get { return GetValue( MenuItemTemplateProperty ).To<DataTemplate>(); }
			set { SetValue( MenuItemTemplateProperty, value ); }
		}	public static readonly DependencyProperty MenuItemTemplateProperty = DependencyProperty.Register( "MenuItemTemplate", typeof(DataTemplate), typeof(ApplicationBar), null );

		public IEnumerable MenuItemsSource
		{
			get { return GetValue( MenuItemsSourceProperty ).To<IEnumerable>(); }
			set { SetValue( MenuItemsSourceProperty, value ); }
		}	public static readonly DependencyProperty MenuItemsSourceProperty = DependencyProperty.Register( "MenuItemsSource", typeof(IEnumerable), typeof(ApplicationBar), null );

		/*/// <summary>
		///     Add new MenuItems
		/// </summary>
		/// <param name="newStartingIndex">the starting index to add the MenuItems</param>
		/// <param name="newItems">Enumerable collection of new application bar MenuItems to add</param>
		void AddMenuItems( int newStartingIndex, IEnumerable<ApplicationBarMenuItem> newItems )
		{
			foreach ( var newItem in newItems )
			{
				// Set the DataContext of the child element
				newItem.DataContext = newItem.DataContext ?? DataContext;

				// Button 
				var content = new TextBlock
				{
					HorizontalAlignment = HorizontalAlignment.Left,
					Style = (Style)TryFindResource( "WinTextTitle3Style" )
				};

				var button =
					new Button
					{
						HorizontalAlignment = HorizontalAlignment.Left,
						Tag = newItem,
						Style = (Style)TryFindResource( "ChromeButtonStyle" ),
						Margin = (Thickness)TryFindResource( "WinVerticalMargin" ),
						Content = content
					};

				// Add bindings
				var binding = new Binding { Source = newItem, Path = new PropertyPath( ApplicationBarMenuItem.CommandProperty ) };
				button.SetBinding( ButtonBase.CommandProperty, binding );

				binding = new Binding { Source = newItem, Path = new PropertyPath( ApplicationBarMenuItem.DescriptionProperty ) };
				content.SetBinding( TextBlock.TextProperty, binding );

				binding = new Binding { Source = newItem, Path = new PropertyPath( VisibilityProperty ) };
				button.SetBinding( VisibilityProperty, binding );

				// Add it to the collection
				menuItemPanel.Children.Insert( newStartingIndex, button );
				newStartingIndex++;
			}
		}

		/// <summary>
		///     Add new MenuItems
		/// </summary>
		/// <param name="oldItems">Enumerable collection of new application bar MenuItems to remove</param>
		void RemoveMenuItems( IEnumerable<ApplicationBarMenuItem> oldItems )
		{
			var removeableMenuItems = menuItemPanel.Children.Cast<FrameworkElement>()
				.Join( oldItems,
					element => element.Tag, oldItem => oldItem,
					( element, oldItem ) => element ).ToList();

			// Remove it from the MenuItempanel
			foreach ( var toRemove in removeableMenuItems )
			{
				menuItemPanel.Children.Remove( toRemove );
			}
		}*/
		#endregion

		#region Translation Methods
		/// <summary>
		///     Adjust the height of the control, so that only the icons are visible
		/// </summary>
		void AdjustHeightToShowOnlyIcons( object sender, EventArgs eventArgs )
		{
			// If an animation is already running, than don't disturbe
			if ( !isAnimating )
			{
				if ( firstStart )
				{
					layoutTranslation.Y = ActualHeight;
					firstStart = false;
				}

				// Calculate the new Y translation
				var menuItemHeight = 0.0;
				double newYPosition;
				RectangleGeometry newClipping;
				switch ( menuItemVisibility )
				{
					case Visibility.Visible:
						newYPosition = 0;
						newClipping = new RectangleGeometry( new Rect( 0, 0, ActualWidth, ActualHeight ) );
						break;
					case Visibility.Hidden:
						newYPosition = ActualHeight;
						newClipping = new RectangleGeometry( new Rect( 0, 0, ActualWidth, ActualHeight ) );
						break;
					case Visibility.Collapsed:
						newYPosition = menuItemHeight = menuItemPanel.ActualHeight;
						newClipping = new RectangleGeometry( new Rect( 0, 0, ActualWidth, ActualHeight - ( menuItemPanel.ActualHeight > 0 ? 1 : 0 ) ) );
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}

				// Check, if the NewYPosition changes to the current one
				if ( newYPosition != layoutTranslation.Y )
				{
					Clip = newClipping;

					// Create new animation and start it
					isAnimating = true;
					slideAnimation = new DoubleAnimation( newYPosition, new Duration( AnimationDuration ) )
					{
						EasingFunction = new ExponentialEase
						{
							EasingMode = EasingMode.EaseOut
						}
					};

					slideAnimation.Completed += (EventHandler)
						( ( s, e ) =>
						{
							Dispatcher.Invoke( (Action)( () => Clip = new RectangleGeometry( new Rect( 0, 0, ActualWidth, ActualHeight - menuItemHeight ) ) ) );
							isAnimating = false;
						} );
					layoutTranslation.BeginAnimation( TranslateTransform.YProperty, slideAnimation );
				}
			}

			// Hide the window on the first start

			// Set the new Clipping Geometry
		}

		/// <summary>
		///     Adjusts the Width in order to fill the window size
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void AdjustHorizontalSize( object sender, SizeChangedEventArgs e )
		{
			if ( e.WidthChanged )
			{
				Clip = new RectangleGeometry( new Rect( 0, 0, e.NewSize.Width, Clip != null ? Clip.Bounds.Height : e.NewSize.Height ) );
			}
		}
		#endregion

		#region Behaviour Implementation
		/// <summary>
		///     Switches the visiblity of the menu items
		/// </summary>
		void SwitchMenuItemVisibility( object sender, RoutedEventArgs e )
		{
			switch ( menuItemVisibility )
			{
				case Visibility.Visible:
					menuItemVisibility = Visibility.Collapsed;
					break;
				case Visibility.Hidden:
					break;
				case Visibility.Collapsed:
					menuItemVisibility = Visibility.Visible;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			AdjustHeightToShowOnlyIcons( sender, e );
		}

		/// <summary>
		///     This method show the pane when navigation has been finished
		/// </summary>
		void ShowWhenNavigated( object sender, NavigationEventArgs e )
		{
			menuItemVisibility = Visibility.Collapsed;
		}

		/// <summary>
		///     This method hides the pane on navigation
		/// </summary>
		void HideOnNavigation( object sender, NavigatingCancelEventArgs e )
		{
			menuItemVisibility = Visibility.Hidden;
		}
		#endregion

		/*/// <summary>
		/// Handles a message of a specific type.
		/// </summary>
		/// <param name="message">the message to handle</param>
		public void Handle(HideMenuItemsRequest message)
		{
			menuItemVisibility = Visibility.Collapsed;
			AdjustHeightToShowOnlyIcons(this, null);
		}*/
	}
}
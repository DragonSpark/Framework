using DragonSpark.Client.Windows.Compensations.Rendering;
using DragonSpark.Extensions;
using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Xamarin.Forms;
using Page = Xamarin.Forms.Page;

namespace DragonSpark.Client.Windows.Compensations.Application
{
	public class ShellBehavior : System.Windows.Interactivity.Behavior<FrameworkElement>
	{
		protected override void OnAttached()
		{
			base.OnAttached();
			AssociatedObject.SizeChanged += RendererSizeChanged;
		}

		void RendererSizeChanged( object sender, SizeChangedEventArgs e )
		{
			throw new NotImplementedException();
		}
	}

	class Engine : IPlatformEngine
	{
		public SizeRequest GetNativeSize( VisualElement view, double widthConstraint, double heightConstraint )
		{
			var result = widthConstraint > 0.0 && heightConstraint > 0.0 ? view.GetRenderer().Transform( x => x.GetDesiredSize( widthConstraint, heightConstraint ) ) : default(SizeRequest);
			return result;
		}

		public bool Supports3D
		{
			get { return true; }
		}
	}

	public class ApplicationHost : IPlatform
	{
		public event EventHandler BindingContextChanged = delegate {};

		readonly IPlatformEngine engine;
		readonly global::Xamarin.Forms.Application application;

		public ApplicationHost( IPlatformEngine engine, global::Xamarin.Forms.Application application )
		{
			this.engine = engine;
			this.application = application;
		}

		public void SetPage( Page newRoot )
		{
			if ( newRoot != Page )
			{
				application.MainPage = newRoot;
			}
		}

		public Page Page
		{
			get { return application.MainPage; }
		}

		public object BindingContext
		{
			get { return application.BindingContext; }
			set
			{
				if ( application.BindingContext != value )
				{
					application.BindingContext = value;
					BindingContextChanged( this, EventArgs.Empty );
				}
			}
		}

		public IPlatformEngine Engine
		{
			get { return engine; }
		}
	}

	class Navigation : INavigation
	{
		readonly IPlatform platform;
		readonly NavigationModel model;

		public Navigation( IPlatform platform, NavigationModel model )
		{
			this.platform = platform;
			this.model = model;
		}

		public IReadOnlyList<Page> NavigationStack
		{
			get { return model.Tree.Last(); }
		}

		public IReadOnlyList<Page> ModalStack
		{
			get { return model.Roots.ToList(); }
		}

		public void RemovePage( Page page )
		{
			if ( model.CurrentPage != page )
			{
				model.RemovePage( page );
			}
			else
			{
				PopAsync();
			}
		}

		public void InsertPageBefore( Page page, Page before )
		{
			model.InsertPageBefore( page, before );
		}

		public Task PushModalAsync( Page modal )
		{
			return PushModalAsync( modal, true );
		}

		public Task PushModalAsync( Page modal, bool animated )
		{
			var tcs = new TaskCompletionSource<object>();
			model.PushModal( modal );
			SetCurrent( model.CurrentPage, animated, false, () => tcs.SetResult( null ) );
			modal.NavigationProxy.Inner = this;
			return tcs.Task;
		}

		public Task<Page> PopModalAsync()
		{
			return PopModalAsync( true );
		}

		public Task<Page> PopModalAsync( bool animated )
		{
			var source = new TaskCompletionSource<Page>();
			var result = model.PopModal();
			SetCurrent( model.CurrentPage, animated, true, () => source.SetResult( result ) );
			return source.Task;
		}

		public Task PushAsync( Page root )
		{
			return PushAsync( root, true );
		}

		public Task PushAsync( Page root, bool animated )
		{
			Push( root, platform.Page, animated );
			return Task.FromResult( root );
		}

		public void Push( Page root, Page ancester, bool animated )
		{
			model.Push( root, ancester );
			SetCurrent( model.CurrentPage, animated );
			if ( root.NavigationProxy.Inner == null )
			{
				root.NavigationProxy.Inner = this;
			}
		}

		public Task<Page> PopAsync()
		{
			return PopAsync( true );
		}

		public Task<Page> PopAsync( bool animated )
		{
			return Task.FromResult( Pop( platform.Page, animated ) );
		}

		public Page Pop( Page ancestor, bool animated )
		{
			var result = model.Pop( ancestor );
			SetCurrent( model.CurrentPage, animated, true );
			return result;
		}

		public Task PopToRootAsync()
		{
			return PopToRootAsync( true );
		}

		public Task PopToRootAsync( bool animated )
		{
			model.PopToRoot( platform.Page );
			SetCurrent( model.CurrentPage, animated, true );
			return Task.FromResult( model.CurrentPage );
		}

		void SetCurrent( Page page, bool animated, bool popping = false, Action completedCallback = null )
		{
			/*if ( page != currentDisplayedPage )
			{
				page.Platform = platform;
				if ( page.GetRenderer() == null )
				{
					page.SetRenderer( RendererFactory.GetRenderer( page ) );
				}
				page.Layout( new Rectangle( 0.0, 0.0, renderer.ActualWidth, renderer.ActualHeight ) );
				if ( popping )
				{
					var self = currentDisplayedPage;
					var currentElement = (UIElement)self.GetRenderer();
					renderer.Children.Remove( currentElement );
					var previous = page;
					// UpdateToolbarTracker();
					renderer.Children.Add( (UIElement)previous.GetRenderer() );
					if ( completedCallback != null )
					{
						completedCallback();
					}
				}
				else
				{
					var page2 = currentDisplayedPage;
					if ( page2 != null )
					{
						var previousElement = (UIElement)page2.GetRenderer();
						renderer.Children.Remove( previousElement );
					}
					renderer.Children.Add( (UIElement)page.GetRenderer() );
					// UpdateToolbarTracker();
					if ( completedCallback != null )
					{
						completedCallback();
					}
					//}
				}
				currentDisplayedPage = page;
			}*/
		}
	}

	public static class ShellProperties
	{
		public static readonly DependencyProperty TitleProperty = DependencyProperty.RegisterAttached( "Title", typeof(string), typeof(ShellProperties), new PropertyMetadata( OnTitlePropertyChanged ) );

		static void OnTitlePropertyChanged( DependencyObject o, DependencyPropertyChangedEventArgs e )
		{}

		public static string GetTitle( FrameworkElement element )
		{
			return (string)element.GetValue( TitleProperty );
		}

		public static void SetTitle( FrameworkElement element, string value )
		{
			element.SetValue( TitleProperty, value );
		}

		public static readonly DependencyProperty DialogProperty = DependencyProperty.RegisterAttached( "Dialog", typeof(UIElement), typeof(ShellProperties), new PropertyMetadata( OnDialogPropertyChanged ) );

		static void OnDialogPropertyChanged( DependencyObject o, DependencyPropertyChangedEventArgs e )
		{}

		public static Window GetDialog( UIElement element )
		{
			return (Window)element.GetValue( DialogProperty );
		}

		public static void SetDialog( UIElement element, Window value )
		{
			element.SetValue( DialogProperty, value );
		}
	}


	public class ApplicationInteraction
	{
		readonly ToolbarTracker tracker = new ToolbarTracker();

		public ApplicationInteraction()
		{
			// page.BackKeyPress += new EventHandler<CancelEventArgs>( OnBackKeyPress );
			tracker.CollectionChanged += ( sender, args ) => UpdateToolbarItems();
			// ProgressIndicator indicator;
			/*SystemTray.SetProgressIndicator( page, indicator = new ProgressIndicator
			{
				IsVisible = false,
				IsIndeterminate = true
			} );*/
			/*var busyCount = 0;
			MessagingCenter.Subscribe( this, "Xamarin.BusySet", delegate( Page sender, bool enabled )
			{
				busyCount = Math.Max( 0, enabled ? ( busyCount + 1 ) : ( busyCount - 1 ) );
				indicator.IsVisible = ( busyCount > 0 );
			}, null );*/
			MessagingCenter.Subscribe( this, "Xamarin.SendAlert", delegate( Page sender, AlertArguments arguments )
			{
				var dialog = new ModernDialog
				{
					Title = arguments.Title,
					Content = arguments.Message
				};

				arguments.Accept.With( accept => dialog.OkButton.Content = accept );
				dialog.CancelButton.Content = arguments.Cancel;
				dialog.Buttons = new[] { arguments.Accept != null ? dialog.OkButton : null, dialog.CancelButton }.NotNull().ToArray();
				dialog.Show();

				var shell = System.Windows.Application.Current.GetShell();

				ShellProperties.SetDialog( shell, dialog );

				dialog.Closed += ( o, args ) =>
				{
					arguments.SetResult( dialog.MessageBoxResult == MessageBoxResult.OK );
					ShellProperties.SetDialog( shell, null );
				};
			} );

			MessagingCenter.Subscribe( this, "Xamarin.ShowActionSheet", ( Page sender, ActionSheetArguments arguments ) =>
			{
				var list = new List<string>();
				if ( !string.IsNullOrWhiteSpace( arguments.Destruction ) )
				{
					list.Add( arguments.Destruction );
				}
				list.AddRange( arguments.Buttons );
				if ( !string.IsNullOrWhiteSpace( arguments.Cancel ) )
				{
					list.Add( arguments.Cancel );
				}
				var listBox = new ListBox
				{
					FontSize = 36.0,
					Margin = new System.Windows.Thickness( 12.0 ), ItemsSource = list.Select( s => new TextBlock
					{
						Text = s,
						Margin = new System.Windows.Thickness( 0.0, 12.0, 0.0, 12.0 )
					} )
				};
				var dialog = new ModernDialog
				{
					Title = arguments.Title, Content = listBox
				};
				listBox.SelectionChanged += ( o, args ) => dialog.Close();

				var shell = System.Windows.Application.Current.GetShell();
				dialog.Closed +=  ( o, args ) =>
				{
					var result = ( (TextBlock)listBox.SelectedItem ).Text;
					arguments.SetResult( result );
					ShellProperties.SetDialog( shell, null );
				};
				dialog.Show();
				ShellProperties.SetDialog( shell, dialog );
			} );
		}

		void UpdateToolbarTracker()
		{
			// navigationModel.Roots.LastOrDefault().With( x => tracker.Target = x );
		}

		/*class TaggedAppBarButton : ApplicationBarIconButton, IDisposable
		{
			object tag;
			bool disposed;

			public object Tag
			{
				get { return tag; }
				set
				{
					if ( tag == null && value is ToolbarItem )
					{
						( (ToolbarItem)value ).PropertyChanged += TaggedAppBarButton_PropertyChanged;
					}
					tag = value;
				}
			}

			void TaggedAppBarButton_PropertyChanged( object sender, PropertyChangedEventArgs e )
			{
				if ( e.PropertyName == MenuItem.IsEnabledProperty.PropertyName )
				{
					var toolbarItem = Tag as ToolbarItem;
					if ( toolbarItem != null )
					{
						base.IsEnabled = toolbarItem.IsEnabled;
					}
				}
			}

			public void Dispose()
			{
				if ( disposed )
				{
					return;
				}
				disposed = true;
				var item = Tag as ToolbarItem;
				if ( item != null )
				{
					item.PropertyChanged -= TaggedAppBarButton_PropertyChanged;
				}
			}
		}

		class TaggedAppBarMenuItem : ApplicationBarMenuItem
		{
			public object Tag { get; set; }
		}*/

		/*void OnBackKeyPress( object sender, CancelEventArgs e )
		{
			if ( visibleMessageBox != null )
			{
				visibleMessageBox.CloseCommand.Execute( null );
				e.Cancel = true;
				return;
			}
			var page = navModel.Roots.Last();
			var flag = page.SendBackButtonPressed();
			if ( !flag && navModel.Tree.Count > 1 )
			{
				var page2 = navModel.PopModal();
				if ( page2 != null )
				{
					SetCurrent( navModel.CurrentPage, true, true );
					flag = true;
				}
			}
			e.Cancel = flag;
		}*/

		void UpdateToolbarItems()
		{
			/*if ( page.ApplicationBar == null )
			{
				page.ApplicationBar = new ApplicationBar();
			}
			var items = tracker.ToolbarItems.ToArray();
			var masterDetail = tracker.Target.Descendants().Prepend( tracker.Target ).OfType<MasterDetailPage>().FirstOrDefault();
			var taggedAppBarButton = Enumerable.OfType<TaggedAppBarButton>( page.ApplicationBar.Buttons ).FirstOrDefault( ( TaggedAppBarButton b ) => b.Tag is MasterDetailPage && b.Tag != masterDetail );
			if ( taggedAppBarButton != null )
			{
				page.ApplicationBar.Buttons.Remove( taggedAppBarButton );
			}
			if ( masterDetail != null && masterDetail.ShouldShowToolbarButton() )
			{
				if ( Enumerable.OfType<TaggedAppBarButton>( page.ApplicationBar.Buttons ).All( ( TaggedAppBarButton b ) => b.Tag != masterDetail ) )
				{
					var taggedAppBarButton2 = new TaggedAppBarButton
					{
						IconUri = new Uri( masterDetail.Master.Icon ?? "ApplicationIcon.jpg", UriKind.Relative ),
						Text = masterDetail.Master.Title,
						IsEnabled = true,
						Tag = masterDetail
					};
					taggedAppBarButton2.Click += delegate( object sender, EventArgs args )
					{
						var masterDetailRenderer = masterDetail.GetRenderer() as MasterDetailRenderer;
						if ( masterDetailRenderer != null )
						{
							masterDetailRenderer.Toggle();
						}
					};
					page.ApplicationBar.Buttons.Add( taggedAppBarButton2 );
				}
			}
			var list = new List<TaggedAppBarButton>();
			using ( var enumerator = items.Where( ( ToolbarItem i ) => i.Order != ToolbarItemOrder.Secondary ).GetEnumerator() )
			{
				while ( enumerator.MoveNext() )
				{
					var item = enumerator.Current;
					if ( !Enumerable.OfType<TaggedAppBarButton>( page.ApplicationBar.Buttons ).Any( ( TaggedAppBarButton b ) => b.Tag == item ) )
					{
						var taggedAppBarButton3 = new TaggedAppBarButton
						{
							IconUri = new Uri( item.Icon ?? "ApplicationIcon.jpg", UriKind.Relative ),
							Text = ( !string.IsNullOrWhiteSpace( item.Name ) ) ? item.Text : ( item.Icon ?? "ApplicationIcon.jpg" ),
							IsEnabled = item.IsEnabled,
							Tag = item
						};
						taggedAppBarButton3.Click += delegate( object sender, EventArgs args ) { item.Activate(); };
						list.Add( taggedAppBarButton3 );
					}
				}
			}
			var list2 = new List<TaggedAppBarMenuItem>();
			using ( var enumerator2 = items.Where( ( ToolbarItem i ) => i.Order == ToolbarItemOrder.Secondary ).GetEnumerator() )
			{
				while ( enumerator2.MoveNext() )
				{
					var item = enumerator2.Current;
					if ( !Enumerable.OfType<TaggedAppBarMenuItem>( page.ApplicationBar.MenuItems ).Any( ( TaggedAppBarMenuItem b ) => b.Tag == item ) )
					{
						var taggedAppBarMenuItem = new TaggedAppBarMenuItem
						{
							Text = ( !string.IsNullOrWhiteSpace( item.Name ) ) ? item.Text : ( item.Icon ?? "MenuItem" ),
							IsEnabled = true,
							Tag = item
						};
						taggedAppBarMenuItem.Click += delegate( object sender, EventArgs args ) { item.Activate(); };
						list2.Add( taggedAppBarMenuItem );
					}
				}
			}
			var array = Enumerable.OfType<TaggedAppBarButton>( page.ApplicationBar.Buttons ).Where( ( TaggedAppBarButton b ) => b.Tag is ToolbarItem && !items.Contains( b.Tag ) ).ToArray();
			var array2 = Enumerable.OfType<TaggedAppBarMenuItem>( page.ApplicationBar.MenuItems ).Where( ( TaggedAppBarMenuItem b ) => b.Tag is ToolbarItem && !items.Contains( b.Tag ) ).ToArray();
			var array3 = array;
			for ( var k = 0; k < array3.Length; k++ )
			{
				var taggedAppBarButton4 = array3[k];
				taggedAppBarButton4.Dispose();
				page.ApplicationBar.Buttons.Remove( taggedAppBarButton4 );
			}
			var array4 = array2;
			for ( var j = 0; j < array4.Length; j++ )
			{
				var value = array4[j];
				page.ApplicationBar.MenuItems.Remove( value );
			}
			foreach ( var current in list )
			{
				page.ApplicationBar.Buttons.Add( current );
			}
			foreach ( var current2 in list2 )
			{
				page.ApplicationBar.MenuItems.Add( current2 );
			}
			page.ApplicationBar.IsVisible = ( page.ApplicationBar.Buttons.Count > 0 || page.ApplicationBar.MenuItems.Count > 0 );*/
		}
	}

	class ApplicationModel
	{
		readonly global::Xamarin.Forms.Application application;

		public ApplicationModel( global::Xamarin.Forms.Application application )
		{
			/*PhoneApplicationService.Current.Launching += OnLaunching;
			PhoneApplicationService.Current.Activated += OnActivated;
			PhoneApplicationService.Current.Deactivated += OnDeactivated;
			PhoneApplicationService.Current.Closing += OnClosing;*/
			
			this.application = global::Xamarin.Forms.Application.Current = application;
			application.PropertyChanged += ApplicationOnPropertyChanged;
			application.SendStart();
			SetMainPage();
		}

		void ApplicationOnPropertyChanged( object sender, PropertyChangedEventArgs args )
		{
			switch ( args.PropertyName )
			{
				case "MainPage":
					SetMainPage();
					break;
			}
		}

		void OnLaunching( object sender, EventArgs e )
		{
			application.SendStart();
		}

		void OnActivated( object sender, EventArgs e )
		{
			application.SendResume();
		}

		void OnDeactivated( object sender, EventArgs e )
		{
			application.SendSleepAsync().Wait();
		}

		void OnClosing( object sender, EventArgs e )
		{
			application.SendSleepAsync().Wait();

		}

		void SetMainPage()
		{
		}
	}
}

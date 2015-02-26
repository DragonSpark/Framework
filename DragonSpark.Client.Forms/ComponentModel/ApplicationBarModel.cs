using DragonSpark.Application.Client.Presentation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace DragonSpark.Application.Client.Forms.ComponentModel
{
	public class ApplicationBarModel : ViewObject
	{
		readonly ToolbarTracker tracker = new ToolbarTracker();

		public ApplicationBarModel()
		{
			items = new ReadOnlyObservableCollection<ToolbarItem>( itemsSource );
			// page.BackKeyPress += new EventHandler<CancelEventArgs>( OnBackKeyPress );
			tracker.CollectionChanged += ( sender, args ) => UpdateToolbarItems();

			this.Event<ShellPageChangedEvent>().Subscribe( this, page => tracker.Target = page );

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
			/*MessagingCenter.Subscribe( this, "Xamarin.SendAlert", delegate( Page sender, AlertArguments arguments )
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

				dialog.Closed +=  ( o, args ) =>
				{
					var result = ( (TextBlock)listBox.SelectedItem ).Text;
					arguments.SetResult( result );
					ShellProperties.SetDialog( shell, null );
				};
				dialog.Show();
				ShellProperties.SetDialog( shell, dialog );
			} );*/
		}

		/*class TaggedAppBarButton : ApplicationBarIconButton, IDisposable
		{
			object tag;
			bool disposed;

			/*public object Tag
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
			}#1#

			void TaggedAppBarButton_PropertyChanged( object sender, PropertyChangedEventArgs e )
			{
				ToolbarItem toolbarItem = this.Tag as ToolbarItem;
				if (toolbarItem == null)
				{
					return;
				}
				if (e.PropertyName == Xamarin.Forms.MenuItem.IsEnabledProperty.PropertyName)
				{
					base.IsEnabled = toolbarItem.IsEnabled;
					return;
				}
				if (e.PropertyName == Xamarin.Forms.MenuItem.TextProperty.PropertyName)
				{
					base.Description = !string.IsNullOrWhiteSpace(toolbarItem.Text) ? toolbarItem.Text : (string)(toolbarItem.Icon ?? "ApplicationIcon.jpg");
					return;
				}
				if (e.PropertyName == Xamarin.Forms.MenuItem.IconProperty.PropertyName)
				{
					ImageSource = new BitmapImage(new Uri(toolbarItem.Icon ?? "ApplicationIcon.jpg", UriKind.Relative));
				}
			}

			public void Dispose()
			{
				if ( !disposed )
				{
					disposed = true;
					var item = Tag as ToolbarItem;
					if ( item != null )
					{
						item.PropertyChanged -= TaggedAppBarButton_PropertyChanged;
					}
				}
			}
		}*/

		/*class TaggedAppBarMenuItem : ApplicationBarMenuItem
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

		public IReadOnlyCollection<ToolbarItem> Items
		{
			get { return items; }
		}	readonly IReadOnlyCollection<ToolbarItem> items;
		readonly ViewCollection<ToolbarItem> itemsSource = new ViewCollection<ToolbarItem>();

		public IEnumerable<ToolbarItem> Primary
		{
			get { return Items.Where( item => item.Order < ToolbarItemOrder.Secondary ).ToArray(); }
		}

		public IEnumerable<ToolbarItem> Secondary
		{
			get { return Items.Where( item => item.Order == ToolbarItemOrder.Secondary ).ToArray(); }
		}	

		void UpdateToolbarItems()
		{
			itemsSource.Clear();

			/*var master = tracker.Target.Descendants().Prepend( tracker.Target ).FirstOrDefaultOfType<MasterDetailPage>().Transform( page => page.ShouldShowToolbarButton() )
			var add = .Prepend().NotNull();*/

			var add = tracker.ToolbarItems.ToArray();
			itemsSource.AddRange( add ); // TODO: MasterDetails
			NotifyOfPropertyChange( () => Primary );
			NotifyOfPropertyChange( () => Secondary );
			/*var bar = ApplicationBar.EnsureInstance( tracker.Target );
			var items = tracker.ToolbarItems.ToArray();
			var masterDetail = tracker.Target.Descendants().Prepend( tracker.Target ).FirstOrDefaultOfType<MasterDetailPage>();
			var taggedAppBarButton = Primary.OfType<ApplicationBarIconButton>().FirstOrDefault( b => b.Tag is MasterDetailPage && b.Tag != masterDetail );
			if ( taggedAppBarButton != null )
			{
				primarySource.Remove( taggedAppBarButton );
			}
			if ( masterDetail != null && masterDetail.ShouldShowToolbarButton() )
			{
				if ( Enumerable.OfType<ApplicationBarIconButton>( page.ApplicationBar.Buttons ).All( b => b.Tag != masterDetail ) )
				{
					var taggedAppBarButton2 = new ApplicationBarIconButton
					{
						IconUri = new Uri( masterDetail.Master.Icon ?? "ApplicationIcon.jpg", UriKind.Relative ),
						Text = masterDetail.Master.Title,
						IsEnabled = true,
						Tag = masterDetail
					};
					taggedAppBarButton2.MouseLeftButtonUp += ( sender, args ) => masterDetail.GetRenderer().As<MasterDetailRenderer>( renderer => renderer.Toggle() );
					page.ApplicationBar.Buttons.Add( taggedAppBarButton2 );
				}
			}
			var list = new List<ApplicationBarIconButton>();
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
}
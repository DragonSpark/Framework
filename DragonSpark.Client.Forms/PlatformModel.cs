using DragonSpark.Activation.IoC.Commands;
using DragonSpark.Application.Client.Eventing;
using DragonSpark.Application.Client.Forms.Rendering;
using DragonSpark.Application.Client.Interaction;
using DragonSpark.Extensions;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using System;
using Xamarin.Forms;
using Size = System.Windows.Size;

namespace DragonSpark.Application.Client.Forms
{
	[RegisterAs( typeof(IPlatform) )]
	public class PlatformModel : BindableBase, IPlatform
	{
		readonly IPlatformEngine engine;
		readonly Xamarin.Forms.Application application;

		public event EventHandler BindingContextChanged
		{
			add { Application.BindingContextChanged += value; }
			remove { Application.BindingContextChanged -= value; }
		}

		public PlatformModel( IPlatformEngine engine, Xamarin.Forms.Application application )
		{
			this.engine = engine;
			this.application = application;
		}

		public InteractionRequest<DialogNotification> DisplayRequest
		{
			get { return displayRequest; }
		}	readonly InteractionRequest<DialogNotification> displayRequest = new InteractionRequest<DialogNotification>();

		/*void OnBackKeyPress( object sender, CancelEventArgs e )
		{
			if ( visibleMessageBox != null )
			{
				visibleMessageBox.CloseCommand.Execute( null );
				e.Cancel = true;
				return;
			}			
		}*/

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

		public Xamarin.Forms.Application Application
		{
			get { return application; }
		}

		public Size Size
		{
			get { return size; }
			set
			{
				if ( SetProperty( ref size, value ) )
				{
					Refresh();
				}
			}
		}	Size size;

		void Refresh()
		{
			Page.With( item => item.Layout( new Rectangle( 0.0, 0.0, Size.Width, Size.Height ) ) );
		}

		public object Content
		{

			get { return content; }
			private set 
			{
				if ( SetProperty( ref content, value ) )
				{
					Refresh();
				}
			}
		}	object content;

		void IPlatform.SetPage( Page current )
		{
			Page = current;
		}

		public Page Page
		{
			get { return page; }
			private set
			{
				if ( SetProperty( ref page, value ) )
				{
					page.Platform = this;
					Content = page.DetermineRenderer();

					this.Event<ShellPageChangedEvent>().Publish( Page );
				}
			}
		}	Page page;

		object IPlatform.BindingContext
		{
			get { return application.BindingContext; }
			set
			{
				if ( application.BindingContext != value )
				{
					application.BindingContext = value;
				}
			}
		}

		public IPlatformEngine Engine
		{
			get { return engine; }
		}
	}
}
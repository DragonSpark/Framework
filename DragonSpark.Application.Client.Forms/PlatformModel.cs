using DragonSpark.Application.Client.Eventing;
using DragonSpark.Application.Client.Interaction;
using DragonSpark.Extensions;
using DragonSpark.Setup;
using Prism.Interactivity.InteractionRequest;
using Prism.Presentation;
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
		}

		public InteractionRequest<DialogNotification> DisplayRequest
		{
			get { return displayRequest; }
		}	readonly InteractionRequest<DialogNotification> displayRequest = new InteractionRequest<DialogNotification>();

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
			Page.With( item =>
			{
				var bounds = new Rectangle( 0.0, 0.0, Size.Width, Size.Height );
				item.Layout( bounds );
				this.Event<ShellSizeChangedEvent>().Publish( bounds.Size );
			} );
		}

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
					
					this.Event<ShellPageChangedEvent>().Publish( Page );

					Refresh();
				}
			}
		}	Page page;

		public double ScaleFactor
		{
			get { return scaleFactor; }
			set
			{
				if ( SetProperty( ref scaleFactor, value ) )
				{
					this.Event<ShellScaleFactorChangedEvent>().Publish( value );
				}
			}
		}   double scaleFactor = 1.0d;

		public IPlatformEngine Engine
		{
			get { return engine; }
		}
	}
}
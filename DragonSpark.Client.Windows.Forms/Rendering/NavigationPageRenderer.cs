using System;
using System.Linq;
using System.Windows;
using DragonSpark.Extensions;
using Xamarin.Forms;

namespace DragonSpark.Client.Windows.Forms.Rendering
{
	public class NavigationPageRenderer : VisualElementRenderer<NavigationPage, FrameworkElement>
	{
		public NavigationPageRenderer() // TODO: Uncomment and render through Frame control.
		{
			AutoPackage = false;
		}

		protected override void OnElementChanged( ElementChangedEventArgs<NavigationPage> e )
		{
			base.OnElementChanged( e );
			Action init = delegate
			{
				Element.PushRequested += PageOnPushed;
				base.Element.PopRequested += PageOnPopped;
				base.Element.PopToRootRequested += PageOnPoppedToRoot;
				base.Element.RemovePageRequested += RemovePageRequested;
				base.Element.InsertPageBeforeRequested += this.ElementOnInsertPageBeforeRequested;
				/*var platform = (ApplicationHost)base.Element.ApplicationHost;
				base.Element.ContainerArea = new Rectangle( new Point( 0.0, 0.0 ), platform.Size );
				platform.SizeChanged += ( sender, args ) => this.Element.ContainerArea = new Rectangle( new Point( 0.0, 0.0 ), platform.Size );*/
				var stack = base.Element.StackCopy.Reverse().ToArray();
				if ( stack.Any() )
				{
					var page = stack.First();
					page.IgnoresContainerArea = true;
					if ( page.GetRenderer() == null )
					{
						page.SetRenderer( RendererFactory.GetRenderer( page ) );
					}
					page.GetRenderer().As<UIElement>( x => base.Children.Add( x ) );
				}
				if ( stack.Count() <= 1 )
				{
					return;
				}
				Device.BeginInvokeOnMainThread( delegate
				{
					foreach ( var current in stack.Skip( 1 ) )
					{
						this.PageOnPushed( this, new NavigationRequestedEventArgs( current, false ) );
					}
				} );
			};
			if ( Element.Platform == null )
			{
				Element.PlatformSet += ( sender, args ) => init();
			}
			else
			{
				init();
			}
			Loaded += ( sender, args ) => Element.SendAppearing();
			Unloaded += ( sender, args ) => Element.SendDisappearing();
		}

		void ElementOnInsertPageBeforeRequested( object sender, NavigationRequestedEventArgs eventArgs )
		{
			/*var platform = Element.ApplicationHost as ApplicationHost;
			if ( platform != null )
			{
				platform.InsertPageBefore( eventArgs.Page, eventArgs.BeforePage );
			}*/
		}

		void RemovePageRequested( object sender, NavigationRequestedEventArgs eventArgs )
		{
			/*var platform = Element.ApplicationHost as ApplicationHost;
			if ( platform != null )
			{
				platform.RemovePage( eventArgs.Page );
			}*/
		}

		void PageOnPoppedToRoot( object sender, NavigationRequestedEventArgs eventArgs )
		{
			/*var platform = Element.ApplicationHost as ApplicationHost;
			if ( platform != null )
			{
				platform.PopToRoot( Element, eventArgs.Animated );
			}*/
		}

		void PageOnPopped( object sender, NavigationRequestedEventArgs eventArg )
		{
			/*var platform = Element.ApplicationHost as ApplicationHost;
			if ( platform != null )
			{
				platform.Pop( Element, eventArg.Animated );
			}*/
		}

		void PageOnPushed( object sender, NavigationRequestedEventArgs eventArg )
		{
			/*var platform = Element.ApplicationHost as ApplicationHost;
			if ( platform != null )
			{
				if ( eventArg.Page == Element.StackCopy.LastOrDefault() )
				{
					eventArg.Page.IgnoresContainerArea = true;
				}
				platform.Push( eventArg.Page, Element, eventArg.Animated );
			}*/
		}
	}
}

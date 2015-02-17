using DragonSpark.Client.Windows.Forms.Rendering;
using DragonSpark.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Page = Xamarin.Forms.Page;

namespace DragonSpark.Client.Windows.Forms
{
	/*public class ShellBehavior : System.Windows.Interactivity.Behavior<FrameworkElement>
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
	}*/

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
		readonly INavigationModel model;

		public Navigation( IPlatform platform, INavigationModel model )
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
			modal.AssignNavigation( this );
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
			return this.PushAsync(root, true);
		}

		public async Task PushAsync(Xamarin.Forms.Page root, bool animated)
		{
			await this.Push(root, platform.Page, animated);
		}

		public async Task Push(Xamarin.Forms.Page root, Xamarin.Forms.Page ancester, bool animated)
		{
			model.Push( root, ancester );
			await SetCurrent( model.CurrentPage, animated );
			if ( root.GetNavigation() == null )
			{
				root.AssignNavigation( this );
			}
		}

		public Task<Page> PopAsync()
		{
			return PopAsync( true );
		}

		public Task<Page> PopAsync( bool animated )
		{
			return Pop( platform.Page, animated );
		}

		public async Task<Page> Pop( Page ancestor, bool animated )
		{
			var result = model.Pop( ancestor );
			await SetCurrent( model.CurrentPage, animated, true );
			return result;
		}

		public Task PopToRootAsync()
		{
			return PopToRootAsync( true );
		}

		public async Task PopToRootAsync( bool animated )
		{
			await PopToRoot( platform.Page, animated );
		}

		public async Task PopToRoot( Page ancestor, bool animated )
		{
			model.PopToRoot( ancestor );
			await SetCurrent( model.CurrentPage, animated, true, null );
		}

		Task SetCurrent( Page page, bool animated, bool popping = false, Action completedCallback = null )
		{
			return Task.FromResult( 0 );
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

	/*public static class ShellProperties
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
	}*/
}

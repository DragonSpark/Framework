using DragonSpark.Activation.IoC;
using DragonSpark.Client.Windows.Extensions;
using DragonSpark.Client.Windows.Forms.Rendering;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Activation;
using Xamarin.Forms;
using Page = Xamarin.Forms.Page;
using ServiceLocator = Microsoft.Practices.ServiceLocation.ServiceLocator;
using Size = System.Windows.Size;

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

	[System.Windows.Markup.ContentProperty( "Initializer" )]
	public class InitializeFormsCommand : IContainerConfigurationCommand
	{
		public void Configure( IUnityContainer container )
		{
			var initializer = Initializer ?? new Initializer();
			initializer.Initialize();
		}

		// [Activate( typeof(Initializer) )]
		public IInitializer Initializer { get; set; }
	}

	[System.Windows.Markup.ContentProperty( "Application" )]
	public class SetupFormsCommand : IContainerConfigurationCommand
	{
		public async void Configure( IUnityContainer container )
		{
			var platform = new PlatformModel( Engine, Application );
			container.RegisterInstance<IPlatform>( platform );
			container.RegisterInstance( platform );

			var navigation = new Navigation( platform, NavigationModel );
			await navigation.PushAsync( Application.MainPage );
		}

		public Xamarin.Forms.Application Application { get; set; }

		[Activate( typeof(Engine) )]
		public IPlatformEngine Engine { get; set; }

		[Activate( typeof(NavigationModel) )]
		public INavigationModel NavigationModel { get; set; }
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

	public class PlatformModel : BindableBase, IPlatform
	{
		public event EventHandler BindingContextChanged = delegate {};

		readonly IPlatformEngine engine;
		readonly global::Xamarin.Forms.Application application;

		public PlatformModel( IPlatformEngine engine, global::Xamarin.Forms.Application application )
		{
			this.engine = engine;
			this.application = application;
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
			Page.With( page => page.Layout( new Rectangle( 0.0, 0.0, Size.Width, Size.Height ) ) );
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
			if ( current != Page )
			{
				Page = current;
				current.Assign( this );
				
				Content = current.DetermineRenderer();
				// UpdateToolbarTracker();

			}
		}

		public Page Page { get; private set; }

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

		public async Task PushAsync( Page root, bool animated )
		{
			await Push( root, platform.Page, animated );
		}

		public async Task Push( Page root, Page ancester, bool animated )
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
			await SetCurrent( model.CurrentPage, animated, true );
		}

		Task SetCurrent( Page page, bool animated, bool popping = false, Action completedCallback = null )
		{
			return Task.Factory.StartNew( () =>
			{
				platform.SetPage( page );
				completedCallback.With( x => x() );
			}, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext() );
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

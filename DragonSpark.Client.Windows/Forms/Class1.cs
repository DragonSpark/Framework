using DragonSpark.Activation.IoC.Commands;
using DragonSpark.Client.Windows.Forms.Rendering;
using DragonSpark.Common.Application;
using DragonSpark.Extensions;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Page = Xamarin.Forms.Page;
using Size = System.Windows.Size;

namespace DragonSpark.Client.Windows.Forms
{
	[System.Windows.Markup.ContentProperty( "Initializer" )]
	public class ConfigureApplicationCommand : Common.IoC.Commands.ConfigureApplicationCommand
	{
		public IInitializer Initializer { get; set; }

		// [Default( PrincipalPolicy.WindowsPrincipal )]
		public PrincipalPolicy? PrincipalPolicy { get; set; }

		protected override void OnConfigure( IUnityContainer container )
		{
			base.OnConfigure( container );

			PrincipalPolicy.WithValue( AppDomain.CurrentDomain.SetPrincipalPolicy );

			Initializer.Initialize();

			container.Resolve<IEventAggregator>().With( aggregator => aggregator.ExecuteWhenStatusIs( SetupStatus.Configured, async () =>
			{
				var navigation = container.Resolve<INavigation>();
				var application = container.Resolve<Xamarin.Forms.Application>();
				await navigation.PushAsync( application.MainPage );
				/*var platform = container.Resolve<IPlatform>();
				container.RegisterInstance( platform );

				var navigation = new Navigation( platform, NavigationModel );
				await navigation.PushAsync( Application.MainPage );*/
			} ) );

			System.Windows.Application.Current.With( x =>
			{
				container.RegisterInstance( System.Windows.Application.Current.Dispatcher );
				x.Exit += ( s, a ) => container.Resolve<IServiceLocator>().TryDispose();
			} );
		}
	}

	/*[System.Windows.Markup.ContentProperty( "Application" )]
	public class ConfigureFormsCommand : IContainerConfigurationCommand
	{
		public async void Configure( IUnityContainer container )
		{
			
		}

		public Xamarin.Forms.Application Application { get; set; }

		[Activate( typeof(PlatformEngine) )]
		public IPlatformEngine Engine { get; set; }

		[Activate( typeof(NavigationModel) )]
		public INavigationModel NavigationModel { get; set; }
	}*/

	public class PlatformEngine : IPlatformEngine
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

	[RegisterAs( typeof(IPlatform) )]
	public class PlatformModel : BindableBase, IPlatform
	{
		public event EventHandler BindingContextChanged
		{
			add { Application.BindingContextChanged += value; }
			remove { Application.BindingContextChanged -= value; }
		}

		readonly IPlatformEngine engine;
		readonly global::Xamarin.Forms.Application application;

		public PlatformModel( IPlatformEngine engine, global::Xamarin.Forms.Application application )
		{
			this.engine = engine;
			this.application = application;
		}

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
			Page = current;
		}

		public Page Page
		{
			get { return page; }
			private set
			{
				if ( SetProperty( ref page, value ) )
				{
					page.Assign( this );
				
					Content = page.DetermineRenderer();
					// UpdateToolbarTracker();
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

	public class Navigation : INavigation
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

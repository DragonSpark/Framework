using DragonSpark.Extensions;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Security.Principal;
using Xamarin.Forms;

namespace DragonSpark.Application.Client.Forms
{
	[System.Windows.Markup.ContentProperty( "Initializer" )]
	public class ConfigureApplicationCommand : DragonSpark.Application.IoC.Commands.ConfigureApplicationCommand
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

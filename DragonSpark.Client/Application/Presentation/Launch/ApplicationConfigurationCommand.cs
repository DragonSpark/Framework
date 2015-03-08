using DragonSpark.Extensions;
using DragonSpark.IoC;
using DragonSpark.IoC.Configuration;
using DragonSpark.Runtime;
using Microsoft.LightSwitch.Security.ClientGenerated.Implementation.Internal;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System.IO.IsolatedStorage;
using System.Net;
using System.Net.Browser;
using System.ServiceModel.DomainServices.Client.ApplicationServices;

namespace DragonSpark.Application.Presentation.Launch
{
	public class ApplicationConfigurationCommand : IContainerConfigurationCommand
	{
		public void Configure( IUnityContainer container )
		{
			WebRequest.RegisterPrefix( "http://", WebRequestCreator.ClientHttp );

			System.Windows.Application.Current.With( x =>
			{
                var context = x.ApplicationLifetimeObjects.FirstOrDefaultOfType<AuthenticationContext>();
                
                container.RegisterInstance<WebContextBase>( context );
                container.RegisterInstance( context.Authentication );
				context.Authentication.As<WebAuthenticationService>( y =>
				{
					y.DomainContext = container.TryResolve<AuthenticationDomainContextBase>() ?? y.DomainContext;
				} );
				
                var handler = container.TryResolve<IExceptionHandler>();
				handler.NotNull( y => x.UnhandledException += ( s, a ) => a.Handled = a.Handled || y.Handle( a.ExceptionObject ).Handled );

				x.Exit += ( s, a ) =>
				{
					container.TryResolve<IsolatedStorageSettings>().NotNull( y => y.Save() );
					container.Resolve<IServiceLocator>().TryDispose();
				};

                // AuthenticationHelper.BeginResolveAuthenticationType( _authenticationContext.Service );
            
                /*ManifestService.CacheDirectory = x.IsRunningOutOfBrowser && x.HasElevatedPermissions ? 
                    Path.Combine( System.Environment.GetFolderPath( System.Environment.SpecialFolder.Personal ), @"Microsoft\LightSwitch\Manifests", string.Format( "{0}.{1}", ApplicationConfiguration.Instance.ApplicationName, ApplicationConfiguration.Instance.ApplicationVersion ) )
                    :
                    IsolatedStorageFile.IsEnabled ? "Manifests" : ManifestService.CacheDirectory;
            
                var manifestFiles = ApplicationConfiguration.Instance.Config.Descendants( "Manifest" ).Select( element2 => element2.Value ).ToArray();
                ManifestService.AddManifests( manifestFiles );
            
                var catalog = ManifestService.CreateCatalog( "SubsystemService" );
                var settings = new VsExportProviderSettings( Application.LocalScope, VsExportProvidingPreference.Default, VsExportSharingPolicy.ShareWithinScope );
                var subsystemService = VsCompositionContainer.Create( catalog, settings ).GetExportedValue<ISubsystemService>();
                if ( subsystemService.Load( "SubsystemLoader", new[] { "SubsystemLoader" } ) )
                {
                    x.Exit += ( _, a ) => subsystemService.UnloadAll();
                }

                var contractName = AttributedModelServices.GetContractName( typeof(IClientApplicationFactory) );
                var factory = VsExportProviderService.GetExportedValue<IClientApplicationFactory>( Application.LocalScope, VsExportSharingPolicy.IncludeExportsFromOthers, contractName );
			    factory.NotNull( y => y.GetInstance().AuthenticationService = x.ApplicationLifetimeObjects.OfType<AuthenticationContext>().Single().Service );

                VsExportProviderService.GetServiceFromCache<IThemingService>( Application.LocalScope ).Initialize();
            
                IUserSettingsService service;
                if ( System.Windows.Application.Current.IsRunningOutOfBrowser && VsExportProviderService.TryGetServiceFromCache( Application.LocalScope, out service ) )
                {
                    service.Closing += SettingServiceClosing;
                    try
                    {
                        var setting = service.GetSetting<OutOfBrowserWindowSettings>( "Application.OutOfBrowser.WindowSettings" );
                        if ( setting != null )
                        {
                            if ( setting.WindowState == WindowState.Minimized )
                            {
                                setting.WindowState = WindowState.Maximized;
                            }
                            System.Windows.Application.Current.MainWindow.WindowState = setting.WindowState;
                            switch ( setting.WindowState )
                            {
                                case WindowState.Normal:
                                    try
                                    {
                                        System.Windows.Application.Current.MainWindow.Width = setting.Width;
                                    }
                                    catch ( ArgumentOutOfRangeException )
                                    {}
                                    try
                                    {
                                        System.Windows.Application.Current.MainWindow.Height = setting.Height;
                                    }
                                    catch ( ArgumentOutOfRangeException )
                                    {}
                                    try
                                    {
                                        System.Windows.Application.Current.MainWindow.Top = setting.Top;
                                    }
                                    catch ( ArgumentOutOfRangeException )
                                    {}
                                    try
                                    {
                                        System.Windows.Application.Current.MainWindow.Left = setting.Left;
                                        return;
                                    }
                                    catch ( ArgumentOutOfRangeException )
                                    {
                                        return;
                                    }
                            }
                            return;
                        }
                        System.Windows.Application.Current.MainWindow.WindowState = WindowState.Maximized;
                    }
                    catch ( Exception )
                    {}
                }*/
			} );
		}

	    /*static void SetFlowDirection( FrameworkElement root )
        {
            var items = new[] { "ar", "he" };
            var any = items.Any( x => string.Equals( CultureInfo.CurrentUICulture.TwoLetterISOLanguageName, x, StringComparison.OrdinalIgnoreCase ) );
            root.FlowDirection = any ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
        }

        static void SetThreadCulture()
        {
            if ( !string.IsNullOrEmpty( ApplicationConfiguration.Instance.ApplicationCulture ) )
            {
                try
                {
                    var info = new CultureInfo( ApplicationConfiguration.Instance.ApplicationCulture );
                    Thread.CurrentThread.CurrentUICulture = info;
                }
                catch ( ArgumentException )
                {}
            }
        }

	    static void SettingServiceClosing( object sender, EventArgs e )
        {
            if ( System.Windows.Application.Current.IsRunningOutOfBrowser )
            {
                var serviceFromCache = VsExportProviderService.GetServiceFromCache<IUserSettingsService>( Application.LocalScope );
                if ( serviceFromCache != null )
                {
                    var setting = serviceFromCache.GetSetting<OutOfBrowserWindowSettings>( "Application.OutOfBrowser.WindowSettings" ) ?? new OutOfBrowserWindowSettings();
                    setting.WindowState = System.Windows.Application.Current.MainWindow.WindowState;
                    setting.Top = System.Windows.Application.Current.MainWindow.Top;
                    setting.Left = System.Windows.Application.Current.MainWindow.Left;
                    setting.Width = System.Windows.Application.Current.MainWindow.Width;
                    setting.Height = System.Windows.Application.Current.MainWindow.Height;
                    serviceFromCache.SetSetting( "Application.OutOfBrowser.WindowSettings", setting );
                }
            }
        }

        sealed class ApplicationConfiguration
        {
            const string ConfigFile = "config.xml";

            ApplicationConfiguration()
            {
                ApplicationVersion = "1.0.0.0";
                ApplicationName = "DefaultApplication";
                Config = XDocument.Load( ConfigFile );
                var element = Config.Descendants( "ApplicationName" ).FirstOrDefault();
                if ( ( element != null ) && !string.IsNullOrEmpty( element.Value ) )
                {
                    ApplicationName = element.Value;
                }
                var element2 = Config.Descendants( "Version" ).FirstOrDefault();
                if ( ( element2 != null ) && !string.IsNullOrEmpty( element2.Value ) )
                {
                    ApplicationVersion = element2.Value;
                }
                var element3 = Config.Descendants( "ApplicationCulture" ).FirstOrDefault();
                if ( ( element3 != null ) && !string.IsNullOrEmpty( element3.Value ) )
                {
                    ApplicationCulture = element3.Value;
                }
            }

            public string ApplicationCulture { get; private set; }

            public string ApplicationName { get; private set; }

            public string ApplicationVersion { get; private set; }

            public XDocument Config { get; private set; }

            public static ApplicationConfiguration Instance
            {
                get { return InstanceField; }
            }   static readonly ApplicationConfiguration InstanceField = new ApplicationConfiguration();
        }*/
	}
}
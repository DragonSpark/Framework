using DragonSpark.Application.Presentation.Configuration;
using DragonSpark.Application.Presentation.Launch;
using DragonSpark.Extensions;
using DragonSpark.Objects;
using Microsoft.LightSwitch.Runtime.Shell.Implementation;
using Microsoft.LightSwitch.Runtime.Shell.Internal;
using Microsoft.LightSwitch.Runtime.Shell.Internal.Implementation;
using Microsoft.LightSwitch.Security;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Navigation;

namespace DragonSpark.Application.Presentation
{
    [ContentProperty( "ApplicationLauncher" )]
    public class Application : App
    {
		public static readonly AuthenticationType AuthenticationType = AuthenticationType.Forms;

	    readonly AuthenticationServiceBridge bridge = new AuthenticationServiceBridge();

		Frame rootFrame;

        public Application()
        {
            Startup += ( s, a ) => OnStartup( a );

            ApplicationLifetimeObjects.FirstOrDefaultOfType<Microsoft.LightSwitch.Security.ClientGenerated.Implementation.Internal.AuthenticationContext>().With( x => x.Authentication = bridge );
        }

        [ActivationDefault( typeof(InstanceFactory<ApplicationLauncher>) )]
        public IFactory ApplicationLauncherLocator { get; set; }

        protected virtual void OnStartup( StartupEventArgs e )
        {
			var launcher = this.WithDefaults().ApplicationLauncherLocator.Transform( x => x.Create<ApplicationLauncher>() );
		    launcher.NotNull( x => x.Launch( e.InitParams ) );

	        Threading.Application.Start( () =>
	        {
				rootFrame = System.Windows.Application.Current.RootVisual.AsTo<ContentPresenter, Frame>( x => x.Content.AsTo<Page, Frame>( y => y.Content.As<Frame>() ) );
				rootFrame.NotNull( x =>
				{
					x.Navigating += XOnNavigating;
					x.Navigated += RootFrameNavigated;

					bridge.Run();
				} );
	        } );
        }

	    void XOnNavigating( object sender, NavigatingCancelEventArgs navigatingCancelEventArgs )
	    {
		    navigatingCancelEventArgs.Cancel = ( navigatingCancelEventArgs.Uri != ShellUri || !navigatingCancelEventArgs.Cancel ) && navigatingCancelEventArgs.Cancel;
	    }

	    void RootFrameNavigated( object sender, NavigationEventArgs e )
	    {
		    e.Content.As<LoginPage>( x => NavigateToShell() );
	    }

	    Uri ShellUri
	    {
		    get { return shellUri ?? ( shellUri = Exports.With<IShellService, Uri>( x => x.CurrentShell.ShellUri ) ); }
	    }	Uri shellUri;

	    void NavigateToShell()
	    {
		    rootFrame.Navigate( ShellUri );
	    }
    }
}

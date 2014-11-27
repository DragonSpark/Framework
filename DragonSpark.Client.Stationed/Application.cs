using System.Windows;
using System.Windows.Markup;
using DragonSpark.Extensions;

namespace DragonSpark.Client.Stationed
{
	[ContentProperty( "Launcher" )]
	public class Application : System.Windows.Application
	{
		public Application()
		{
			Startup += ( s, a ) => OnStartup( a );
		}

		public ApplicationLauncher Launcher { get; set; }

		protected override void OnStartup( StartupEventArgs e )
		{
			base.OnStartup( e );

			Launcher.With( x => x.Launch( e.Args ) );
		}
	}
}
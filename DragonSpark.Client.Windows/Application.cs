using DragonSpark.Extensions;
using System.Windows;
using System.Windows.Markup;

namespace DragonSpark.Client.Windows
{
	[ContentProperty( "Setup" )]
	public class Application : System.Windows.Application
	{
		public Application()
		{
			// Startup += ( s, a ) => OnStartup( a );
		}

		public Setup Setup { get; set; }

		protected override void OnStartup( StartupEventArgs e )
		{
			Setup.With( x => x.Launch( e.Args ) );

			base.OnStartup( e );
		}
	}
}
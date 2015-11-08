using DragonSpark.Extensions;
using Prism;
using System.Windows;
using System.Windows.Markup;

namespace DragonSpark.Application.Client
{
	[ContentProperty( "Setup" )]
	public class Application : System.Windows.Application
	{
		public ISetup Setup { get; set; }

		protected override void OnStartup( StartupEventArgs e )
		{
			Setup.With( x => x.Run( e.Args ) );

			base.OnStartup( e );
		}
	}
}
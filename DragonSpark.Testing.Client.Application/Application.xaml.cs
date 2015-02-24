
using System.Diagnostics;

namespace DragonSpark.Testing.Client.Application
{
	/// <summary>
	/// Interaction logic for Application.xaml
	/// </summary>
	public partial class Application
	{
		public Application()
		{
			Trace.WriteLine( "Instantiating Application." );
			InitializeComponent();
		}

		/*protected override void OnStartup( StartupEventArgs e )
		{
			try
			{
				var temp = Resources["PhoneAccentBrush"];
			}
			catch ( Exception exception )
			{
				Console.WriteLine( exception );
			}
			base.OnStartup( e );
		}*/
	}
}

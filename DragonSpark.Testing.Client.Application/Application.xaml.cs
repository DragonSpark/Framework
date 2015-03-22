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
			typeof(ApplicationDefinition).GetType();
		}

		/*protected override void OnStartup( StartupEventArgs e )
		{
			base.OnStartup( e );

			Setup.As<Setup>( setup =>
			{
				var commands = setup.Commands;
				Debugger.Break();
			} );
		}*/
	}
}

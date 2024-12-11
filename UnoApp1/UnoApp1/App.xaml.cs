using DragonSpark.Compose;

namespace UnoApp1;

public partial class App : Application
{
	/// <summary>
	/// Initializes the singleton application object. This is the first line of authored code
	/// executed, and as such is the logical equivalent of main() or WinMain().
	/// </summary>
	public App()
	{
		this.InitializeComponent();
	}

	protected Window? MainWindow { get; private set; }
	protected IHost? Host { get; private set; }

	protected async override void OnLaunched(LaunchActivatedEventArgs args)
	{
		var (builder, host) = await RunApplication.Default.Await(new(this, args));

		MainWindow = builder.Window;
		Host       = host;
	}
}

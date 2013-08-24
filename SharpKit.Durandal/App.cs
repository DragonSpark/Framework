using SharpKit.Durandal.Modules;
using SharpKit.JavaScript;
using SharpKit.jQuery;

namespace SharpKit.Durandal
{
	[Module( Export = false )]
	public class App
	{
		/// <summary>
		/// The title of your application.
		/// </summary>
		[JsProperty( NativeField = true )]
		public string Title { get; set; }

		/// <summary>
		/// Configures one or more plugins to be loaded and installed into the application.
		/// </summary>
		/// <param name="config">Keys are plugin names. Values can be truthy, to simply install the plugin, or a configuration object to pass to the plugin.</param>
		/// <param name="baseUrl">The base url to load the plugins from.</param>
		public void ConfigurePlugins( object config, string baseUrl )
		{}

		/// <summary>
		/// Starts the application.
		/// </summary>
		/// <returns>Promise</returns>
		public Promise Start()
		{
			return default(Promise);
		}

		/// <summary>
		/// Sets the root module/view for the application.
		/// </summary>
		/// <param name="root">The root view or module.</param>
		/// <param name="transition">The transition to use from the previous root (or splash screen) into the new root.</param>
		/// <param name="applicationHost">The application host element or id. By default the id 'applicationHost' will be used.</param>
		public void SetRoot( string root, string transition, string applicationHost )
		{}
	}
}
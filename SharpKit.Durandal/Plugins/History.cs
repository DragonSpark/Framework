using SharpKit.Durandal.Modules;
using SharpKit.JavaScript;
using System.ComponentModel;

namespace SharpKit.Durandal.Plugins
{
	[JsType( JsMode.Json )]
	public class HistoryOptions
	{
		[JsProperty(  Name = "routeHandler", NativeField = true )]
		public JsFunction RouteHandler { get; set; }

		[JsProperty(  Name = "root", NativeField = true )]
		public string Root { get; set; }

		[JsProperty(  Name = "hashChange", NativeField = true )]
		public bool HashChange { get; set; }

		[JsProperty(  Name = "pushState", NativeField = true )]
		public bool PushState { get; set; }

		[JsProperty(  Name = "silent", NativeField = true )]
		public bool Silent { get; set; }
	}

	[Module( Export = false )]
	public static class History
	{
		[DefaultValue( 50 )]
		[JsProperty( NativeField = true )]
		public static int Interval { get; set; }

		[JsProperty( NativeField = true )]
		public static bool Active { get; set; }

		/// <summary>
		/// Gets the true hash value. Cannot use location.hash directly due to a bug in Firefox where location.hash will always be decoded.
		/// </summary>
		/// <param name="window">The optional window instance</param>
		/// <returns>The hash.</returns>
		public static string GetHash( string window )
		{
			return default(string);
		}

		/// <summary>
		/// Get the cross-browser normalized URL fragment, either from the URL, the hash, or the override.
		/// </summary>
		/// <param name="fragment">The fragment.</param>
		/// <param name="forcePushState">Should we force push state?</param>
		/// <returns>The fragment.</returns>
		public static string GetFragment( string fragment, bool forcePushState )
		{
			return default(string);
		}

		/// <summary>
		/// Activate the hash change handling, returning `true` if the current URL matches an existing route, and `false` otherwise.
		/// </summary>
		/// <param name="options"></param>
		/// <returns>Returns true/false from loading the url unless the silent option was selected.</returns>
		public static bool? Activate( HistoryOptions options )
		{
			return null;
		}

		/// <summary>
		/// Disable history, perhaps temporarily. Not useful in a real app, but possibly useful for unit testing Routers.
		/// </summary>
		public static void Deactivate()
		{}

		/// <summary>
		/// Checks the current URL to see if it has changed, and if it has, calls `loadUrl`, normalizing across the hidden iframe.
		/// </summary>
		/// <returns>Returns true/false from loading the url.</returns>
		public static bool CheckUrl()
		{
			return default(bool);
		}

		/// <summary>
		/// Attempts to load the current URL fragment. A pass-through to options.routeHandler.
		/// </summary>
		/// <param name="fragmentOverride"></param>
		/// <returns></returns>
		public static bool LoadUrl( string fragmentOverride )
		{
			return default(bool);
		}

		/// <summary>
		/// Save a fragment into the hash history, or replace the URL state if the 'replace' option is passed. You are responsible for properly URL-encoding the fragment in advance. The options object can contain `trigger: true` if you wish to have the route callback be fired (not usually desirable), or `replace: true`, if you wish to modify the current URL without adding an entry to the history.</summary>
		/// <param name="fragment">The url fragment to navigate to.</param>
		/// <param name="trigger">An options object with optional trigger and replace flags. You can also pass a boolean directly to set the trigger option.</param>
		/// <returns>Returns true/false from loading the url.</returns>
		public static bool Navigate( string fragment, bool trigger )
		{
			return default(bool);
		}

		/// <summary>
		/// Save a fragment into the hash history, or replace the URL state if the 'replace' option is passed. You are responsible for properly URL-encoding the fragment in advance. The options object can contain `trigger: true` if you wish to have the route callback be fired (not usually desirable), or `replace: true`, if you wish to modify the current URL without adding an entry to the history.</summary>
		/// <param name="fragment">The url fragment to navigate to.</param>
		/// <param name="options">An options object with optional trigger and replace flags. You can also pass a boolean directly to set the trigger option.</param>
		/// <returns>Returns true/false from loading the url.</returns>
		public static bool Navigate( string fragment, HistoryOptions options )
		{
			return default(bool);
		}
	}
}
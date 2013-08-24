using SharpKit.Durandal.Modules;
using SharpKit.JavaScript;
using SharpKit.jQuery;

namespace SharpKit.Durandal.Plugins
{
	[Module( Export = false )]
	public static class Http
	{
		/// <summary>
		/// The name of the callback parameter to inject into jsonp requests by default.
		/// </summary>
		[JsProperty( NativeField = true )]
		public static string CallbackParam { get; set; }

		/// <summary>
		/// Makes an HTTP GET request.
		/// </summary>
		/// <param name="url">The url to send the get request to.</param>
		/// <param name="query">An optional key/value object to transform into query string parameters.</param>
		/// <returns>A promise of the get response data.</returns>
		public static Promise Get( string url, string query )
		{
			return default(Promise);
		}

		/// <summary>
		/// Makes an JSONP request.
		/// </summary>
		/// <param name="url">The url to send the get request to.</param>
		/// <param name="query">An optional key/value object to transform into query string parameters.</param>
		/// <param name="callbackParam">The name of the callback parameter the api expects (overrides the default callbackParam).</param>
		/// <returns>A promise of the response data.</returns>
		public static Promise JsonP( string url, string query, string callbackParam )
		{
			return default(Promise);
		}

		/// <summary>
		/// Makes an HTTP POST request.
		/// </summary>
		/// <param name="url">The url to send the post request to.</param>
		/// <param name="data">The data to post. It will be converted to JSON. If the data contains Knockout observables, they will be converted into normal properties before serialization.</param>
		/// <returns>A promise of the response data.</returns>
		public static Promise Post( string url, object data )
		{
			return default(Promise);
		}
	}
}
using SharpKit.Durandal.Modules;
using SharpKit.Html;
using SharpKit.JavaScript;
using SharpKit.jQuery;

namespace SharpKit.Durandal
{
	[Module( Export = false )]
	public class ViewEngine
	{
		/// <summary>
		/// The file extension that view source files are expected to have.  Default is .html
		/// </summary>
		[JsProperty( NativeField = true )]
		public JsString ViewExtension { get; set; }

		/// <summary>
		/// The name of the RequireJS loader plugin used by the viewLocator to obtain the view source. (Use requirejs to map the plugin's full path).  Default is text.
		/// </summary>
		[JsProperty( NativeField = true )]
		public JsString ViewPlugin { get; set; }

		/// <summary>
		/// Determines if the url is a url for a view, according to the view engine.
		/// </summary>
		/// <param name="url">The potential view url.</param>
		/// <returns>True if the url is a view url, false otherwise.</returns>
		public bool IsViewUrl( JsString url )
		{
			return default(bool);
		}

		/// <summary>
		/// Converts a view url into a view id.
		/// </summary>
		/// <param name="url">The url to convert.</param>
		/// <returns>The view id.</returns>
		public string ConvertViewUrlToViewId( JsString url )
		{
			return default(string);
		}

		/// <summary>
		/// Converts a view id into a full RequireJS path.
		/// </summary>
		/// <param name="viewId">The view id to convert.</param>
		/// <returns>The require path.</returns>
		public string ConvertViewIdToRequirePath( JsString viewId )
		{
			return default(string);
		}

		/// <summary>
		/// Parses the view engine recognized markup and returns DOM elements.
		/// </summary>
		/// <param name="markup">The markup to parse.</param>
		/// <returns>The elements.</returns>
		public HtmlElement[] ParseMarkup( JsString markup )
		{
			return default(HtmlElement[]);
		}

		/// <summary>
		/// Calls `parseMarkup` and then pipes the results through `ensureSingleElement`.
		/// </summary>
		/// <param name="markup">The markup to parse.</param>
		/// <returns>The elements.</returns>
		public HtmlElement ProcessMarkup( JsString markup )
		{
			return default(HtmlElement);
		}

		/// <summary>
		/// Converts an array of elements into a single element. White space and comments are removed. If a single element does not remain, then the elements are wrapped.
		/// </summary>
		/// <param name="allElements">The elements.</param>
		/// <returns>A single element.</returns>
		public HtmlElement EnsureSingleElement( HtmlElement[] allElements )
		{
			return default(HtmlElement);
		}

		/// <summary>
		/// Creates the view associated with the view id.
		/// </summary>
		/// <param name="viewId">The view id whose view should be created.</param>
		/// <returns>A promise of the view.</returns>
		public Promise CreateView( JsString viewId )
		{
			return default(Promise);
		}

		/// <summary>
		/// Called when a view cannot be found to provide the opportunity to locate or generate a fallback view. Mainly used to ease development.
		/// </summary>
		/// <param name="viewId">The view id whose view should be created.</param>
		/// <param name="requirePath">The require path that was attempted.</param>
		/// <param name="err">The error that was returned from the attempt to locate the default view.</param>
		/// <returns>A promise for the fallback view.</returns>
		public Promise CreateFallbackView( JsString viewId, JsString requirePath, JsError err )
		{
			return default(Promise);
		}
	}
}
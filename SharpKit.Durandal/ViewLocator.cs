using SharpKit.Durandal.Modules;
using SharpKit.Html;
using SharpKit.JavaScript;
using SharpKit.jQuery;

namespace SharpKit.Durandal
{
	[Module( Export = false )]
	public class ViewLocator
	{
		/// <summary>
		/// Allows you to set up a convention for mapping module folders to view folders. It is a convenience method that customizes `convertModuleIdToViewId` and `translateViewIdToArea` under the covers.
		/// </summary>
		/// <param name="modulesPath">A string to match in the path and replace with the viewsPath. If not specified, the match is 'viewmodels'.</param>
		/// <param name="viewsPath">The replacement for the modulesPath. If not specified, the replacement is 'views'.</param>
		/// <param name="areasPath">Partial views are mapped to the "views" folder if not specified. Use this parameter to change their location.</param>
		public void UseConvention( JsString modulesPath, JsString viewsPath, JsString areasPath )
		{}

		/// <summary>
		/// Maps an object instance to a view instance.
		/// </summary>
		/// <param name="obj">The object to locate the view for.</param>
		/// <param name="elementsToSearch">An existing set of elements to search first.</param>
		/// <returns>A promise of the view.</returns>
		public Promise LocateViewForObject( object obj, HtmlElement[] elementsToSearch )
		{
			return default(Promise);
		}

		/// <summary>
		/// Converts a module id into a view id. By default the ids are the same.
		/// </summary>
		/// <param name="moduleId">The module id.</param>
		/// <returns>The view id.</returns>
		public JsString ConvertModuleIdToViewId( JsString moduleId )
		{
			return default(JsString);
		}

		/// <summary>
		/// If no view id can be determined, this function is called to genreate one. By default it attempts to determine the object's type and use that.
		/// </summary>
		/// <param name="obj">The object to determine the fallback id for.</param>
		/// <returns>The view id.</returns>
		public JsString DetermineFallbackViewId( object obj )
		{
			return default(JsString);
		}

		/// <summary>
		/// Takes a view id and translates it into a particular area. By default, no translation occurs.
		/// </summary>
		/// <param name="viewId">The view id.</param>
		/// <param name="area">The area to translate the view to.</param>
		/// <returns>The translated view id.</returns>
		public JsString TranslateViewIdToArea( JsString viewId, JsString area )
		{
			return default(JsString);
		}

		/// <summary>
		/// Locates the specified view.
		/// </summary>
		/// <param name="urlOrId">A view url or view id to locate.</param>
		/// <param name="area">The area to translate the view to.</param>
		/// <param name="elementsToSearch">An existing set of elements to search first.</param>
		/// <returns>A promise of the view.</returns>
		public Promise LocateView( JsString urlOrId, JsString area, HtmlElement[] elementsToSearch )
		{
			return default(Promise);
		}

		/// <summary>
		/// Locates the specified view.
		/// </summary>
		/// <param name="view">A view to locate.</param>
		/// <param name="area">The area to translate the view to.</param>
		/// <param name="elementsToSearch">An existing set of elements to search first.</param>
		/// <returns>A promise of the view.</returns>
		public Promise LocateView( HtmlElement view, JsString area, HtmlElement[] elementsToSearch )
		{
			return default(Promise);
		}
	}
}
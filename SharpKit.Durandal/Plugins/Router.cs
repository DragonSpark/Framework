using SharpKit.Durandal.Modules;
using SharpKit.JavaScript;
using SharpKit.jQuery;
using SharpKit.KnockoutJs;

namespace SharpKit.Durandal.Plugins
{
	[Module( Export = false )]
	public class Router
	{
		/// <summary>
		/// The route handlers that are registered. Each handler consists of a `routePattern` and a `callback`.
		/// </summary>
		[JsProperty( NativeField = true )]
		public object[] Handlers { get; set; }

		/// <summary>
		/// The route configs that are registered.
		/// </summary>
		[JsProperty( NativeField = true )]
		public object[] Routes { get; set; }

		/// <summary>
		/// The route configurations that have been designated as displayable in a nav ui (nav:true).
		/// </summary>
		[JsProperty( NativeField = true )]
		public ObservableArray<object> NavigationModel { get; set; }

		/// <summary>
		/// The active item/screen based on the current navigation state.
		/// </summary>
		[JsProperty( NativeField = true )]
		public Activator ActiveItem { get; set; }

		/// <summary>
		/// Indicates that the router (or a child router) is currently in the process of navigating.
		/// </summary>
		[JsProperty( NativeField = true )]
		public DependentObservable<bool> IsNavigating { get; set; }

		/// <summary>
		/// Parses a query string into an object.
		/// </summary>
		/// <param name="queryString">The query string to parse.</param>
		/// <returns></returns>
		public object ParseQueryString( string queryString )
		{
			return default(object);
		}

		/// <summary>
		/// Add a route to be tested when the url fragment changes.
		/// </summary>
		/// <param name="routePattern">The route pattern to test against.</param>
		/// <param name="callback">The callback to execute when the route pattern is matched.</param>
		public void Route( JsRegExp routePattern, JsFunction callback )
		{}

		/// <summary>
		/// Attempt to load the specified URL fragment. If a route succeeds with a match, returns `true`. If no defined routes matches the fragment, returns `false`.
		/// </summary>
		/// <param name="fragment">The URL fragment to find a match for.</param>
		/// <returns>True if a match was found, false otherwise.</returns>
		public bool LoadUrl( string fragment )
		{
			return default(bool);
		}

		/// <summary>
		/// Updates the document title based on the activated module instance, the routing instruction and the app.title.
		/// </summary>
		/// <param name="instance">The activated module.</param>
		/// <param name="instruction">The routing instruction associated with the action. It has a `config` property that references the original route mapping config.</param>
		public void UpdateDocumentTitle( object instance, object instruction )
		{}

		/// <summary>
		/// Save a fragment into the hash history, or replace the URL state if the 'replace' option is passed. You are responsible for properly URL-encoding the fragment in advance. The options object can contain `trigger: true` if you wish to have the route callback be fired (not usually desirable), or `replace: true`, if you wish to modify the current URL without adding an entry to the history.
		/// </summary>
		/// <param name="fragment">The url fragment to navigate to.</param>
		/// <param name="options">A boolean to set the trigger option.</param>
		/// <returns>Returns true/false from loading the url.</returns>
		public bool Navigate( string fragment, bool options )
		{
			return default(bool);
		}

		/// <summary>
		/// Save a fragment into the hash history, or replace the URL state if the 'replace' option is passed. You are responsible for properly URL-encoding the fragment in advance. The options object can contain `trigger: true` if you wish to have the route callback be fired (not usually desirable), or `replace: true`, if you wish to modify the current URL without adding an entry to the history.
		/// </summary>
		/// <param name="fragment">The url fragment to navigate to.</param>
		/// <param name="options">An options object with optional trigger and replace flags.</param>
		/// <returns>Returns true/false from loading the url.</returns>
		public bool Navigate( string fragment, object options )
		{
			return default(bool);
		}

		/// <summary>
		/// Navigates back in the browser history.
		/// </summary>
		public void NavigateBack()
		{}

		/// <summary>
		/// Converts a route to a hash suitable for binding to a link's href.
		/// </summary>
		/// <param name="route">The route.</param>
		/// <returns>The hash.</returns>
		public string ConvertRouteToHash( string route )
		{
			return default(string);
		}

		/// <summary>
		/// Converts a route to a module id. This is only called if no module id is supplied as part of the route mapping.
		/// </summary>
		/// <param name="route">The route.</param>
		/// <returns>The module id.</returns>
		public string ConvertRouteToModuleId( string route )
		{
			return default(string);
		}

		/// <summary>
		/// Converts a route to a displayable title. This is only called if no title is specified as part of the route mapping.
		/// </summary>
		/// <returns>The title.</returns>
		public string ConvertRouteToTitle()
		{
			return default(string);
		}

		/// <summary>
		/// Maps route patterns to modules.
		/// </summary>
		/// <param name="route">A route, config or array of configs.</param>
		/// <param name="configuration">The config for the specified route.</param>
		/// <returns>Chainable</returns>
		/// <example>
		/// router.map([
        ///    { route: '', title:'Home', moduleId: 'homeScreen', nav: true },
        ///    { route: 'customer/:id', moduleId: 'customerDetails'}
        /// ]);
        /// </example>
		public Chainable Map( string route, string configuration )
		{
			return default(Chainable);
		}

		/// <summary>
		/// Maps route patterns to modules.
		/// </summary>
		/// <param name="config">A route, config or array of configs.</param>
		/// <param name="configuration">The config for the specified route.</param>
		/// <returns>Chainable</returns>
		/// <example>
		/// router.map([
        ///    { route: '', title:'Home', moduleId: 'homeScreen', nav: true },
        ///    { route: 'customer/:id', moduleId: 'customerDetails'}
        /// ]);
        /// </example>
		public Chainable Map( object config, string configuration )
		{
			return default(Chainable);
		}

		/// <summary>
		/// Maps route patterns to modules.
		/// </summary>
		/// <param name="config">A route, config or array of configs.</param>
		/// <param name="configuration">The config for the specified route.</param>
		/// <returns>Chainable</returns>
		/// <example>
		/// router.map([
        ///    { route: '', title:'Home', moduleId: 'homeScreen', nav: true },
        ///    { route: 'customer/:id', moduleId: 'customerDetails'}
        /// ]);
        /// </example>
		public Chainable Map( object[] config, string configuration )
		{
			return default(Chainable);
		}

		/// <summary>
		/// Builds an observable array designed to bind a navigation UI to. The model will exist in the `navigationModel` property.
		/// </summary>
		/// <param name="defaultOrder">The default order to use for navigation visible routes that don't specify an order. The defualt is 100.</param>
		/// <returns>Chainable</returns>
		public Chainable BuildNavigationModel( int defaultOrder )
		{
			return default(Chainable);
		}

		/// <summary>
		/// Configures how the router will handle unknown routes.
		/// </summary>
		/// <param name="config">If not supplied, then the router will map routes to modules with the same name.  If a string is supplied, it represents the module id to route all unknown routes to. Finally, if config is a function, it will be called back with the route instruction containing the route info. The function can then modify the instruction by adding a moduleId and the router will take over from there.</param>
		/// <param name="replaceRoute">If config is a module id, then you can optionally provide a route to replace the url with.</param>
		/// <returns></returns>
		public Chainable MapUnknownRoutes( string config, string replaceRoute )
		{
			return default(Chainable);
		}

		/// <summary>
		/// Configures how the router will handle unknown routes.
		/// </summary>
		/// <param name="config">If not supplied, then the router will map routes to modules with the same name.  If a string is supplied, it represents the module id to route all unknown routes to. Finally, if config is a function, it will be called back with the route instruction containing the route info. The function can then modify the instruction by adding a moduleId and the router will take over from there.</param>
		/// <param name="replaceRoute">If config is a module id, then you can optionally provide a route to replace the url with.</param>
		/// <returns></returns>
		public Chainable MapUnknownRoutes( JsFunction config, string replaceRoute )
		{
			return default(Chainable);
		}

		/// <summary>
		/// Resets the router by removing handlers, routes, event handlers and previously configured options.
		/// </summary>
		public void Reset()
		{}

		/// <summary>
		/// Makes all configured routes and/or module ids relative to a certain base url.
		/// </summary>
		/// <param name="settings">If string, the value is used as the base for routes and module ids. If an object, you can specify route and/or moduleId separately.</param>
		public void MakeRelative( string settings )
		{}

		/// <summary>
		/// Makes all configured routes and/or module ids relative to a certain base url.
		/// </summary>
		/// <param name="settings">If string, the value is used as the base for routes and module ids. If an object, you can specify route and/or moduleId separately.</param>
		public void MakeRelative( object settings )
		{}

		/// <summary>
		/// Creates a child router.
		/// </summary>
		/// <returns></returns>
		public Router CreateChildRouter()
		{
			return default(Router);
		}

		/// <summary>
		/// Activates the router and the underlying history tracking mechanism.
		/// </summary>
		/// <returns>A promise that resolves when the router is ready.</returns>
		public Promise Activate()
		{
			return default(Promise);
		}

		/// <summary>
		/// Disable history, perhaps temporarily. Not useful in a real app, but possibly useful for unit testing Routers.
		/// </summary>
		public void Deactivate()
		{}
	}
}
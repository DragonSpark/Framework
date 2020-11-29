using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

// ReSharper disable All
#pragma warning disable 8618

namespace DragonSpark.Presentation.Components.Routing
{
	/// <summary>
	/// ATTRIBUTION: https://github.com/ShaunCurtis/CEC.Routing/blob/master/CEC.Routing/Routing/RecordRouter.cs
	/// </summary>
	/// <summary>
	/// A customized Router to handle unsaved pages
	/// </summary>
	public class Router : IComponent, IHandleAfterRender, IDisposable
	{
		static string StringUntilAny(string str, char[] chars)
		{
			var firstIndex = str.IndexOfAny(chars);
			return firstIndex < 0
				       ? str
				       : str.Substring(0, firstIndex);
		}

		readonly static char[] _queryOrHashStart = {'?', '#'};

		string          _baseUri;
		string          _locationAbsolute;
		ILogger<Router> _logger;
		bool            _navigationInterceptionEnabled;

		RenderHandle _renderHandle;

		[Inject]
		NavigationManager NavigationManager { get; set; }

		[Inject]
		RouterSession Session { get; set; }

		[Inject]
		INavigationInterception NavigationInterception { get; set; }

		[Inject]
		ILoggerFactory LoggerFactory { get; set; }

		/// <summary>
		/// Gets or sets the assembly that should be searched for components matching the URI.
		/// </summary>
		[Parameter]
		public Assembly AppAssembly { get; set; }

		/// <summary>
		/// Gets or sets a collection of additional assemblies that should be searched for components
		/// that can match URIs.
		/// </summary>
		[Parameter]
		public IEnumerable<Assembly> AdditionalAssemblies { get; set; }

		/// <summary>
		/// Gets or sets the content to display when no match is found for the requested route.
		/// </summary>
		[Parameter]
		public RenderFragment NotFound { get; set; }

		/// <summary>
		/// Gets or sets the content to display when a match is found for the requested route.
		/// </summary>
		[Parameter]
		public RenderFragment<RouteData> Found { get; set; }

		RouteTable Routes { get; set; }

		RenderFragment? Fragment(RouteContext context, bool isNavigationIntercepted)
		{
			var route = Routes.Route(context);
			if (route != null)
			{
				Log.NavigatingToComponent(_logger, route.PageType, context.Path, _baseUri);
				var result = Found(route);
				return result;
			}
			else if (!isNavigationIntercepted)
			{
				Log.DisplayingNotFound(_logger, context.Path, _baseUri);
				return NotFound;
			}

			return null;
		}

		string Path => StringUntilAny(NavigationManager.ToBaseRelativePath(_locationAbsolute), _queryOrHashStart);

		RenderState Render(bool intercepted)
		{
			var context  = new RouteContext(Path);
			var fragment = Fragment(context, intercepted);
			var result   = new RenderState(fragment, context.Path);
			return result;
		}

		readonly struct RenderState
		{
			public RenderState(RenderFragment? fragment, string path)
			{
				Fragment = fragment;
				Path     = path;
			}

			public RenderFragment? Fragment { get; }

			public string Path { get; }
		}

		void Refresh(bool intercepted)
		{
			var render = Render(intercepted);
			if (render.Fragment != null)
			{
				_renderHandle.Render(render.Fragment);
			}
			else
			{
				Log.NavigatingToExternalUri(_logger, _locationAbsolute, render.Path, _baseUri);
				NavigationManager.NavigateTo(_locationAbsolute, true);
			}
		}

		void OnLocationChanged(object? sender, LocationChangedEventArgs args)
		{
			_locationAbsolute = args.Location;

			// SCC ADDED - SessionState Check for Unsaved Page
			if (_renderHandle.IsInitialized && Routes != null && !Session.HasChanges)
			{
				// Clear the Active Component - the next page will load itself if required
				Session.ActiveComponent        = null;
				Session.NavigationCancelledUrl = null;
				Refresh(args.IsNavigationIntercepted);
			}
			// SCC ADDED - Trigger a Navigation Cancelled Event on the SessionStateService
			else if (Session.PageUrl.Equals(_locationAbsolute, StringComparison.CurrentCultureIgnoreCase))
			{
				// Cancel routing
				Session.TriggerNavigationCancelledEvent();
			}
			else
			{
				//  we're cancelling routing, but the Navigation Manager is current set to the aborted page
				//  so we set the navigation cancelled url so the page can navigate to it if necessary
				//  and do a dummy trip through the Navigation Manager again to set this back to the original page
				Session.NavigationCancelledUrl = NavigationManager.Uri;
				NavigationManager.NavigateTo(Session.PageUrl);
			}

			// Get the Page Uri minus any query string
			var pageurl = NavigationManager.Uri.Contains("?")
				              ? NavigationManager.Uri.Substring(0, NavigationManager.Uri.IndexOf("?"))
				              : NavigationManager.Uri;

			if (Session.LastPageUrl?.Equals(pageurl, StringComparison.CurrentCultureIgnoreCase) ?? false)
			{
				Session.TriggerIntraPageNavigation();
			}

			Session.LastPageUrl = pageurl;
		}

		/// <inheritdoc />
		public void Attach(RenderHandle renderHandle)
		{
			_logger                           =  LoggerFactory.CreateLogger<Router>();
			_renderHandle                     =  renderHandle;
			_baseUri                          =  NavigationManager.BaseUri;
			_locationAbsolute                 =  NavigationManager.Uri;
			NavigationManager.LocationChanged += OnLocationChanged;
		}

		/// <inheritdoc />
		public Task SetParametersAsync(ParameterView parameters)
		{
			parameters.SetParameterProperties(this);

			if (AppAssembly == null)
			{
				throw new
					InvalidOperationException($"The {nameof(Microsoft.AspNetCore.Components.Routing.Router)} component requires a value for the parameter {nameof(AppAssembly)}.");
			}

			// Found content is mandatory, because even though we could use something like <RouteView ...> as a
			// reasonable default, if it's not declared explicitly in the template then people will have no way
			// to discover how to customize this (e.g., to add authorization).
			if (Found == null)
			{
				throw new
					InvalidOperationException($"The {nameof(Microsoft.AspNetCore.Components.Routing.Router)} component requires a value for the parameter {nameof(Found)}.");
			}

			// NotFound content is mandatory, because even though we could display a default message like "Not found",
			// it has to be specified explicitly so that it can also be wrapped in a specific layout
			if (NotFound == null)
			{
				throw new
					InvalidOperationException($"The {nameof(Microsoft.AspNetCore.Components.Routing.Router)} component requires a value for the parameter {nameof(NotFound)}.");
			}

			var assemblies = AdditionalAssemblies == null
				                 ? new[] {AppAssembly}
				                 : new[] {AppAssembly}.Concat(AdditionalAssemblies);
			Routes = RouteTableFactory.Create(assemblies);
			Refresh(false);
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public void Dispose()
		{
			GC.SuppressFinalize(this);
			NavigationManager.LocationChanged -= OnLocationChanged;
		}

		Task IHandleAfterRender.OnAfterRenderAsync()
		{
			if (!_navigationInterceptionEnabled)
			{
				_navigationInterceptionEnabled = true;
				return NavigationInterception.EnableNavigationInterceptionAsync();
			}

			return Task.CompletedTask;
		}

		static class Log
		{
			internal static void DisplayingNotFound(ILogger logger, string path, string baseUri)
			{
				_displayingNotFound(logger, path, baseUri, null);
			}

			internal static void NavigatingToComponent(ILogger logger, Type componentType, string path, string baseUri)
			{
				_navigatingToComponent(logger, componentType, path, baseUri, null);
			}

			internal static void NavigatingToExternalUri(ILogger logger, string externalUri, string path,
			                                             string baseUri)
			{
				_navigatingToExternalUri(logger, externalUri, path, baseUri, null);
			}

			readonly static Action<ILogger, string, string, Exception?> _displayingNotFound =
				LoggerMessage.Define<string, string>(LogLevel.Debug, new EventId(1, "DisplayingNotFound"),
				                                     $"Displaying {nameof(NotFound)} because path '{{Path}}' with base URI '{{BaseUri}}' does not match any component route");

			readonly static Action<ILogger, Type, string, string, Exception?> _navigatingToComponent =
				LoggerMessage.Define<Type, string, string>(LogLevel.Debug, new EventId(2, "NavigatingToComponent"),
				                                           "Navigating to component {ComponentType} in response to path '{Path}' with base URI '{BaseUri}'");

			readonly static Action<ILogger, string, string, string, Exception?> _navigatingToExternalUri =
				LoggerMessage.Define<string, string, string>(LogLevel.Debug, new EventId(3, "NavigatingToExternalUri"),
				                                             "Navigating to non-component URI '{ExternalUri}' in response to path '{Path}' with base URI '{BaseUri}'");
		}
	}
}
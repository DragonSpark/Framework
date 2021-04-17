using DragonSpark.Compose;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// ReSharper disable All
#pragma warning disable 8618

namespace DragonSpark.Presentation.Components.Routing
{
	/// <summary>
	/// ATTRIBUTION: https://github.com/ShaunCurtis/CEC.Routing/blob/master/CEC.Routing/Services/RouterSessionService.cs
	///
	/// Service Class used by the Record Router to track routing operations for the current user session
	/// Needs to be loaded as a Scoped Service
	///
	/// </summary>
	public class RouterSession
	{
		readonly IJSRuntime _runtime;

		public RouterSession(IJSRuntime runtime) => _runtime = runtime;

		/// <summary>
		/// Property containing the currently loaded component if set
		/// </summary>
		public IRoutingComponent? ActiveComponent { get; set; }

		/// <summary>
		/// Boolean to check if the Router Should Navigate
		/// </summary>
		public bool HasChanges => ActiveComponent?.HasChanges ?? false;

		/// <summary>
		/// Url of Current Page being navigated from
		/// </summary>
		public string PageUrl => ActiveComponent?.PageUrl ?? string.Empty;

		/// <summary>
		/// Url of the previous page
		/// </summary>
		public string? LastPageUrl { get; set; }

		/// <summary>
		/// Url of the navigation cancelled page
		/// </summary>
		public string? NavigationCancelledUrl { get; set; }

		bool ExitShowState { get; set; }

		/// <summary>
		/// Method to trigger the NavigationCancelled Event
		/// </summary>
		public void TriggerNavigationCancelledEvent() => NavigationCanceled?.Invoke(this, EventArgs.Empty);

		/// <summary>
		/// Method to trigger the IntraPageNavigation Event
		/// </summary>
		public void TriggerIntraPageNavigation() => IntraPageNavigation?.Invoke(this, EventArgs.Empty);

		readonly HashSet<object> _active = new HashSet<object>();

		public ValueTask Register(object instance)
			=> _active.Add(instance) ? SetPageExitCheck(true) : Task.CompletedTask.ToOperation();

		public ValueTask Unregister(object instance)
		{
			_active.Remove(instance);

			return SetPageExitCheck(_active.Count > 0);
		}

		async ValueTask SetPageExitCheck(bool show)
		{
			if (show != ExitShowState)
			{
				ExitShowState = show;
				await _runtime.InvokeVoidAsync("cec_setEditorExitCheck", show);
			}
		}

		/// <summary>
		/// Event to notify Navigation Cancellation
		/// </summary>
		public event EventHandler NavigationCanceled;

		/// <summary>
		/// Event to notify that Intra Page Navigation has taken place
		/// useful when using Querystring controlled pages
		/// </summary>
		public event EventHandler IntraPageNavigation;
	}
}
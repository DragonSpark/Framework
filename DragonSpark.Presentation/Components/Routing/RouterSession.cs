﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// ReSharper disable All
#pragma warning disable 8618

namespace DragonSpark.Presentation.Components.Routing;

/// <summary>
/// ATTRIBUTION: https://github.com/ShaunCurtis/CEC.Routing/blob/master/CEC.Routing/Services/RouterSessionService.cs
///
/// Service Class used by the Record Router to track routing operations for the current user session
/// Needs to be loaded as a Scoped Service
///
/// </summary>
public class RouterSession
{
	readonly ISetPageExitCheck _runtime;

	readonly Stack<IRoutingComponent> _active = new Stack<IRoutingComponent>();

	public RouterSession(ISetPageExitCheck runtime) => _runtime = runtime;

	/// <summary>
	/// Property containing the currently loaded component if set
	/// </summary>
	public IRoutingComponent? ActiveComponent { get; private set; }

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
	public void TriggerSamePageNavigation() => SamePageNavigation?.Invoke(this, EventArgs.Empty);

	/// <summary>
	/// Method to trigger the IntraPageNavigation Event
	/// </summary>
	public void TriggerNavigation() => Navigation?.Invoke(this, EventArgs.Empty);

	public ValueTask Register(IRoutingComponent instance)
	{
		if (!_active.Contains(instance))
		{
			_active.Push(ActiveComponent = instance);
			return UpdateExitState();
		}

		return ValueTask.CompletedTask;
	}

	public ValueTask Unregister(IRoutingComponent instance)
	{
		var routingComponent = _active.Pop();
		if (routingComponent != instance)
		{
			throw new InvalidOperationException("Unexpected routing component encountered");
		}

		var more = _active.TryPeek(out var current);
		ActiveComponent = more ? current : null;

		return UpdateExitState();
	}

	public ValueTask UpdateExitState() => SetPageExitCheck(_active.Any(x => x.EnablePrompt && x.HasChanges));

	ValueTask SetPageExitCheck(bool show)
	{
		if (show != ExitShowState)
		{
			ExitShowState = show;
			return _runtime.Get(show);
		}

		return ValueTask.CompletedTask;
	}

	public ValueTask Clear()
	{
		ActiveComponent = null;
		_active.Clear();
		return SetPageExitCheck(false);
	}

	/// <summary>
	/// Event to notify Navigation Cancellation
	/// </summary>
	public event EventHandler NavigationCanceled;

	/// <summary>
	/// Event to notify that Intra Page Navigation has taken place
	/// useful when using Querystring controlled pages
	/// </summary>
	public event EventHandler SamePageNavigation;

	/// <summary>
	/// Called when navigation occurs that is not same page
	/// </summary>
	public event EventHandler Navigation;
}
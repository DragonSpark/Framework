using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Components;
using System;

namespace DragonSpark.Application.AspNet.Navigation;

public class Navigation : Command
{
	protected Navigation(NavigationManager navigation, Func<string> path, bool force = false)
		: base(new Navigate(navigation, force).Then().Bind(path)) {}

	protected Navigation(NavigationManager navigation, string path, bool force = false)
		: base(new Navigate(navigation, force).Then().Bind(path)) {}
}

public class Navigation<T> : Command<T>
{
	// ReSharper disable once TooManyDependencies
	protected Navigation(NavigationManager navigation, Func<T, string> path, bool force = false, bool replace = false)
		: base(path.Start().Terminate(new Navigate(navigation, force, replace))) {}
}
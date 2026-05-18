using Microsoft.AspNetCore.Components;

namespace DragonSpark.Application.AspNet.Security.Identity.Model;

sealed class NavigateToSignOut : Application.AspNet.Navigation.Navigation, INavigateToSignOut
{
	public NavigateToSignOut(NavigationManager navigation, SignOutPath path) : base(navigation, path) {}
}
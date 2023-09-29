using Microsoft.AspNetCore.Components;

namespace DragonSpark.Application.Security.Identity.Model;

sealed class NavigateToSignOut : Navigation.Navigation, INavigateToSignOut
{
	public NavigateToSignOut(NavigationManager navigation) : base(navigation, SignOutPath.Default, true) {}
}
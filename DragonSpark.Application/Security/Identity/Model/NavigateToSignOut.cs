using DragonSpark.Model;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Model
{
	sealed class NavigateToSignOut : Navigation.Navigation, INavigateToSignOut
	{
		public NavigateToSignOut(NavigationManager navigation) : base(navigation, "/Identity/Account/LogOut", true) {}

		public void Execute(ClaimsPrincipal parameter)
		{
			Execute(None.Default);
		}
	}
}
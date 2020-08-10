using DragonSpark.Application.Components;
using DragonSpark.Model;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;

namespace DragonSpark.Application.Security
{
	sealed class NavigateToSignOut : Navigation, INavigateToSignOut
	{
		public NavigateToSignOut(NavigationManager navigation) : base(navigation, "/Identity/Account/LogOut", true) {}

		public void Execute(ClaimsPrincipal parameter)
		{
			base.Execute(None.Default);
		}
	}
}
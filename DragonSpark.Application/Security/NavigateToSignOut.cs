using DragonSpark.Application.Components;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Application.Security
{
	sealed class NavigateToSignOut : Navigation, INavigateToSignOut
	{
		public NavigateToSignOut(NavigationManager navigation) : base(navigation, "/Identity/Account/LogOut", true) {}
	}
}

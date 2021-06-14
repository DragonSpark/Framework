using DragonSpark.Application.Navigation;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Security.Identity
{
	public class RefreshExternalLogin : Navigation
	{
		protected RefreshExternalLogin(NavigationManager navigation, CurrentExternalLogin current)
			: base(navigation, current.Get, true) {}
	}
}
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Application.Navigation.Security.Identity;

public class RefreshExternalLogin : Navigation
{
	protected RefreshExternalLogin(NavigationManager navigation, CurrentExternalLogin current)
		: base(navigation, current.Get, true) {}
}
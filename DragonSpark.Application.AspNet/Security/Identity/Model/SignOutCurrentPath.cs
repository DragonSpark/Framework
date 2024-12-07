using DragonSpark.Application.Navigation;
using DragonSpark.Compose;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Application.Security.Identity.Model;

public sealed class SignOutCurrentPath : Navigation.Navigation
{
	public SignOutCurrentPath(NavigationManager navigation, CurrentPath path) 
		: base(navigation, SignOutReturnPath.Default.Then().Bind(path), true) {}
}
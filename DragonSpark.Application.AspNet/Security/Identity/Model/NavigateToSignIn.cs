using DragonSpark.Application.AspNet.Navigation;
using DragonSpark.Application.AspNet.Navigation.Security;
using DragonSpark.Compose;
using DragonSpark.Model;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Application.AspNet.Security.Identity.Model;

public sealed class NavigateToSignIn : Navigation<None>
{
	public NavigateToSignIn(NavigationManager navigation, RedirectLoginPath path)
		: base(navigation, path.Then().Accept<None>(), true) {}
}
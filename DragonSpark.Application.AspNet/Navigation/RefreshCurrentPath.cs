using Microsoft.AspNetCore.Components;

namespace DragonSpark.Application.AspNet.Navigation;

public sealed class RefreshCurrentPath : Navigation
{
	public RefreshCurrentPath(NavigationManager navigation, CurrentPath path) : base(navigation, path.Get, true) {}
}
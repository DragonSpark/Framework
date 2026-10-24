using DragonSpark.Compose;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Application.AspNet.Navigation;

public sealed class CurrentRootPath : Text.Text
{
	public CurrentRootPath(NavigationManager manager) : base(RootPath.Default.Then().Bind(manager)) {}
}
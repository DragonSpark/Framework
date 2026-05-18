using DragonSpark.Application.AspNet.Navigation;
using DragonSpark.Compose;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Application.AspNet.Security.Identity.Model;

sealed class SignOutCurrentPath : Application.AspNet.Navigation.Navigation
{
    public SignOutCurrentPath(NavigationManager navigation, CurrentPath path)
        : base(navigation, SignOutReturnPath.Default.Then().Bind(path)) {}
}
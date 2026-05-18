using DragonSpark.Model;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Application.AspNet.Security.Identity.Model;

sealed class RedirectToSignOut : IRedirectToSignOut
{
    readonly NavigationManager  _manager;
    readonly string             _path;
    readonly SignOutCurrentPath _previous;

    public RedirectToSignOut(NavigationManager manager, SignOutPath path, SignOutCurrentPath previous)
        : this(manager, path.Get(), previous) {}

    public RedirectToSignOut(NavigationManager manager, string path, SignOutCurrentPath previous)
    {
        _manager  = manager;
        _path     = path;
        _previous = previous;
    }

    public void Execute(None parameter)
    {
        if (!_manager.RootPath().StartsWith(_path))
        {
            _previous.Execute(parameter);    
        }
    }
}
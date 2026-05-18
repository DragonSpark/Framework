using DragonSpark.Text;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Application.AspNet.Navigation;

sealed class RootPath : IFormatter<NavigationManager>
{
    public static RootPath Default { get; } = new();

    RootPath() : this(Path.Default) {}

    readonly IFormatter<NavigationManager> _previous;

    public RootPath(IFormatter<NavigationManager> previous) => _previous = previous;

    public string Get(NavigationManager parameter) => $"/{_previous.Get(parameter)}";
}
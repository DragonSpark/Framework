using DragonSpark.Text;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Application.AspNet.Navigation;

sealed class Path : IFormatter<NavigationManager>
{
    public static Path Default { get; } = new();

    Path() {}

    public string Get(NavigationManager parameter) => parameter.ToBaseRelativePath(parameter.Uri);
}
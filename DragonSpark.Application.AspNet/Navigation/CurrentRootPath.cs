using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Application.AspNet.Navigation;

public sealed class CurrentRootPath : Text.Text
{
    public CurrentRootPath(NavigationManager manager) : base(RootPath.Default.Then().Bind(manager)) {}
}

public readonly record struct IsOnInput(NavigationManager Subject, string Path);
sealed class IsOn : ICondition<IsOnInput>
{
    public static IsOn Default { get; } = new();

    IsOn() {}
    
    public bool Get(IsOnInput parameter)
    {
        var (subject, path) = parameter;
        var result = subject.Path().StartsWith(path.TrimStart('/'));
        return result;
    }
}
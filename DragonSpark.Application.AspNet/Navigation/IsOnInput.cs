using Microsoft.AspNetCore.Components;

namespace DragonSpark.Application.AspNet.Navigation;

public readonly record struct IsOnInput(NavigationManager Subject, string Path);
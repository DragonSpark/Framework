using DragonSpark.Model.Results;
using LightInject;
using LightInject.Microsoft.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Composition.Construction;

sealed class DefaultOptions : Deferred<ContainerOptions>
{
    public static DefaultOptions Default { get; } = new();

    DefaultOptions() : base(() => ContainerOptions.Default.Clone().WithMicrosoftSettings().WithAspNetCoreSettings()) {}
}
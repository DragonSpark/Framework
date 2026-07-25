using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Alterations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace DragonSpark.Application.Hosting.Server;

sealed class DefaultApplicationConfiguration : ICommand<IApplicationBuilder>
{
    public static DefaultApplicationConfiguration Default { get; } = new();

    DefaultApplicationConfiguration()
        : this(CoreApplicationConfiguration.Default, EndpointConfiguration.Default.Execute) {}

    readonly IAlteration<IApplicationBuilder> _previous;
    readonly Action<IEndpointRouteBuilder>    _endpoints;

    public DefaultApplicationConfiguration(IAlteration<IApplicationBuilder> previous,
                                           Action<IEndpointRouteBuilder> endpoints)
    {
        _previous  = previous;
        _endpoints = endpoints;
    }

    public void Execute(IApplicationBuilder parameter)
    {
        _previous.Get(parameter).UseEndpoints(_endpoints);
    }
}
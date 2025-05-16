using System;
using DragonSpark.Compose;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Communication.Http;

sealed class AddHttpClient<T> : IAddClient<T> where T : Options
{
    public static AddHttpClient<T> Default { get; } = new();

    AddHttpClient() : this(ConfigurePrimaryActions.Default.Then().Select(ConfigureDelegatingHandlers.Default).Get) {}

    readonly Func<IHttpClientBuilder, IHttpClientBuilder> _configure;

    public AddHttpClient(Func<IHttpClientBuilder, IHttpClientBuilder> configure) => _configure = configure;

    public IServiceCollection Get(AddHttpClientInput<T> parameter)
    {
        var (result, options, client, configure) = parameter;
        var start = client(result);
        var next = start.When(options.Configure, _configure)
                        .ConfigureHttpClient((_, c) => c.BaseAddress = new(options.Address));
        _ = configure(next, options);
        return result;
    }
}
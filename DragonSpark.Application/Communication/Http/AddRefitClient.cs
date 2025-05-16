using System;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace DragonSpark.Application.Communication.Http;

sealed class AddRefitClient<T, TOptions> : ISelect<AddRefitClientInput<TOptions>, IServiceCollection>
    where TOptions : Options where T : class
{
    public static AddRefitClient<T, TOptions> Default { get; } = new();

    AddRefitClient() : this(AddHttpClient<TOptions>.Default) {}

    readonly IAddClient<TOptions> _previous;

    public AddRefitClient(IAddClient<TOptions> previous) => _previous = previous;

    public IServiceCollection Get(AddRefitClientInput<TOptions> parameter)
    {
        var (subject, options, settings, configure) = parameter;
        return _previous.Get(new(subject, options, new AddRefitClient<T>(settings).Get, configure));
    }
}

sealed class AddRefitClient<T> : ISelect<IServiceCollection, IHttpClientBuilder> where T : class
{
    readonly Func<IServiceProvider, RefitSettings> _settings;

    public AddRefitClient(Action<IServiceProvider, RefitSettings> settings) : this(new ComposeSettings(settings).Get) {}

    public AddRefitClient(Func<IServiceProvider, RefitSettings> settings) => _settings = settings;

    public IHttpClientBuilder Get(IServiceCollection parameter) => parameter.AddRefitClient<T>(_settings);
}
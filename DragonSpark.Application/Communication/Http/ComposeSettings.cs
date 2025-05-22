using System;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace DragonSpark.Application.Communication.Http;

sealed class ComposeSettings : ISelect<IServiceProvider, RefitSettings>
{
    readonly Action<IServiceProvider, RefitSettings> _configure;

    public ComposeSettings(Action<IServiceProvider, RefitSettings> configure) => _configure = configure;

    public RefitSettings Get(IServiceProvider parameter)
    {
        var serializer = parameter.GetService<IHttpContentSerializer>();
        var settings   = serializer is not null ? new() { ContentSerializer = serializer } : new RefitSettings();
        var provider   = parameter.GetService<IRefitAccessTokenProvider>();

        settings.AuthorizationHeaderValueGetter
            = provider is not null ? provider.Get : settings.AuthorizationHeaderValueGetter;

        _configure(parameter, settings);
        return settings;
    }
}
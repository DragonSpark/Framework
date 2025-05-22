using System;
using DragonSpark.Model.Selection.Alterations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

namespace DragonSpark.Application.Communication.Http;

class ConfigureActions : IAlteration<IHttpClientBuilder>
{
    readonly Action<HttpMessageHandlerBuilder> _add;

    protected ConfigureActions(Action<HttpMessageHandlerBuilder> add) => _add = add;

    public IHttpClientBuilder Get(IHttpClientBuilder parameter)
    {
        parameter.Services.Configure(parameter.Name,
                                     (HttpClientFactoryOptions o) => o.HttpMessageHandlerBuilderActions.Add(_add));

        return parameter;
    }
}
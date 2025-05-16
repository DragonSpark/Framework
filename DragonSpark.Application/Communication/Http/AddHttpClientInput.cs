using System;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Communication.Http;

public readonly record struct AddHttpClientInput<TOptions>(
    IServiceCollection Subject,
    TOptions Options,
    Func<IServiceCollection, IHttpClientBuilder> Client,
    Func<IHttpClientBuilder, TOptions, IHttpClientBuilder> Configure) where TOptions : class;
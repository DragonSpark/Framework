using System;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace DragonSpark.Application.Communication.Http;

public readonly record struct AddRefitClientInput<T>(
    IServiceCollection Subject,
    T Options,
    Action<IServiceProvider, RefitSettings> Settings,
    Func<IHttpClientBuilder, T, IHttpClientBuilder> Configure);
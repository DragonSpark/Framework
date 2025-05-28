using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;

namespace DragonSpark.Application.Communication.Http;

public readonly record struct AddRefitClientInput<T>(
    IServiceCollection Subject,
	string Name,
    T Options,
    Action<IServiceProvider, RefitSettings> Settings,
    Func<IHttpClientBuilder, T, IHttpClientBuilder> Configure);
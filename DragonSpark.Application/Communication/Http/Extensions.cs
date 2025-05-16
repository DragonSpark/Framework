using System;
using DragonSpark.Compose;
using DragonSpark.Composition;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace DragonSpark.Application.Communication.Http;

public static class Extensions
{
    public static T When<T>(this T @this, bool @true, Func<T, T> alter) => @true ? alter(@this) : @this;

    public static IServiceCollection AddRefitApi<T>(this IServiceCollection @this)
        where T : class
        => @this.AddRefitApi<T, Options>();

    public static IServiceCollection AddRefitApi<T>(this IServiceCollection @this, Options options)
        where T : class
        => @this.AddRefitApi<T, Options>(options);

    public static IServiceCollection AddRefitApi<T>(this IServiceCollection @this, Options options,
                                                    Action<IServiceProvider, RefitSettings> settings)
        where T : class
        => @this.AddRefitApi<T, Options>(options, settings);

    // ReSharper disable once TooManyArguments
    public static IServiceCollection AddRefitApi<T>(this IServiceCollection @this, Options options,
                                                    Action<IServiceProvider, RefitSettings> settings,
                                                    Func<IHttpClientBuilder, Options, IHttpClientBuilder> configure)
        where T : class
        => @this.AddRefitApi<T, Options>(options, settings, configure);

    public static IServiceCollection AddRefitApi<T, TOptions>(this IServiceCollection @this)
        where TOptions : Options where T : class
        => @this.AddRefitApi<T, TOptions>(A.Type<TOptions>().IsInterface
                                              ? A.Type<TOptions>().Name[1..]
                                              : A.Type<TOptions>().Name);

    public static IServiceCollection AddRefitApi<T, TOptions>(this IServiceCollection @this, string name)
        where TOptions : Options where T : class
        => @this.AddRefitApi<T, TOptions>(@this.Section<TOptions>(name).Verify());

    public static IServiceCollection AddRefitApi<T, TOptions>(this IServiceCollection @this, TOptions options)
        where TOptions : Options where T : class
        => @this.AddRefitApi<T, TOptions>(options, (_, _) => {});

    public static IServiceCollection AddRefitApi<T, TOptions>(this IServiceCollection @this, TOptions options,
                                                              Action<IServiceProvider, RefitSettings> settings)
        where TOptions : Options where T : class
        => @this.AddRefitApi<T, TOptions>(options, settings, (x, _) => x);

    // ReSharper disable once TooManyArguments
    public static IServiceCollection AddRefitApi<T, TOptions>(
        this IServiceCollection @this, TOptions options,
        Action<IServiceProvider, RefitSettings> settings,
        Func<IHttpClientBuilder, TOptions, IHttpClientBuilder> configure)
        where TOptions : Options where T : class
        => AddRefitClient<T, TOptions>.Default.Get(new(@this, options, settings, configure));
}
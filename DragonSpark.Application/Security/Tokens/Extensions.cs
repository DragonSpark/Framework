using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Security.Tokens;

public static class Extensions
{
    public static IHttpClientBuilder WithDeviceAuthorization(this IHttpClientBuilder @this)
        => @this.AddHttpMessageHandler<DevicePoPHandler>();
}
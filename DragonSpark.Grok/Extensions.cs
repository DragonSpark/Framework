using DragonSpark.Compose;
using DragonSpark.Grok.Chat;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Grok;

public static class Extensions
{
    public static IServiceCollection AddGrokChat(this IServiceCollection @this)
        => Registrations.Default.Parameter(@this);
}
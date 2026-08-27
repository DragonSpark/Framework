using System.Collections.Immutable;
using DragonSpark.Compose;
using DragonSpark.Contracts.General.Chat;
using DragonSpark.Grok.Chat;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Grok;

public static class Extensions
{
    public static IServiceCollection AddGrokChat(this IServiceCollection @this)
	    => Chat.Registrations.Default.Parameter(@this);
    public static IServiceCollection AddGrokImage(this IServiceCollection @this)
	    => Image.Registrations.Default.Parameter(@this);

    public static WithSuggestionsResult WithSuggestions(this ImmutableArray<ChatMessage> @this)
        => Chat.WithSuggestions.Default.Get(@this);
}
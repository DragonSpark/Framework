using System.Buffers;
using System.Collections.Immutable;
using DragonSpark.Application.Runtime.Objects;
using DragonSpark.Compose;
using DragonSpark.Contracts.General.Chat;
using DragonSpark.Model.Selection;
using NetFabric.Hyperlinq;

namespace DragonSpark.Grok.Chat;

public sealed class WithSuggestions : ISelect<ImmutableArray<ChatMessage>, WithSuggestionsResult>
{
    public static WithSuggestions Default { get; } = new();

    WithSuggestions()
        : this(SuggestionToolName.Default, DefaultSerializer<SuggestionsResult>.Default.Parser.Get,
               ArrayPool<ChatMessage>.Shared) {}

    readonly string                          _name;
    readonly Func<string, SuggestionsResult> _suggestions;
    readonly ArrayPool<ChatMessage>          _pool;

    public WithSuggestions(string name, Func<string, SuggestionsResult> suggestions, ArrayPool<ChatMessage> pool)
    {
        _name        = name;
        _suggestions = suggestions;
        _pool        = pool;
    }

    public WithSuggestionsResult Get(ImmutableArray<ChatMessage> parameter)
    {
        using var all = parameter.AsValueEnumerable().ToArray(_pool);
        using var tools = parameter.OfType<ToolMessage>()
                                   .Introduce(_name)
                                   .AsValueEnumerable()
                                   .Where(x => x.Item1.ToolName == x.Item2)
                                   .Select(x => x.Item1)
                                   .ToArray(ArrayPool<ToolMessage>.Shared);
        var suggestions = tools.LastOrDefault() is {} last ? _suggestions(last.Content) : null;
        return new ([..all.Except(tools)], suggestions?.Suggestions.ToImmutableArray());
    }
}
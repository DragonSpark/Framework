using System.Text.Json;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Grok.Chat;

sealed class Suggestions : IExecute<SuggestionsResult>
{
    public static Suggestions Default { get; } = new();

    Suggestions() {}

    public ValueTask<string> Get(Stop<SuggestionsResult> parameter)
    {
        var (input, _) = parameter;

        return JsonSerializer.Serialize(input).ToOperation();
    }
}
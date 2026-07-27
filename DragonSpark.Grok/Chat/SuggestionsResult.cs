using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DragonSpark.Grok.Chat;

[method: JsonConstructor]
public sealed record SuggestionsResult(
    string Result,
    [property:
        Description("3 to 5 natural, concise follow-up prompt suggestions based on the current conversation. Each suggestion should be something the user might actually type next (max 75 characters).")]
    List<string> Suggestions)
{
    public SuggestionsResult(string result) : this(result, []) {}
}
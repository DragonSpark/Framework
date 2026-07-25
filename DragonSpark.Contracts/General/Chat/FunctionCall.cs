using System.Text.Json.Serialization;

namespace DragonSpark.Contracts.General.Chat;

public sealed record FunctionCall(
    string Name,
    [property: JsonConverter(typeof(ArgumentsStringToDictionaryConverter))]
    Dictionary<string, object> Arguments);
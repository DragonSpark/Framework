using System.Text.Json.Serialization;

namespace DragonSpark.Contracts.General.Chat;

[JsonConverter(typeof(ToolCallConverter))]
public abstract record ToolCall(string Id);
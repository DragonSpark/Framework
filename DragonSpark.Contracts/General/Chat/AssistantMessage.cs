using System.Collections.Generic;

namespace DragonSpark.Contracts.General.Chat;

public sealed record AssistantMessage(string Content, List<ToolCall>? ToolCalls = null) : TextMessage(Content);
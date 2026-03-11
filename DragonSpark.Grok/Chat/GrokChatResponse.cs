using System.Collections.Generic;

namespace DragonSpark.Grok.Chat;

public sealed record GrokChatResponse(List<Choice> Choices);
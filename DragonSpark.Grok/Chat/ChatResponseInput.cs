using DragonSpark.Model.Sequences;

namespace DragonSpark.Grok.Chat;

public readonly record struct ChatResponseInput(ChatModelInput Input, Array<Tool> Tools);
using DragonSpark.Contracts.General.Chat;
using DragonSpark.Model.Operations.Allocated.Stop;

namespace DragonSpark.Grok.Chat;

public interface IChatResponse : IAllocated<ChatResponseInput, ChatMessage>;
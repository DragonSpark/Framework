using DragonSpark.Contracts.General;
using DragonSpark.Model.Operations.Allocated.Stop;

namespace DragonSpark.Grok.Chat;

public interface IChatResult : IAllocated<ChatModelInput, ChatMessage>;
using System.Collections.Immutable;
using DragonSpark.Contracts.General;
using DragonSpark.Model.Operations.Allocated.Stop;

namespace DragonSpark.Grok.Chat;

public interface IChat : IAllocated<ChatModelInput, ImmutableArray<ChatMessage>>;
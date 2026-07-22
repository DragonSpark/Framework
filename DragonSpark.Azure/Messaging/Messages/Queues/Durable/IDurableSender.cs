using DragonSpark.Model.Selection;

namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

public interface IDurableSender : IDispatch, ISelect<ScopedInput, IScopedDispatch>;
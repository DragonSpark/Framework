using DragonSpark.Model.Selection;

namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

public interface ISender : IDispatch, ISelect<ScopedInput, ISend>;
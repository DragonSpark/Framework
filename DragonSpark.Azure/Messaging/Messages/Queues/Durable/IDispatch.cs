using DragonSpark.Contracts.Messaging;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

public interface IDispatch : IStopAware<MessageProperties>;
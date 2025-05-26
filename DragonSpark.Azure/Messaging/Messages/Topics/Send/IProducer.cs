using Azure.Messaging.ServiceBus;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Azure.Messaging.Messages.Topics.Send;

public interface IProducer : IStopAware<ServiceBusMessage>;
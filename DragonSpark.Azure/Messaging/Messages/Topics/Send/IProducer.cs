using Azure.Messaging.ServiceBus;
using DragonSpark.Model.Operations;

namespace DragonSpark.Azure.Messaging.Messages.Topics.Send;

public interface IProducer : IOperation<ServiceBusMessage>;
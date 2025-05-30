using Azure.Messaging.ServiceBus;
using DragonSpark.Model.Operations.Results;

namespace DragonSpark.Azure.Messaging.Messages.Queues;

public interface IPeek : IResulting<ServiceBusReceivedMessage?>;
using Azure.Messaging.ServiceBus;
using DragonSpark.Model.Results;

namespace DragonSpark.Azure.Messaging.Messages.Queues;

public interface IClient : IResult<ServiceBusClient>;
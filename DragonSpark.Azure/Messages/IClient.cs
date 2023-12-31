using Azure.Messaging.ServiceBus;
using DragonSpark.Model.Results;

namespace DragonSpark.Azure.Messages;

public interface IClient : IResult<ServiceBusClient>;
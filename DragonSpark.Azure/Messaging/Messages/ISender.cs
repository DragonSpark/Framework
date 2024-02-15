using Azure.Messaging.ServiceBus;
using DragonSpark.Model.Results;

namespace DragonSpark.Azure.Messaging.Messages;

public interface ISender : IResult<ServiceBusSender>;
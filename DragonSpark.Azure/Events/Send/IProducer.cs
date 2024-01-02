using Azure.Messaging.EventHubs.Producer;
using DragonSpark.Model.Results;

namespace DragonSpark.Azure.Events.Send;

public interface IProducer : IResult<EventHubProducerClient>;
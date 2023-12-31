using Azure.Messaging.EventHubs.Producer;
using DragonSpark.Model.Results;

namespace DragonSpark.Azure.Events;

public interface IProducer : IResult<EventHubProducerClient>;
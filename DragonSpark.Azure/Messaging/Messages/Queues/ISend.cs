using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Azure.Messaging.Messages.Queues;

public interface ISend : IStopAware<string>;
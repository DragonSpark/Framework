using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Azure.Messaging.Messages.Topics.Receive;

public sealed class Handlers : SynchronizedCollection<IStopAware<object>>;
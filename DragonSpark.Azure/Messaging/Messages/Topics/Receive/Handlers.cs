using DragonSpark.Model.Operations.Stop;
using System.Collections.Generic;

namespace DragonSpark.Azure.Messaging.Messages.Topics.Receive;

public sealed class Handlers : SynchronizedCollection<IStopAware<object>>;
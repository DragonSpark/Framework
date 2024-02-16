using DragonSpark.Model.Operations;
using System.Collections.Generic;

namespace DragonSpark.Azure.Messaging.Messages.Topics.Receive;

public sealed class Handlers : SynchronizedCollection<IOperation<object>>;
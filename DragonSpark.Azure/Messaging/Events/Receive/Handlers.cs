using DragonSpark.Model.Operations;
using System.Collections.Generic;

namespace DragonSpark.Azure.Messaging.Events.Receive;

public sealed class Handlers : SynchronizedCollection<IOperation<object>>;
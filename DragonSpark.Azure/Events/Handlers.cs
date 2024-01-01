using DragonSpark.Model.Operations;
using System.Collections.Generic;

namespace DragonSpark.Azure.Events;

public sealed class Handlers : SynchronizedCollection<IOperation<object>>;
using DragonSpark.Model.Operations.Results;
using Medallion.Threading;

namespace DragonSpark.Azure.Storage;

public interface IDistributedLock : IResulting<IDistributedSynchronizationHandle>;
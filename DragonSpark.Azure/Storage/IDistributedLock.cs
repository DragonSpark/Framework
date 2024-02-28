using DragonSpark.Model.Operations.Results;
using DragonSpark.Model.Operations.Selection;
using Medallion.Threading;

namespace DragonSpark.Azure.Storage;

public interface IDistributedLock : IResulting<IDistributedSynchronizationHandle>;

public interface IDistributedLock<in T> : ISelecting<T, IDistributedSynchronizationHandle>;
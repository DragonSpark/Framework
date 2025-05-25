using DragonSpark.Model.Operations.Results.Stop;
using DragonSpark.Model.Operations.Selection.Stop;
using Medallion.Threading;

namespace DragonSpark.Azure.Storage;

public interface IDistributedLock : IStopAware<IDistributedSynchronizationHandle>;

public interface IDistributedLock<T> : IStopAware<T, IDistributedSynchronizationHandle>;
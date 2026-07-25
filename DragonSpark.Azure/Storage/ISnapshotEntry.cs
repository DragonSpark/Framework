using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Azure.Storage;

public interface ISnapshotEntry : IStopAware, IAsyncDisposable;
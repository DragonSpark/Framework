using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Azure.Storage;

public interface ISnapshots : IStopAware<ReadOnlyMemory<string>, ISnapshotEntry>;
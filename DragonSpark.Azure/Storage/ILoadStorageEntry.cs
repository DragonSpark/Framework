using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Azure.Storage;

public interface ILoadStorageEntry : IStopAware<EntryInput, IStorageEntry>;
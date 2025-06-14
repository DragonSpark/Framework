using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Azure.Storage;

public interface INewStorageEntry : IStopAware<EntryInput, IStorageEntry>;
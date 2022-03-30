using DragonSpark.Model.Operations;

namespace DragonSpark.Azure.Storage;

public interface ISaveContent : ISelecting<NewStorageEntryInput, IStorageEntry> {}
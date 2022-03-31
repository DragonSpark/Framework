using DragonSpark.Model.Operations;

namespace DragonSpark.Azure.Storage;

public interface ICopy : ISelecting<DestinationInput, IStorageEntry> {}
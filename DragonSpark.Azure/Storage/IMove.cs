using DragonSpark.Model.Operations;

namespace DragonSpark.Azure.Storage;

public interface IMove : ISelecting<DestinationInput, IStorageEntry> {}
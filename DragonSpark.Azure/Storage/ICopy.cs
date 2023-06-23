using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Azure.Storage;

public interface ICopy : ISelecting<DestinationInput, IStorageEntry> {}
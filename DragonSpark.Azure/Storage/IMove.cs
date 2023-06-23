using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Azure.Storage;

public interface IMove : ISelecting<DestinationInput, IStorageEntry> {}
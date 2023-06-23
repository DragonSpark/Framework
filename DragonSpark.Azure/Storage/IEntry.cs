using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Azure.Storage;

public interface IEntry : ISelecting<string, IStorageEntry?> {}
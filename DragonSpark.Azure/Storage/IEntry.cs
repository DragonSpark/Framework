using DragonSpark.Model.Operations;

namespace DragonSpark.Azure.Storage;

public interface IEntry : ISelecting<string, IStorageEntry?> {}
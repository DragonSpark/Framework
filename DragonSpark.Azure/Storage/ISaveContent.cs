using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Azure.Storage;

public interface ISaveContent : ISelecting<WriteInput, IStorageEntry>;
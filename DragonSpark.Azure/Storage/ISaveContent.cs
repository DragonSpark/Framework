using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Azure.Storage;

public interface ISaveContent : IStopAware<WriteInput, IStorageEntry>;
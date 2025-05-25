using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Azure.Storage;

public interface ICopy : IStopAware<DestinationInput, IStorageEntry>;
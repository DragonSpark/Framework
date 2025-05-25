using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Azure.Storage;

public interface IMove : IStopAware<DestinationInput, IStorageEntry>;
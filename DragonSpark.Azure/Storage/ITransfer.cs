using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Azure.Storage;

public interface ITransfer : IStopAware<DestinationInput, IStorageEntry>;
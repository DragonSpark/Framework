using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Azure.Storage;

public interface IEntry : IStopAware<string, IStorageEntry?>;
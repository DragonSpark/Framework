using Azure.Storage.Blobs.Specialized;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Azure.Storage;

public interface IAppend : IStopAware<AppendInput, BlobBaseClient>;
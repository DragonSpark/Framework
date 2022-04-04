using Azure.Storage.Blobs.Specialized;
using DragonSpark.Model.Operations;

namespace DragonSpark.Azure.Storage;

public interface IAppend : ISelecting<AppendInput, BlobBaseClient> {}
using Azure.Storage.Blobs.Specialized;
using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Azure.Storage;

public interface IAppend : ISelecting<AppendInput, BlobBaseClient> {}
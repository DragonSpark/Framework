using Azure.Storage.Blobs;
using DragonSpark.Model.Operations;

namespace DragonSpark.Azure.Storage;

public interface IWrite : ISelecting<NewStorageEntryInput, BlobClient> {}
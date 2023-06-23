using Azure.Storage.Blobs;
using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Azure.Storage;

public interface IWrite : ISelecting<WriteInput, BlobClient> {}
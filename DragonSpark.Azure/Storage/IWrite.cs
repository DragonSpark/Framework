using Azure.Storage.Blobs;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Azure.Storage;

public interface IWrite : IStopAware<WriteInput, BlobClient>;
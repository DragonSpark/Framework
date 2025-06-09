using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;

namespace DragonSpark.Azure.Storage;

public readonly record struct EntryNameInput(BlobBaseClient Client, BlobProperties Properties);
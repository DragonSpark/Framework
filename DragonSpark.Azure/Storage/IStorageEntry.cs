using DragonSpark.Model.Operations;
using System.IO;

namespace DragonSpark.Azure.Storage;

public interface IStorageEntry : IResulting<Stream>, IAltering<Stream>
{
	StorageEntryProperties Properties { get; }
}
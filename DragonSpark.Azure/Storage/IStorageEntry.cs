using DragonSpark.Model.Operations.Results;
using DragonSpark.Model.Operations.Selection;
using System.IO;

namespace DragonSpark.Azure.Storage;

public interface IStorageEntry : IResulting<Stream>, IAltering<Stream>
{
	StorageEntryProperties Properties { get; }
}
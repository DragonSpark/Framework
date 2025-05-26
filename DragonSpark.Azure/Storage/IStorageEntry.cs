using DragonSpark.Model.Operations.Results.Stop;
using DragonSpark.Model.Operations.Selection.Stop;
using System.IO;

namespace DragonSpark.Azure.Storage;

public interface IStorageEntry : IStopAware<Stream>, IAltering<Stream>
{
	StorageEntryProperties Properties { get; }
}
using DragonSpark.Model.Operations.Results.Stop;
using DragonSpark.Model.Operations.Selection.Stop;
using System.Collections.Generic;
using System.IO;

namespace DragonSpark.Azure.Storage;

public interface IStorageEntry : IStopAware<Stream>,
                                 IAltering<Stream>,
                                 Model.Operations.Stop.IStopAware<IDictionary<string, string?>>
{
	StorageEntryProperties Properties { get; }
}
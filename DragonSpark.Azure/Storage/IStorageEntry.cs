using DragonSpark.Model.Operations.Results.Stop;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Azure.Storage;

public interface IStorageEntry : IStopAware<Stream>,
								 IAltering<Stream>,
								 IStopAware<RelayInput, Uri>,
								 Model.Operations.Stop.IStopAware<IDictionary<string, string?>>
{
	StorageEntryProperties Properties { get; }
}
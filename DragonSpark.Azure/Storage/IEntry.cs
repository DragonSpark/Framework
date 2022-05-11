using Azure.Storage.Blobs;
using DragonSpark.Model.Operations;
using DragonSpark.Text;

namespace DragonSpark.Azure.Storage;

public interface IEntry : ISelecting<string, IStorageEntry?> {}

// TODO

public interface IPath : IFormatter<string> {}

sealed class Path : IPath
{
	readonly string _root;

	public Path(BlobContainerClient client) : this(client.Name) {}

	public Path(string root) => _root = root;

	public string Get(string parameter) => $"{_root}/{parameter}";
}
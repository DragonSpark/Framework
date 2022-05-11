using Azure.Storage.Blobs;

namespace DragonSpark.Azure.Storage;

sealed class Path : IPath
{
	readonly string _root;

	public Path(BlobContainerClient client) : this(client.Name) {}

	public Path(string root) => _root = root;

	public string Get(string parameter) => $"{_root}/{parameter}";
}
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.IO;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage
{
	sealed class SaveContent : ISaveContent
	{
		readonly BlobContainerClient _client;

		public SaveContent(BlobContainerClient client) => _client = client;

		public async ValueTask Get(FileContent parameter)
		{
			var (name, contentType, array) = parameter;
			var             client  = _client.GetBlobClient(name);
			var             header  = new BlobHttpHeaders {ContentType = contentType};
			await using var content = new MemoryStream(array);
			await client.UploadAsync(content, header).ConfigureAwait(false);
		}
	}
}
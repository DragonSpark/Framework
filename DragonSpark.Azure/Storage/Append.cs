﻿using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

sealed class Append : IAppend
{
	readonly BlobContainerClient _client;

	public Append(BlobContainerClient client) => _client = client;

	public async ValueTask<BlobBaseClient> Get(AppendInput parameter)
	{
		var (name, contentType, content) = parameter;
		var result = _client.GetAppendBlobClient(name);
		await result.CreateIfNotExistsAsync(new BlobHttpHeaders { ContentType = contentType }).Await();
		await result.AppendBlockAsync(content).Await();
		return result;
	}
}
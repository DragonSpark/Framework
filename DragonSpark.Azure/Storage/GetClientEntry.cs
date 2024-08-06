﻿using Azure.Storage.Blobs.Specialized;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

sealed class GetClientEntry : ISelecting<BlobBaseClient, DefaultStorageEntry>
{
	public static GetClientEntry Default { get; } = new();

	GetClientEntry() {}

	public async ValueTask<DefaultStorageEntry> Get(BlobBaseClient parameter)
	{
		var response = await parameter.GetPropertiesAsync().Await();
		var value    = response.Value;
		var properties = new StorageEntryProperties(parameter.Uri, parameter.Name, value.ContentType,
		                                            (ulong)value.ContentLength, value.CreatedOn, value.LastModified,
		                                            value.ETag);
		return new(parameter, properties);
	}
}
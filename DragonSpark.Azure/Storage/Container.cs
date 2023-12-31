using Azure.Core;
using Azure.Storage.Blobs;
using DragonSpark.Azure.Data;
using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Azure.Storage;

public class Container : Instance<BlobContainerClient>, IContainer
{
	protected Container(AzureStorageConfiguration configuration, string name)
		: this(configuration.Namespace, name.ToLowerInvariant(), DefaultCredential.Default) {}

	protected Container(string @namespace, string name, TokenCredential credential)
		: base(new(new Uri($"https://{@namespace}.blob.core.windows.net/{name}"), credential)) {}
}
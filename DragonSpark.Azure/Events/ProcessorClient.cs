﻿using Azure.Messaging.EventHubs;
using Azure.Storage.Blobs;
using DragonSpark.Azure.Data;
using DragonSpark.Model.Results;

namespace DragonSpark.Azure.Events;

public class ProcessorClient : Instance<EventProcessorClient>
{
	protected ProcessorClient(EventHubConfiguration configuration, BlobContainerClient container, string name)
		: base(new EventProcessorClient(container, configuration.Group,
		                                $"{configuration.Namespace}.servicebus.windows.net", name,
		                                DefaultCredential.Default)) {}
}
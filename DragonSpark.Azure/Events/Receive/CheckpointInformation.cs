using Azure.Messaging.EventHubs.Processor;
using DragonSpark.Runtime;
using System;

namespace DragonSpark.Azure.Events.Receive;

public sealed record CheckpointInformation(DateTimeOffset Last, ProcessEventArgs? Subject)
{
	public CheckpointInformation() : this(Time.Default, null) {}
}
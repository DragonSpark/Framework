using Azure.Messaging.EventHubs.Processor;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection.Stores;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Messaging.Events.Receive;

sealed class CheckpointTracker : IOperation<ProcessEventArgs>
{
	public static CheckpointTracker Default { get; } = new();

	CheckpointTracker() : this(Checkpoints.Default) {}

	readonly ITable<string, ProcessCheckpoint> _checkpoints;

	public CheckpointTracker(ITable<string, ProcessCheckpoint> checkpoints) => _checkpoints = checkpoints;

	public ValueTask Get(ProcessEventArgs parameter)
		=> _checkpoints.Get(parameter.Partition.PartitionId).Get(parameter);
}
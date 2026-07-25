using DragonSpark.Contracts.Messaging;
using DragonSpark.Model.Operations;
using System.Threading.Channels;

namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

sealed class WriteMessage : IWriteMessage
{
	readonly ChannelWriter<DurableMessageProperties> _writer;
	
	public WriteMessage(ChannelWriter<DurableMessageProperties> writer) => _writer = writer;

	public ValueTask Get(Stop<DurableMessageProperties> parameter)
	{
		_writer.TryWrite(parameter);
		return ValueTask.CompletedTask;
	}
}
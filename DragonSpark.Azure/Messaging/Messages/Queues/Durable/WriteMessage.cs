using System.Threading.Channels;
using System.Threading.Tasks;
using DragonSpark.Contracts.Messaging;
using DragonSpark.Model.Operations;

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
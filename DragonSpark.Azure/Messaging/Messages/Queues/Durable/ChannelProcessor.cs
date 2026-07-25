using DragonSpark.Compose;
using DragonSpark.Contracts.Messaging;
using DragonSpark.Model.Operations.Stop;
using System.Threading.Channels;

namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

sealed class ChannelProcessor : IStopAware
{
	readonly ChannelReader<DurableMessageProperties> _reader;
	readonly ProcessMessage                          _process;

	public ChannelProcessor(ChannelReader<DurableMessageProperties> reader, ProcessMessage process)
	{
		_reader  = reader;
		_process = process;
	}

	public async ValueTask Get(CancellationToken parameter)
	{
		while (await _reader.WaitToReadAsync(parameter).Off())
		{
			while (_reader.TryRead(out var message) && await _process.Off(new(message, parameter))) {}
		}
	}
}
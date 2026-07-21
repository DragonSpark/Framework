using DragonSpark.Compose;
using DragonSpark.Contracts.Messaging;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

sealed class ChannelProcessorBackgroundService : BackgroundService
{
	readonly ChannelReader<DurableMessageProperties> _reader;
	readonly ProcessMessage                              _process;

	public ChannelProcessorBackgroundService(ProcessMessage process)
		: this(ProcessChannel.Default, process) {}

	public ChannelProcessorBackgroundService(Channel<DurableMessageProperties> channel, ProcessMessage process)
		: this(channel.Reader, process) {}

	public ChannelProcessorBackgroundService(ChannelReader<DurableMessageProperties> reader, ProcessMessage process)
	{
		_reader  = reader;
		_process = process;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (await _reader.WaitToReadAsync(stoppingToken).Off())
		{
			while (_reader.TryRead(out var item) && await _process.Off(new(item, stoppingToken))) {}
		}
	}
}